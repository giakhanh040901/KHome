using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.CoreDataUtils;
using EPIC.CoreSharedEntities.Dto.BusinessCustomer;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using EPIC.FileEntities.Settings;
using EPIC.FillContractData.Dto;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.FillContractFile.Services
{
    public abstract class FillDataContractFileBaseServices
    {
        protected readonly EpicSchemaDbContext _dbContext;
        protected readonly ILogger _logger;
        protected readonly IConfiguration _configuration;
        protected readonly string _connectionString;
        protected readonly SharedMediaApiUtils _sharedMediaApiUtils;
        protected readonly SharedSignServerApiUtils _sharedSignServerApiUtils;
        protected readonly IHttpContextAccessor _httpContext;
        protected readonly IOptions<FileConfig> _fileConfig;
        protected readonly IOptions<UrlConfirmReceiveContract> _urlConfirmReceiveContract;
        protected readonly CifCodeEFRepository _cifCodeEFRepository;
        protected readonly InvestorEFRepository _investorEFRepository;
        protected readonly DefErrorEFRepository _defErrorEFRepository;
        protected readonly TradingProviderEFRepository _tradingProviderEFRepository;
        protected readonly BusinessCustomerEFRepository _businessCustomerEFRepository;

        public FillDataContractFileBaseServices(EpicSchemaDbContext dbContext,
            ILogger<FillDataContractFileBaseServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            SharedMediaApiUtils sharedMediaApiUtils,
            SharedSignServerApiUtils sharedSignServerApiUtils,
            IHttpContextAccessor httpContext,
            IOptions<FileConfig> fileConfig,
            IOptions<UrlConfirmReceiveContract> urlConfirmReceiveContract
            )
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _sharedMediaApiUtils = sharedMediaApiUtils;
            _sharedSignServerApiUtils = sharedSignServerApiUtils;
            _httpContext = httpContext;
            _fileConfig = fileConfig;
            _urlConfirmReceiveContract = urlConfirmReceiveContract;
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Save contract không ký, sinh file word, pdf, pdf chưa ký
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public virtual async Task<SaveFileDto> SaveContract(SaveContractInputBaseDto input, string contractTemplateUrl = null)
        {
            _logger.LogInformation($"{nameof(SaveContract)}: input = {JsonSerializer.Serialize(input)}");

            if (input.TradingProviderId == null)
                input.TradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new SaveFileDto();

            string filePath = _fileConfig.Value.Path;
            var fileResult = FileUtils.GetPhysicalPath(contractTemplateUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File vật lý của hợp đồng {contractTemplateUrl} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;
            List<Task> tasks = new();
            int pageCount = 1;
            byte[] byteArray = File.ReadAllBytes(fullPath);

            MemoryStream memoryStreamFileThuong = new();
            await memoryStreamFileThuong.WriteAsync(byteArray, 0, byteArray.Length);
            #region xử lý file không có ảnh con dấu
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memoryStreamFileThuong, true))
            {
                var mainPart = wordDoc.MainDocumentPart;
                string docText = "";

                using (StreamReader sr = new(mainPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                docText = FindAndReplace(docText, input.ReplaceTexts);
                using (StreamWriter sw = new(mainPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
                try
                {
                    mainPart.Document.Save();
                }
                catch (XmlException ex)
                {
                    _logger.LogError(ex, "file word lỗi dạng XML");
                    _defErrorEFRepository.ThrowException(ErrorCode.FileWordXmlValid);
                }

                if (wordDoc.ExtendedFilePropertiesPart.Properties.Pages?.Text != null)
                {
                    pageCount = int.Parse(wordDoc.ExtendedFilePropertiesPart.Properties.Pages.Text.Trim());
                }
                else
                {
                    //Nếu Page null mặc định lấy trang ký là trang đầu tiên
                    _logger.LogError("wordDoc.ExtendedFilePropertiesPart.Properties.Pages = null");
                }

            }
            var fileNameNew = GenerateNewFileName(fileName);

            byte[] byteArrayThuong = memoryStreamFileThuong.ToArray();
            tasks.Add(File.WriteAllBytesAsync(Path.Combine(filePath, fileResult.Folder, fileNameNew), byteArrayThuong));

            var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);

            result.FileTempUrl = GetEndPoint(fileResult.Folder, fileNameNew);
            result.FileName = fileNamePdf;
            result.FileSignatureUrl = GetEndPoint(fileResult.Folder, fileNamePdf);

            tasks.Add(_sharedMediaApiUtils.ConvertWordToPdfAsync(byteArrayThuong, Path.Combine(filePath, fileResult.Folder, fileNamePdf)));
            #endregion

            #region xử lý ảnh con dấu
            var tradingProvider = _tradingProviderEFRepository.FindById(input.TradingProviderId ?? 0);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.TradingProviderNotFound);
            }

            MemoryStream memoryStreamFileConDau = new();
            await memoryStreamFileConDau.WriteAsync(byteArrayThuong, 0, byteArrayThuong.Length);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memoryStreamFileConDau, true))
            {
                wordDoc.ChangeDocumentType(WordprocessingDocumentType.Document);

                var mainPart = wordDoc.MainDocumentPart;
                await FillStampImage(tradingProvider.StampImageUrl, mainPart, filePath);
                try
                {
                    mainPart.Document.Save();
                }
                catch (XmlException ex)
                {
                    _logger.LogError(ex, "file word lỗi dạng XML");
                    _defErrorEFRepository.ThrowException(ErrorCode.FileWordXmlValid);
                }
                if (wordDoc.ExtendedFilePropertiesPart.Properties.Pages?.Text != null)
                {
                    pageCount = int.Parse(wordDoc.ExtendedFilePropertiesPart.Properties.Pages.Text.Trim());
                }
                else
                {
                    _logger.LogError("wordDoc.ExtendedFilePropertiesPart.Properties.Pages = null");
                }

            }
            //Tạo file -Sign.pdf
            await _sharedMediaApiUtils.ConvertWordToPdfAsync(memoryStreamFileConDau.ToArray(), Path.Combine(filePath, fileResult.Folder, fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_PDF)));
            #endregion
            //đẩy các nhiệm vụ xử lý file ra ngoài
            await Task.WhenAll(tasks);
            return result;
        }

        /// <summary>
        /// Tải file hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="contractTemplateTempUrl"></param>
        /// <returns></returns>
        public virtual async Task<ExportResultDto> ExportFileContract(ExportContractInputDtoBase input, string contractTemplateTempUrl = null)
        {
            _logger.LogInformation($"{nameof(ExportFileContract)}: input = {JsonSerializer.Serialize(input)}");
            string filePath = _fileConfig.Value.Path;
            var result = new ExportResultDto();

            //Chuyển path db sang path vật lý
            var fileResult = FileUtils.GetPhysicalPath(contractTemplateTempUrl, filePath);

            //Check file có tồn tại không
            if (!File.Exists(fileResult.FullPath))
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerContractTemplateNotFound);
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            //set tên file khi tải xuống
            result.fileDownloadName = fileName;
            //Get Data
            var replaceText = GetReplaceTextContractFile(input);
            byte[] byteArray = File.ReadAllBytes(fullPath);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
                {
                    var mainPart = wordDoc.MainDocumentPart;
                    string docText = "";

                    using (StreamReader sr = new StreamReader(mainPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }
                    docText = FindAndReplace(docText, replaceText);
                    using (StreamWriter sw = new StreamWriter(mainPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }

                    try
                    {
                        mainPart.Document.Save();
                    }
                    catch (XmlException)
                    {
                        _logger.LogError("file word lỗi dạng XML");
                        _defErrorEFRepository.ThrowException(ErrorCode.FileWordXmlValid);
                    }
                }
                //sinh ra tên file mới từ tên file mẫu
                //đổi đuôi thành pdf
                //tên file khi tải về
                result.fileDownloadName = GenerateNewFileName(fileName).Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                //convert sang file pdf
                result.fileData = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
            }
            return result;
        }

        /// <summary>
        /// Lấy thông tin khác hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual List<ReplaceTextDto> GetReplaceTextCustomer(ContractFileInputCustomerDtoBase input)
        {
            var createdDate = DateTime.Now;
            input.InvestorId ??= CommonUtils.GetCurrentInvestorId(_httpContext);
            InvestorDataForContractDto investor = new();
            BusinessCustomerForContractDto businessCustomer = new();
            if (input.InvestorId != null)
            {
                //Lấy địa chỉ mặc định của khách hàng
                var defaultContactAddress = _investorEFRepository.GetDefaultContactAddress(input.InvestorId ?? 0);
                investor = _investorEFRepository.GetDataInvestorForContract(input.InvestorId ?? 0, input.IdentificationId, input.BankAccId, defaultContactAddress?.ContactAddressId ?? 0);
                businessCustomer = null;
            }
            else
            {
                businessCustomer = _businessCustomerEFRepository.GetBusinessCustomerForContract(input.BusinessCustomerId ?? 0, input.TradingBankAccId);
                investor = null;
            }
            var tradingProvider = _tradingProviderEFRepository.GetTradingProviderForContract(input.TradingProviderId, input.TradingBankAccId);

            List<ReplaceTextDto> result = CoreBaseData.GetBaseDataForContract(createdDate, investor, businessCustomer, tradingProvider, null, input.IsSign); //False là chưa ký, NULL là orderSource = null
            return result;
        }

        /// <summary>
        /// Lấy replace text cho tất cả hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract List<ReplaceTextDto> GetReplaceTextContractFile(ContractFileInputDtoBase input);

        /// <summary>
        /// Tải hợp đồng (Id của bảng Order contract File)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public virtual ExportResultDto ExportContract(ExportOrderContractFileDto input)
        {
            _logger.LogInformation($"{nameof(ExportFileContract)}: input = {JsonSerializer.Serialize(input)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            ErrorCode fileNotExists = ErrorCode.GarnerOrderContractFileNotFound;
            Dictionary<string, string> path = null;
            if (input.ContractType == ContractFileTypes.TEMP)
            {
                if (input.FileTempUrl == null)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderContractTempWordNotFound);
                }
                path = GetParams(input.FileTempUrl);
                fileNotExists = ErrorCode.GarnerOrderContractTempWordNotFound;
            }
            else if (input.ContractType == ContractFileTypes.PDF)
            {
                if (input.FileTempPdfUrl == null)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderContractTempPdfNotFound);
                }
                path = GetParams(input.FileTempPdfUrl);
                fileNotExists = ErrorCode.GarnerOrderContractTempPdfNotFound;
            }
            else if (input.ContractType == ContractFileTypes.SCAN)
            {
                if (input.FileScanUrl == null)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderContractScanNotFound);
                }
                path = GetParams(input.FileScanUrl);
                fileNotExists = ErrorCode.GarnerOrderContractScanNotFound;
            }
            else if (input.ContractType == ContractFileTypes.SIGNATURE)
            {
                if (input.FileSignatureUrl == null)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderContractSignatureNotFound);
                }
                path = GetParams(input.FileSignatureUrl);
                fileNotExists = ErrorCode.GarnerOrderContractSignatureNotFound;
            }


            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName;
            string filePath = _fileConfig.Value.Path;
            var fullPath = GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                _defErrorEFRepository.ThrowException(fileNotExists);
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }
    }
}
