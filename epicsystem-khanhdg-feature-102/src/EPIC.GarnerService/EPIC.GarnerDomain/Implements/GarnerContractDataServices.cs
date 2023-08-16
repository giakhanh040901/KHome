using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Bibliography;
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
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.GarnerRepositories;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.EnumType;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SignPdfDto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerContractDataServices : IGarnerContractDataServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<GarnerContractDataServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly SharedMediaApiUtils _sharedMediaApiUtils;
        private readonly SharedSignServerApiUtils _sharedSignServerApiUtils;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly IOptions<UrlConfirmReceiveContract> _urlConfirmReceiveContract;
        private readonly GarnerContractTemplateEFRepository _garnerContractTemplateRepo;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerPolicyDetailEFRepository _garnerPolicyDetailEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly GarnerOrderContractFileEFRepository _garnerOrderContractFileEFRepository;
        private readonly GarnerProductPriceEFRepository _garnerProductPriceEFRepository;
        private readonly SysVarEFRepository _sysVarEFRepository;
        private readonly GarnerDistributionTradingBankAccountRepository _garnerDistributionTradingBankAccountRepository;
        private readonly GarnerWithdrawalEFRepository _garnerWithdrawalEFRepository;
        private readonly GarnerReceiveContractTemplateEFRepository _garnerReceiveContractTemplateEFRepository;
        private readonly IGarnerFormulaServices _garnerFormulaServices;

        public GarnerContractDataServices(EpicSchemaDbContext dbContext,
           ILogger<GarnerContractDataServices> logger,
           IConfiguration configuration,
           DatabaseOptions databaseOptions,
           IHttpContextAccessor httpContext,
           SharedMediaApiUtils sharedMediaApiUtils,
           SharedSignServerApiUtils sharedSignServerApiUtils,
           IOptions<FileConfig> fileConfig,
           IOptions<UrlConfirmReceiveContract> urlConfirmReceiveContract,
           IGarnerFormulaServices garnerFormulaServices
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
            _garnerContractTemplateRepo = new GarnerContractTemplateEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, logger);
            _garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(dbContext, logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _garnerOrderContractFileEFRepository = new GarnerOrderContractFileEFRepository(dbContext, logger);
            _garnerProductPriceEFRepository = new GarnerProductPriceEFRepository(dbContext, logger);
            _sysVarEFRepository = new SysVarEFRepository(dbContext);
            _garnerDistributionTradingBankAccountRepository = new GarnerDistributionTradingBankAccountRepository(dbContext, logger);
            _garnerWithdrawalEFRepository = new GarnerWithdrawalEFRepository(dbContext, logger);
            _garnerReceiveContractTemplateEFRepository = new GarnerReceiveContractTemplateEFRepository(dbContext, logger);
            _garnerFormulaServices = garnerFormulaServices;
        }

        /// <summary>
        /// Save contract không ký, sinh file word, pdf, pdf chưa ký
        /// </summary>
        /// <param name="contractTemplateType"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public async Task<SaveFileDto> SaveContract(string contractTemplateType, int contractTemplateId, List<ReplaceTextDto> replaceTexts, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(SaveContract)}: contractTemplateType = {contractTemplateType}, contractTemplateId = {contractTemplateId}, tradingProviderId = {tradingProviderId}");
            if (tradingProviderId == null)
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new SaveFileDto();
            var contractTemplate = _garnerContractTemplateRepo.FindByIdForUpdateContractFile(contractTemplateId, contractTemplateType, tradingProviderId ?? 0);
            if (contractTemplate == null || contractTemplate?.ContractTemplateUrl == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.InvestContractTemplateNotFound);
            }
            string filePath = _fileConfig.Value.Path;
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTemplateUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
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
                string docText = null;

                using (StreamReader sr = new(mainPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                docText = FindAndReplace(docText, replaceTexts);
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
            var tradingProvider = _tradingProviderEFRepository.FindById(tradingProviderId ?? 0);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.TradingProviderNotFound);
            }

            tasks.Add(Task.Run(async () =>
            {
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
                        //Nếu Page null mặc định lấy trang ký là trang đầu tiên
                        _logger.LogError("wordDoc.ExtendedFilePropertiesPart.Properties.Pages = null");
                    }

                }
                var fileNameSignPdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_PDF);
                await _sharedMediaApiUtils.ConvertWordToPdfAsync(memoryStreamFileConDau.ToArray(), Path.Combine(filePath, fileResult.Folder, fileNameSignPdf));
                result.FileSignatureStampUrl = GetEndPoint(fileResult.Folder, fileNameSignPdf);
            }));
            #endregion
            result.PageSign = pageCount;
            //đẩy các nhiệm vụ xử lý file ra ngoài
            await Task.WhenAll(tasks);
            return result;
        }

        /// <summary>
        /// Tải hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public ExportResultDto ExportFileContract(int id, string contractType)
        {
            _logger.LogInformation($"{nameof(ExportFileContract)}: id = {id}, contractType = {contractType}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var secondaryContractFile = _garnerOrderContractFileEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderContractFileNotFound);
            ErrorCode fileNotExists = ErrorCode.GarnerOrderContractFileNotFound;
            Dictionary<string, string> path = null;
            if (contractType == ContractFileTypes.TEMP)
            {
                if (secondaryContractFile.FileTempUrl == null)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderContractTempWordNotFound);
                }
                path = GetParams(secondaryContractFile.FileTempUrl);
                fileNotExists = ErrorCode.GarnerOrderContractTempWordNotFound;
            }
            else if (contractType == ContractFileTypes.PDF)
            {
                if (secondaryContractFile.FileTempPdfUrl == null)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderContractTempPdfNotFound);
                }
                path = GetParams(secondaryContractFile.FileTempPdfUrl);
                fileNotExists = ErrorCode.GarnerOrderContractTempPdfNotFound;
            }
            else if (contractType == ContractFileTypes.SCAN)
            {
                if (secondaryContractFile.FileScanUrl == null)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderContractScanNotFound);
                }
                path = GetParams(secondaryContractFile.FileScanUrl);
                fileNotExists = ErrorCode.GarnerOrderContractScanNotFound;
            }
            else if (contractType == ContractFileTypes.SIGNATURE)
            {
                if (secondaryContractFile.FileSignatureUrl == null)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderContractSignatureNotFound);
                }
                path = GetParams(secondaryContractFile.FileSignatureUrl);
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

        #region Ký điện tử
        public SaveFileDto SignContractPdf(long orderContractFileId, int contractTemplateId, int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: orderContractFileId = {orderContractFileId}, contractTemplateId = {contractTemplateId}, tradingProviderId = {tradingProviderId}");
            var dto = GetSignPdfDto(tradingProviderId);
            var result = new SaveFileDto();
            var contractTemplate = _garnerContractTemplateRepo.FindByIdForUpdateContractFile(contractTemplateId, SharedContractTemplateType.INVESTOR, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerContractTemplateNotFound);
            var orderContractFile = _garnerOrderContractFileEFRepository.FindById(orderContractFileId, tradingProviderId);
            if (orderContractFile == null || orderContractFile?.FileSignatureUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy bản ghi file hợp đồng: {contractTemplate.Name}"), new FaultCode(((int)ErrorCode.GarnerOrderContractFileNotFound).ToString()), "");
            }
            string filePath = _fileConfig.Value.Path;

            var fileResult = FileUtils.GetPhysicalPath(orderContractFile.FileSignatureUrl, filePath);
            var fileSignPdf = fileResult.FullPath.Replace(".pdf", "-Sign.pdf");
            if (!File.Exists(fileSignPdf))
            {
                throw new FaultException(new FaultReason($"File đã ký của hợp đồng {contractTemplate.Name} không tồn tại: {fileSignPdf}"), new FaultCode(((int)ErrorCode.GarnerOrderContractFileNotFound).ToString()), "");
            }
            var fileName = fileResult.FileName.Replace(".pdf", "-Sign.pdf");
            var fullPath = fileSignPdf;
            byte[] pdfFileBytes = File.ReadAllBytes(fullPath);
            // Đọc file pdf vừa chuyển đổi để ký   
            dto.FilePdfByteArray = pdfFileBytes;
            dto.pageSign = orderContractFile.PageSign;

            var dtoCopy = new RequestSignPdfDto
            {
                AccessKey = dto.AccessKey,
                //Base64Image = dto.Base64Image,
                FileDownloadName = fileName,
                //FilePdf = dto.FilePdf,
                Height = dto.Height,
                SecretKey = dto.SecretKey,
                pageSign = dto.pageSign,
                Server = dto.Server,
                SignatureName = dto.SignatureName,
                TextOut = dto.TextOut,
                TypeSign = dto.TypeSign,
                Width = dto.Width,
                XPoint = dto.XPoint,
                YPoint = dto.YPoint
            };
            _logger.LogInformation($"gọi api ký: {JsonSerializer.Serialize(dtoCopy)}");
            //lưu chữ ký điện tử
            byte[] fileSign = null;
            try
            {
                fileSign = _sharedSignServerApiUtils.RequestSignPdf(dto);
                if (fileSign == null)
                {
                    throw new Exception("fileSign == null");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ký điện tử không thành công. Vui lòng kiểm tra lại cấu hình!");
                throw new FaultException(new FaultReason($"Ký điện tử không thành công. Vui lòng kiểm tra lại cấu hình!"), new FaultCode(((int)ErrorCode.CoreSignPdfFailed).ToString()), "");
            }

            var fileSignName = GenerateNewFileName(fileName);
            //tạo đường dẫn lưu file chữ ký điện tử
            string filePathSign = Path.Combine(filePath, fileResult.Folder, fileSignName);
            File.WriteAllBytes(filePathSign, fileSign);
            result.FileName = fileSignName;
            result.FileSignatureUrl = GetEndPoint("file/get", fileResult.Folder, fileSignName);
            result.FilePathToBeDeleted = fileResult.FullPath; //trả ra file word để xóa
            return result;
        }

        private RequestSignPdfDto GetSignPdfDto(int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: tradingProviderId = {tradingProviderId}");
            var tradingProvider = _tradingProviderEFRepository.FindById(tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.TradingProviderNotFound);
            var businessCustomerTrading = _businessCustomerEFRepository.FindById(tradingProvider.BusinessCustomerId).ThrowIfNull(_dbContext, ErrorCode.CoreBussinessCustomerNotFound);
            var xPoint = int.Parse(_sysVarEFRepository.GetVarByName("SIGN_PDF", "X_POINT")?.VarValue);
            var yPoint = int.Parse(_sysVarEFRepository.GetVarByName("SIGN_PDF", "Y_POINT")?.VarValue);
            var width = int.Parse(_sysVarEFRepository.GetVarByName("SIGN_PDF", "WIDTH")?.VarValue);
            var height = int.Parse(_sysVarEFRepository.GetVarByName("SIGN_PDF", "HEIGHT")?.VarValue);

            var dto = new RequestSignPdfDto()
            {
                TextOut = businessCustomerTrading.Name,
                TypeSign = SignPdfType.TYPE_PDFSIGNATURE_TEXT,
                AccessKey = tradingProvider.Key,
                SecretKey = tradingProvider.Secret,
                Server = tradingProvider.Server,
                XPoint = xPoint,
                YPoint = yPoint,
                Width = width,
                Height = height
            };
            if (dto.AccessKey == null || dto.SecretKey == null || dto.Server == null)
            {
                throw new FaultException(new FaultReason($"Không thể ký điện tử do khách hàng doanh nghiệp thiếu cấu hình"), new FaultCode(((int)ErrorCode.CoreBusinessCustomerSettingSignPdfNotFound).ToString()), "");
            }
            if (!Uri.IsWellFormedUriString(dto.Server, UriKind.Absolute))
            {
                throw new FaultException(new FaultReason($"Cấu hình server ký điện tử không hợp lệ"), new FaultCode(((int)ErrorCode.CoreSignPdfServerNotValid).ToString()), "");
            }
            return dto;
        }
        #endregion

        #region tải file word tạm trên app

        /// <summary>
        /// Fill Data file tạm app order
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="policyId"></param>
        /// <param name="BankAccId"></param>
        /// <param name="identificationId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="replaceTexts">data cho hợp đồng</param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExportContractApp(decimal totalValue, int policyId, int BankAccId, int identificationId, int contractTemplateId)
        {
            _logger.LogInformation($"{nameof(ExportContractApp)}: totalValue = {totalValue}, policyId = {policyId}, BankAccId = {BankAccId}, identificationId = {identificationId}, contractTemplateId = {contractTemplateId},");
            string filePath = _fileConfig.Value.Path;
            // lấy thông tin chính sách
            var policy = _garnerPolicyEFRepository.FindById(policyId);
            if (policy == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyNotFound);
            }

            //Lấy thông tin bán theo kỳ hạn
            var distribution = _garnerDistributionEFRepository.FindById(policy.DistributionId, policy.TradingProviderId);
            if (distribution == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerDistributionNotFound);
            }
            var result = new ExportResultDto();

            //Lấy mẫu hợp đồng
            var contractTemplate = _garnerContractTemplateRepo.FindByIdForUpdateContractFile(contractTemplateId, SharedContractTemplateType.INVESTOR, distribution.TradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTemplateUrl == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerContractTemplateNotFound);
            }

            //var contractCode = contractTemplate.Code;

            //Chuyển path db sang path vật lý
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTemplateUrl, filePath);

            //Check file có tồn tại không
            if (!File.Exists(fileResult.FullPath))
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerContractTemplateNotFound);
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            //set tên file khi tải xuống
            result.fileDownloadName = fileName;
            //load file
            var replaceTexts = new List<ReplaceTextDto>();
            //Get data
            replaceTexts = GetDataContractFileApp(totalValue, policy, distribution, BankAccId, identificationId, null, "");

            byte[] byteArray = File.ReadAllBytes(fullPath);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
                {
                    var mainPart = wordDoc.MainDocumentPart;
                    string docText = null;

                    using (StreamReader sr = new StreamReader(mainPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }
                    docText = FindAndReplace(docText, replaceTexts);
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
        /// Xem tạm hợp đồng rút tiền app
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="amountMoney"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExportContractWithDrawal(long orderId, int contractTemplateId, decimal amountMoney, int investorBankAccId)
        {
            _logger.LogInformation($"{nameof(ExportContractWithDrawal)}: orderId = {orderId}, contractTemplateId = {contractTemplateId}, amountMoney = {amountMoney}");
            string filePath = _fileConfig.Value.Path;
            var result = new ExportResultDto();
            //Lấy mẫu hợp đồng
            var contractTemplate = _garnerContractTemplateRepo.FindByIdForUpdateContractFile(contractTemplateId, SharedContractTemplateType.INVESTOR, null);
            if (contractTemplate == null || contractTemplate?.ContractTemplateUrl == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerContractTemplateNotFound);
            }
            //Chuyển path db sang path vật lý
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTemplateUrl, filePath);

            //Check file có tồn tại không
            if (!File.Exists(fileResult.FullPath))
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerContractTemplateNotFound);
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            //set tên file khi tải xuống
            result.fileDownloadName = fileName;
            //load file
            var replaceTexts = new List<ReplaceTextDto>();

            #region get data theo phân loại và loại hợp đồng
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            var distribution = _garnerDistributionEFRepository.FindById(policy.DistributionId).ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionNotFound);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var cifCodes = _cifCodeEFRepository.FindByInvestor(investorId);
            replaceTexts = GetDataContractFileApp(0, policy, distribution, investorBankAccId, order?.InvestorIdenId ?? 0, investorId, "");
            replaceTexts.AddRange(GetDataWithdrawalContractFileApp(order.PolicyId, cifCodes?.CifCode, amountMoney));
            #endregion

            byte[] byteArray = File.ReadAllBytes(fullPath);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
                {
                    var mainPart = wordDoc.MainDocumentPart;
                    string docText = null;

                    using (StreamReader sr = new StreamReader(mainPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }
                    docText = FindAndReplace(docText, replaceTexts);
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

        #endregion

        #region Lấy data cho hợp đồng garner
        /// <summary>
        /// Lấy dữ liệu để fill data hợp đồng
        /// </summary>
        /// <param name="order"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="isSignature"></param>
        /// <param name="withdrawalId"></param>
        /// <param name="amountMoney"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataContractFile(GarnerOrder order, int tradingProviderId, bool isSignature, int? configContractId = null)
        {
            _logger.LogInformation($"{nameof(GetDataContractFile)}: order = {JsonSerializer.Serialize(order)}, tradingProviderId = {tradingProviderId}, isSignature = {isSignature}, configContractId = {configContractId}");
            List<ReplaceTextDto> replaceTexts = new();
            var createdDate = order.PaymentFullDate ?? DateTime.Now;
            DateTime paymentFullDate = order.PaymentFullDate ?? DateTime.Now;
            InvestorDataForContractDto investor = new();
            BusinessCustomerForContractDto businessCustomer = new();
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            if (cifCode.InvestorId != null)
            {
                var investorId = cifCode.InvestorId;
                //Lấy địa chỉ mặc định của khách hàng
                var defaultContactAddress = _investorEFRepository.GetDefaultContactAddress(investorId ?? 0);
                investor = _investorEFRepository.GetDataInvestorForContract(investorId ?? 0, order.InvestorIdenId ?? 0, order.InvestorBankAccId ?? 0, defaultContactAddress?.ContactAddressId ?? 0);
                businessCustomer = null;
            }
            else
            {
                var businessCustomerId = cifCode.BusinessCustomerId;
                businessCustomer = _businessCustomerEFRepository.GetBusinessCustomerForContract(businessCustomerId ?? 0, order.BusinessCustomerBankAccId ?? 0);
                investor = null;
            }

            //var policyDetail = _garnerPolicyDetailEFRepository.FindById(order.PolicyDetailId ?? 0, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyDetailNotFound);
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId, order.TradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            //Lấy thông tin bán theo kỳ hạn
            var distribution = _garnerDistributionEFRepository.FindById(policy.DistributionId, policy.TradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionNotFound);
            var tradingProvider = _tradingProviderEFRepository.GetTradingProviderForContract(policy.TradingProviderId, order.TradingBankAccId);
            var product = _garnerProductEFRepository.FindById(order.ProductId).ThrowIfNull(_dbContext, ErrorCode.GarnerProductNotFound);
            if (configContractId != null)
            {
                string productType = ConfigContractCode.ProductTypes(product.ProductType);
                var contractCode = _garnerOrderEFRepository.GenContractCode(new OrderContractCodeDto
                {
                    OrderId = order.Id,
                    ConfigContractCodeId = configContractId.Value,
                    Now = DateTime.Now,
                    PolicyCode = policy.Code,
                    PolicyName = policy.Name,
                    ProductCode = product.Code,
                    ProductName = product.Name,
                    ProductType = productType,
                    BuyDate = order.BuyDate,
                    PaymentFullDate = order.PaymentFullDate,
                    InvestDate = order.InvestDate,
                    ShortName = investor?.InvestorIdentification?.Fullname,
                    ShortNameBusiness = businessCustomer?.BusinessCustomer?.ShortName
                });
                replaceTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                    new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                });
            }
            replaceTexts.AddRange(CoreBaseData.GetBaseDataForContract(createdDate, investor, businessCustomer, tradingProvider, order.Source, isSignature));
            replaceTexts.AddRange(GetDataForContract(product, order));
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.TRAN_DATE, paymentFullDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.POLICY_NAME, policy.Name),
            });
            return replaceTexts;
        }

        /// <summary>
        /// Lấy dữ liệu để fill data hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="isSignature"></param>
        /// <param name="withdrawalId"></param>
        /// <param name="amountMoney"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataContractFile(long orderId, int tradingProviderId, bool isSignature, int? configContractId = null)
        {
            _logger.LogInformation($"{nameof(GetDataContractFile)}: orderId = {orderId}, tradingProviderId = {tradingProviderId}, isSignature = {isSignature}, configContractId = {configContractId}");
            List<ReplaceTextDto> replaceTexts = new();
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            return GetDataContractFile(order, tradingProviderId, isSignature, configContractId);
        }

        /// <summary>
        /// Lấy thêm dữ liệu để fill hợp đồng rút tiền
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="withdrawalId"></param>
        /// <param name="amountMoney"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataWithdrawalContractFile(long orderId, long? withdrawalId = null, decimal? amountMoney = null)
        {
            _logger.LogInformation($"{nameof(GetDataContractFile)}: orderId = {orderId}, withdrawalId = {withdrawalId}, amountMoney = {amountMoney}");
            List<ReplaceTextDto> replaceTexts = new();
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            return GetDataWithdrawalContractFile(order, withdrawalId, amountMoney);

        }


        /// <summary>
        /// Lấy thêm dữ liệu để fill data hợp đồng rút tiền
        /// </summary>
        /// <param name="order"></param>
        /// <param name="withdrawalId"></param>
        /// <param name="amountMoney"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataWithdrawalContractFile(GarnerOrder order, long? withdrawalId = null, decimal? amountMoney = null)
        {
            _logger.LogInformation($"{nameof(GetDataContractFile)}: order = {JsonSerializer.Serialize(order)}, withdrawalId = {withdrawalId}, amountMoney = {amountMoney}");
            List<ReplaceTextDto> replaceTexts = new();
            GarnerWithdrawalDetailDto withdrawalDetail = null;
            DateTime withdrawalApproveDate = DateTime.Now;
            if (withdrawalId != null)
            {
                var withdrawal = _garnerWithdrawalEFRepository.FindById(withdrawalId ?? 0).ThrowIfNull(_dbContext, ErrorCode.GarnerWithdrawalNotFound);
                if (withdrawal.ApproveDate != null)
                {
                    withdrawalApproveDate = withdrawal.ApproveDate.Value;
                }
                else
                {
                    withdrawalApproveDate = withdrawal.CreatedDate.Value;
                }

                withdrawalDetail = _garnerFormulaServices.CalculateWithdrawalDetail(order.Id, withdrawalId ?? 0);
            }
            else
            {
                //đối với trường hợp get data cho hợp đồng tạm khi rút
                withdrawalDetail = _garnerFormulaServices.CalculateWithdrawalDetail(new GarnerOrderWithdrawalDto
                {
                    Order = order,
                    AmountMoney = amountMoney ?? 0,
                });
            }
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            var tax = withdrawalDetail?.Tax;
            //Loại hình lợi tức là NET thì thuế lợi nhuận = 0
            if (policy.CalculateType == CalculateTypes.NET)
            {
                tax = 0;
            }
            replaceTexts.AddRange(
            new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.SIGNATURE_WITHDRAWAL, $"Đã ký {withdrawalApproveDate.ToString("dd-MM-yyyy HH:mm:ss")}"),
                #region tính toán
                new ReplaceTextDto(PropertiesContractFile.ACTUALLY_PROFIT, (double?)withdrawalDetail?.ActuallyProfit, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.AMOUNT_RECEIVED, (double?)withdrawalDetail?.AmountReceived, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.AMOUNT_MONEY, (double?)withdrawalDetail?.AmountMoney, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TAX, (double?)tax, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.DEDUCTIBLE_PROFIT, (double?)withdrawalDetail?.DeductibleProfit, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.PROFIT, (double?)withdrawalDetail?.Profit, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.WITH_DRAWAL_FEE, (double?)withdrawalDetail?.WithdrawalFee, EnumReplaceTextFormat.NumberVietNam)
                #endregion
            });
            return replaceTexts;
        }

        /// <summary>
        /// Lấy thêm dữ liệu theo chính sách để fill data hợp đồng rút tiền
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="cifCode"></param>
        /// <param name="amountMoney"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataWithdrawalContractFileApp(int policyId, string cifCode, decimal amountMoney)
        {
            _logger.LogInformation($"{nameof(GetDataWithdrawalContractFile)}: policyId = {policyId}, amountMoney = {amountMoney}");
            List<ReplaceTextDto> replaceTexts = new();
            List<GarnerOrderWithdrawalDto> orderWithdraw = _garnerWithdrawalEFRepository.CalOrderWithdraw(cifCode, policyId, amountMoney);
            CalculateGarnerWithdrawalDto result = _garnerFormulaServices.CalculateWithdrawal(orderWithdraw);
            var policy = _garnerPolicyEFRepository.FindById(policyId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            var tax = result?.Tax;
            //Loại hình lợi tức là NET thì thuế lợi nhuận = 0
            if (policy.CalculateType == CalculateTypes.NET)
            {
                tax = 0;
            }
            replaceTexts.AddRange(
            new List<ReplaceTextDto>()
            {
                #region tính toán
                new ReplaceTextDto(PropertiesContractFile.ACTUALLY_PROFIT, (double?)result?.ActuallyProfit, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.AMOUNT_RECEIVED, (double?)result?.AmountReceived, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.AMOUNT_MONEY, (double?)amountMoney, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TAX, (double?)tax, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.DEDUCTIBLE_PROFIT, (double?)result?.DeductibleProfit, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.PROFIT, (double?)result?.Profit, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.WITH_DRAWAL_FEE, (double?)result?.WithdrawalFee, EnumReplaceTextFormat.NumberVietNam)
                #endregion
            });
            return replaceTexts;
        }


        /// <summary>
        /// Lấy data hợp đồng tạm trên app đặt lệnh
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="policy"></param>
        /// <param name="bankAccId"></param>
        /// <param name="identificationId"></param>
        /// <param name="contractCode"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataContractFileApp(decimal totalValue, GarnerPolicy policy, GarnerDistribution distribution, int bankAccId, int identificationId, int? investorId, string contractCode, int? contactAddressId = null)
        {
            var createdDate = DateTime.Now;
            investorId ??= CommonUtils.GetCurrentInvestorId(_httpContext);
            var identification = _investorEFRepository.GetIdentificationById(identificationId);
            if (identification?.InvestorId != investorId)
            {
                investorId = identification?.InvestorId;
            }
            //Lấy địa chỉ mặc định của khách hàng
            var defaultContactAddress = _investorEFRepository.GetDefaultContactAddress(investorId ?? 0);
            var investor = _investorEFRepository.GetDataInvestorForContract(investorId ?? 0, identificationId, bankAccId, defaultContactAddress?.ContactAddressId);
            var distributionBankDefault = _garnerDistributionTradingBankAccountRepository
                .EntityNoTracking
                .FirstOrDefault(d => d.DistributionId == distribution.Id && d.Status == Status.ACTIVE)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionTradingBankAccNotFound);
            var tradingProvider = _tradingProviderEFRepository.GetTradingProviderForContract(policy.TradingProviderId, distributionBankDefault.Id);
            List<ReplaceTextDto> result = CoreBaseData.GetBaseDataForContract(createdDate, investor, null, tradingProvider, null, false);
            result.AddRange(
                new List<ReplaceTextDto>() {
                    new ReplaceTextDto(PropertiesContractFile.SIGNATURE_WITHDRAWAL, $"Đã ký {createdDate.ToString("dd-MM-yyyy HH:mm:ss")}"),
                    new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                    new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                });
            var product = _garnerProductEFRepository.FindById(distribution.ProductId).ThrowIfNull(_dbContext, ErrorCode.GarnerProductNotFound);
            var owner = _businessCustomerEFRepository.FindById(product.InvOwnerId ?? 0);
            var productPrice = _garnerProductPriceEFRepository.FindProductPriceByDistributionAndPriceDate(distribution.Id, DateTime.Now, distribution.TradingProviderId);
            decimal quantity = 0;
            decimal price = 0;
            decimal rightPrice = 0;
            if (productPrice?.Price > 0 && totalValue > 0)
            {
                price = Math.Round(productPrice.Price);
                //số lượng = số tiền đầu tư / giá trị trong bảng giá
                quantity = Math.Floor(totalValue / price);
                //giá trị cổ phần = số tiền đầu tư / số lượng
                rightPrice = Math.Round(totalValue / quantity);
            }
            result.AddRange(new List<ReplaceTextDto>()
            {
                #region Sản phẩm
                new ReplaceTextDto(PropertiesContractFile.TRAN_DATE, DateTime.Now, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.POLICY_NAME, policy.Name),
                new ReplaceTextDto(PropertiesContractFile.PRODUCT_NAME, product?.Name),
                new ReplaceTextDto(PropertiesContractFile.PRODUCT_CODE, product?.Code),
                new ReplaceTextDto(PropertiesContractFile.CPS_PAR_VALUE, (double?)product?.CpsParValue, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.CPS_PAR_VALUE_TEXT, (double?)product?.CpsParValue, EnumReplaceTextFormat.NumberToWord),
                new ReplaceTextDto(PropertiesContractFile.CPS_QUANTITY, (double?)quantity, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.CPS_QUANTITY_TEXT, (double?)quantity, EnumReplaceTextFormat.NumberToWord),
                new ReplaceTextDto(PropertiesContractFile.OWNER_NAME, owner?.Name),
                new ReplaceTextDto(PropertiesContractFile.OWNER_SHORT_NAME, owner?.ShortName),
                new ReplaceTextDto(PropertiesContractFile.PRODUCT_PRICE, (double?)rightPrice, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.PRODUCT_PRICE_TEXT, (double?)rightPrice, EnumReplaceTextFormat.NumberToWord),
                #endregion
                
                #region lệnh
                new ReplaceTextDto(PropertiesContractFile.TOTAL_VALUE, (double?)totalValue, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TOTAL_VALUE_TEXT, (double?)totalValue, EnumReplaceTextFormat.NumberToWord),
                #endregion
            });
            return result;
        }

        private List<ReplaceTextDto> GetDataForContract(GarnerProduct product, GarnerOrder order)
        {
            var owner = _businessCustomerEFRepository.FindById(product.InvOwnerId ?? 0);
            var productPrice = _garnerProductPriceEFRepository.FindProductPriceByDistributionAndPriceDate(order.DistributionId, order?.PaymentFullDate ?? DateTime.Now, order?.TradingProviderId);
            List<ReplaceTextDto> replaceTexts = new();
            decimal quantity = 0;
            decimal price = 0;
            decimal rightPrice = 0;
            if (productPrice?.Price > 0 && order?.TotalValue > 0)
            {
                price = Math.Round(productPrice.Price);
                //số lượng = số tiền đầu tư / giá trị trong bảng giá
                quantity = Math.Floor(order.TotalValue / price);
                //giá trị cổ phần = số tiền đầu tư / số lượng
                rightPrice = quantity != 0 ? Math.Round(order.TotalValue / quantity) : 0;
            }
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                #region Sản phẩm
                new ReplaceTextDto(PropertiesContractFile.PRODUCT_NAME, product?.Name),
                new ReplaceTextDto(PropertiesContractFile.PRODUCT_CODE, product?.Code),
                new ReplaceTextDto(PropertiesContractFile.CPS_PAR_VALUE, (double?)product?.CpsParValue, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.CPS_PAR_VALUE_TEXT, (double?)product?.CpsParValue, EnumReplaceTextFormat.NumberToWord),
                new ReplaceTextDto(PropertiesContractFile.CPS_QUANTITY, (double?)quantity, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.CPS_QUANTITY_TEXT, (double?)quantity, EnumReplaceTextFormat.NumberToWord),
                new ReplaceTextDto(PropertiesContractFile.OWNER_NAME, owner?.Name),
                new ReplaceTextDto(PropertiesContractFile.OWNER_SHORT_NAME, owner?.ShortName),
                new ReplaceTextDto(PropertiesContractFile.PRODUCT_PRICE, (double?)rightPrice, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.PRODUCT_PRICE_TEXT, (double?)rightPrice, EnumReplaceTextFormat.NumberToWord),
                #endregion
                
                #region lệnh
                new ReplaceTextDto(PropertiesContractFile.TOTAL_VALUE, (double?)order?.TotalValue, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TOTAL_VALUE_TEXT, (double?)order?.TotalValue, EnumReplaceTextFormat.NumberToWord),
                #endregion
            });
            return replaceTexts;
        }

        ///// <summary>
        ///// Get contract code cho hợp đồng
        ///// </summary>
        ///// <param name="order"></param>
        ///// <param name="product"></param>
        ///// <param name="policy"></param>
        ///// <param name="configContractId"></param>
        ///// <returns></returns>
        //public string GetContractCode(GarnerOrder order, GarnerProduct product, GarnerPolicy policy, int configContractId)
        //{
        //    InvestorDataForContractDto investor = new();
        //    BusinessCustomerForContractDto businessCustomer = new();
        //    var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
        //    if (cifCode.InvestorId != null)
        //    {
        //        var investorId = cifCode.InvestorId;
        //        investor = _investorEFRepository.GetDataInvestorForContract(investorId ?? 0, order.InvestorIdenId ?? 0, order.InvestorBankAccId ?? 0);
        //        businessCustomer = null;
        //    }
        //    else
        //    {
        //        var businessCustomerId = cifCode.BusinessCustomerId;
        //        businessCustomer = _businessCustomerEFRepository.GetBusinessCustomerForContract(businessCustomerId ?? 0, order.InvestorBankAccId ?? 0);
        //        investor = null;
        //    }
        //    string productType = ConfigContractCode.ProductTypes(product.ProductType);
        //    var contractCode = _garnerOrderEFRepository.GenContractCode(new OrderContractCodeDto
        //    {
        //        Id = order.Id,
        //        ConfigContractCodeId = configContractId,
        //        Now = DateTime.Now,
        //        PolicyName= policy.Name,
        //        PolicyCode = policy.Code,
        //        ProductType= productType,
        //        ProductCode = product.Code,
        //        ProductName= product.Name,
        //        BuyDate = order.BuyDate,
        //        PaymentFullDate = order.PaymentFullDate,
        //        InvestDate = order.InvestDate,
        //        ShortName = investor?.InvestorIdentification?.Fullname,
        //        ShortNameBusiness = businessCustomer?.BusinessCustomer?.ShortName
        //    });

        //    return contractCode;
        //}
        #endregion

        #region Hợp đồng giao nhận
        public async Task<ExportResultDto> ExportContractReceive(long orderId, int? tradingProviderId = null)
        {
            string filePath = _fileConfig.Value.Path;
            var result = new ExportResultDto();
            var order = _garnerOrderEFRepository.FindById(orderId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var contractTemplate = _garnerReceiveContractTemplateEFRepository.GetByDistribution(order.DistributionId, order.TradingProviderId);
            if (contractTemplate == null || contractTemplate?.FileUrl == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerReceiveContractTemplateNotFound);
            }

            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.FileUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerReceiveContractTemplateNotFound);
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replaceTexts = new List<ReplaceTextDto>();
            replaceTexts = GetDataContractFile(order, order.TradingProviderId, true);
            replaceTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, order.ContractCode),
                    new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, order.ContractCode),
                });

            byte[] byteArray = File.ReadAllBytes(fullPath);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
                {
                    var mainPart = wordDoc.MainDocumentPart;
                    string docText = null;

                    using (StreamReader sr = new StreamReader(mainPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }
                    docText = FindAndReplace(docText, replaceTexts);
                    using (StreamWriter sw = new StreamWriter(mainPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }

                    string text;
                    string urlConfirmReceiveContract = _urlConfirmReceiveContract.Value.Url;
                    text = $"{urlConfirmReceiveContract}/{order.DeliveryCode}";
                    if (!string.IsNullOrEmpty(text))
                    {
                        var png = _sharedMediaApiUtils.GenQrCode(text).Result;
                        MemoryStream msQrCode = new MemoryStream(png);
                        var picture = mainPart.Document.Descendants<DocumentFormat.OpenXml.Drawing.Pictures.Picture>().FirstOrDefault(p => p.NonVisualPictureProperties.NonVisualDrawingProperties.Name.Value.Contains("Picture"));
                        if (picture != null)
                        {
                            var blip = picture.BlipFill.Blip;
                            ImagePart newImagePath = mainPart.AddImagePart(ImagePartType.Png);
                            newImagePath.FeedData(msQrCode);
                            blip.Embed = mainPart.GetIdOfPart(newImagePath);
                        }
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

        #region Test fill data file hợp đồng giao nhận
        /// <summary>
        /// Test fill data hợp đồng giao nhận
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExportContractReceivePdfTest(int tradingProviderId, int contractTemplateId)
        {
            string filePath = _fileConfig.Value.Path;

            var result = new ExportResultDto();
            var contractTemplate = _garnerReceiveContractTemplateEFRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.FileUrl == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerReceiveContractTemplateNotFound);
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.FileUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerReceiveContractTemplateNotFound);
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replaceTexts = new List<ReplaceTextDto>();
            //Get Data
            replaceTexts = CoreBaseData.GetDataContractFileTest();

            byte[] byteArray = File.ReadAllBytes(fullPath);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
                {
                    var mainPart = wordDoc.MainDocumentPart;
                    string docText = null;

                    using (StreamReader sr = new StreamReader(mainPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }
                    docText = FindAndReplace(docText, replaceTexts);
                    using (StreamWriter sw = new StreamWriter(mainPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }
                    var png = _sharedMediaApiUtils.GenQrCode("Công ty cổ phần Công nghệ STE xin chào quý khách hàng.").Result;
                    MemoryStream msQrCode = new MemoryStream(png);
                    var picture = mainPart.Document.Descendants<DocumentFormat.OpenXml.Drawing.Pictures.Picture>().FirstOrDefault(p => p.NonVisualPictureProperties.NonVisualDrawingProperties.Name.Value.Contains("Picture"));
                    if (picture != null)
                    {
                        var blip = picture.BlipFill.Blip;
                        ImagePart newImagePath = mainPart.AddImagePart(ImagePartType.Png);
                        newImagePath.FeedData(msQrCode);
                        blip.Embed = mainPart.GetIdOfPart(newImagePath);
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

        #endregion
        #endregion
    }
}
