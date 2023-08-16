using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondShared;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.UploadFile;
using EPIC.FileDomain.Services;
using EPIC.FileEntities.Settings;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SignPdfDto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Codecrete.QrCodeGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace EPIC.BondDomain.Implements
{
    public partial class BondContractDataService : IBondContractDataService
    {
        private readonly ILogger<BondContractDataService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly BondOrderRepository _orderRepository;
        private readonly BondOrderPaymentRepository _orderPaymentRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestorBankAccountRepository _investorBankAccountRepository;
        private readonly BondInfoRepository _productBondInfoRepository;
        private readonly BondIssuerRepository _issuerRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly BondContractTemplateRepository _contractTemplateRepository;
        private readonly BondReceiveContractTemplateRepository _receiveContractTemplateRepository;
        private readonly BondSecondPriceRepository _productBondSecondPriceRepository;
        private readonly BondSecondaryContractRepository _bondSecondaryContractRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly SharedMediaApiUtils _sharedMediaApiUtils;
        private readonly SharedSignServerApiUtils _sharedSignServerApiUtils;
        private readonly IFileServices _fileServices;
        private readonly IBondSharedService _bondSharedService;
        private readonly BankRepository _bankRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly BondDepositProviderRepository _depositProviderRepository;
        private readonly SysVarRepository _sysVarRepository;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly IOptions<UrlConfirmReceiveContract> _urlConfirmReceiveContract;
        public BondContractDataService(
            ILogger<BondContractDataService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IFileServices fileServices,
            IBondSharedService bondSharedService,
            SharedMediaApiUtils sharedMediaApiUtils,
            SharedSignServerApiUtils sharedSignServerApiUtils,
            IOptions<FileConfig> fileConfig,
            IOptions<UrlConfirmReceiveContract> urlConfirmReceiveContract)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _orderRepository = new BondOrderRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _orderPaymentRepository = new BondOrderPaymentRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investorBankAccountRepository = new InvestorBankAccountRepository(_connectionString, _logger);
            _productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _issuerRepository = new BondIssuerRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _contractTemplateRepository = new BondContractTemplateRepository(_connectionString, _logger);
            _receiveContractTemplateRepository = new BondReceiveContractTemplateRepository(_connectionString, _logger);
            _productBondSecondPriceRepository = new BondSecondPriceRepository(_connectionString, _logger);
            _bondSecondaryContractRepository = new BondSecondaryContractRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _depositProviderRepository = new BondDepositProviderRepository(_connectionString, _logger);
            _sysVarRepository = new SysVarRepository(_connectionString, _logger);
            _sharedMediaApiUtils = sharedMediaApiUtils;
            _sharedSignServerApiUtils = sharedSignServerApiUtils;
            _fileServices = fileServices;
            _bondSharedService = bondSharedService;
            _fileConfig = fileConfig;
            _urlConfirmReceiveContract = urlConfirmReceiveContract;
        }

        private string FindAndReplace(string docText, List<ReplaceTextDto> replateTextDtos)
        {
            string docTextReplate = docText;
            foreach (var text in replateTextDtos)
            {
                var replaceTextXml = text.ReplaceText;
                if (text.ReplaceText != null)
                {
                    replaceTextXml = HttpUtility.HtmlEncode(text.ReplaceText);
                }
                docTextReplate = docTextReplate.Replace(text.FindText, replaceTextXml);
            }
            return docTextReplate;
        }

        public ExportResultDto ExportContract(int orderId, int contractTemplateId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            string filePath = _fileConfig.Value.Path;

            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTempUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replateTexts = new List<ReplaceTextDto>();
            replateTexts = GetDataContractFile(orderId, tradingProviderId, false);

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
                    //docText = mainPart.Document.Body.InnerText;
                    docText = FindAndReplace(docText, replateTexts);
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
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                result.fileData = ms.ToArray();
            }
            return result;
        }


        public SaveFileDto SaveContract(int orderId, int contractTemplateId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new SaveFileDto();
            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            string filePath = _fileConfig.Value.Path;

            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTempUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;


            //load file
            var replateTexts = new List<ReplaceTextDto>();
            replateTexts = GetDataContractFile(orderId, tradingProviderId, false);

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
                    //docText = mainPart.Document.Body.InnerText;
                    docText = FindAndReplace(docText, replateTexts);

                    using (StreamWriter sw = new StreamWriter(mainPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }
                    #region In ra table đối với hợp đồng phụ lục dòng tiền
                    if (contractTemplate.ContractType == ContractTemplateType.BMHTNTTPVKQDT)
                    {
                        var cashFlows = GetCashFlow(orderId, tradingProviderId);
                        CreateTableCashFlow(cashFlows, mainPart);
                    }
                    #endregion
                    try
                    {
                        mainPart.Document.Save();
                    }
                    catch (XmlException)
                    {
                        _logger.LogError("file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                var fileNameNew = ContractDataUtils.GenerateNewFileName(fileName);
                result.FileName = fileNameNew;
                result.FileSignatureUrl = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileNameNew);
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
            }
            return result;
        }

        public async Task<SaveFileDto> SaveContractPdfNotSign(int orderId, int contractTemplateId, int policyDetailId)
        {
            var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(policyDetailId);
            if (bondPolicyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
            }

            var bondPolicy = _productBondSecondaryRepository.FindPolicyById(bondPolicyDetail.PolicyId, bondPolicyDetail.TradingProviderId);
            if (bondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(bondPolicy.SecondaryId, bondPolicy.TradingProviderId);
            if (bondSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }
            var result = new SaveFileDto();
            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, bondSecondary.TradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }

            string filePath = _fileConfig.Value.Path;       
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTempUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            List<Task> tasks = new();
            //load file
            var replateTexts = new List<ReplaceTextDto>();
            replateTexts = GetDataContractFile(orderId, bondPolicy.TradingProviderId, true);

            byte[] byteArray = File.ReadAllBytes(fullPath);
            int pageCount = 1;
            MemoryStream memoryStreamFileThuong = new();
            await memoryStreamFileThuong.WriteAsync(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memoryStreamFileThuong, true))
            {
                #region xử lý file word không dấu
                wordDoc.ChangeDocumentType(WordprocessingDocumentType.Document);

                var mainPart = wordDoc.MainDocumentPart;
                string docText = null;

                using (StreamReader sr = new StreamReader(mainPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                //docText = mainPart.Document.Body.InnerText;
                docText = FindAndReplace(docText, replateTexts);
                using (StreamWriter sw = new StreamWriter(mainPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
                #region In ra table đối với hợp đồng phụ lục dòng tiền
                if (contractTemplate.ContractType == ContractTemplateType.BMHTNTTPVKQDT)
                {
                    var cashFlows = GetCashFlow(orderId, bondSecondary.TradingProviderId);
                    CreateTableCashFlow(cashFlows, mainPart);
                }
                #endregion
                try
                {
                    mainPart.Document.Save();
                }
                catch (XmlException)
                {
                    _logger.LogError("file word lỗi dạng XML");
                    throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                }
            }
            var fileNameNew = ContractDataUtils.GenerateNewFileName(fileName);
            string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
            byte[] byteArrayThuong = memoryStreamFileThuong.ToArray();
            tasks.Add(File.WriteAllBytesAsync(filePathNew, byteArrayThuong));
            var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
            string filePathPdf = Path.Combine(filePath, fileResult.Folder, fileNamePdf);
            tasks.Add(_sharedMediaApiUtils.ConvertWordToPdfAsync(byteArrayThuong, filePathPdf));
            #endregion
            result.FileName = fileNamePdf;
            result.FileSignatureUrl = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileNamePdf);
            result.FilePathToBeDeleted = filePathNew; //trả ra file word để xóa
            result.PageSign = pageCount;
            #region xử lý ảnh con dấu
            tasks.Add(Task.Run(async () =>
            {
                MemoryStream memoryStreamFileConDau = new();
                await memoryStreamFileConDau.WriteAsync(byteArrayThuong, 0, byteArrayThuong.Length);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memoryStreamFileConDau, true))
                {
                    wordDoc.ChangeDocumentType(WordprocessingDocumentType.Document);

                    var mainPart = wordDoc.MainDocumentPart;

                    #region Xử lý ảnh con dấu
                    FillStampImage(bondSecondary.TradingProviderId, mainPart, filePath);
                    #endregion
                    try
                    {
                        mainPart.Document.Save();
                    }
                    catch (XmlException)
                    {
                        _logger.LogError("file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
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
                var fileNameNewSign = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_DOCX);
                string filePathNewSign = Path.Combine(filePath, fileResult.Folder, fileNameNewSign);
                byte[] byteArrayConDau = memoryStreamFileConDau.ToArray();
                await File.WriteAllBytesAsync(filePathNewSign, byteArrayConDau);
                var fileNameSignPdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_PDF);
                string filePathSignPdf = Path.Combine(filePath, fileResult.Folder, fileNameSignPdf);
                await _sharedMediaApiUtils.ConvertWordToPdfAsync(byteArrayConDau, filePathSignPdf);
            }));
            #endregion 
            await Task.WhenAll(tasks);
            return result;
        }

        public SaveFileDto SaveContractSignPdf(int secondaryContractFileId, int contractTemplateId, int tradingProviderId)
        {
            var dto = GetSignPdfDto(tradingProviderId);
            var result = new SaveFileDto();
            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu của hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var secondaryContractFile = _bondSecondaryContractRepository.FindById(secondaryContractFileId, tradingProviderId: tradingProviderId);
            if (secondaryContractFile == null || secondaryContractFile.FileSignatureUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy bản ghi file hợp đồng {contractTemplate.Name}: {secondaryContractFileId}"), new FaultCode(((int)ErrorCode.BondOrderContractFileNotFound).ToString()), "");
            }
            string filePath = _fileConfig.Value.Path;
            var fileResult = FileUtils.GetPhysicalPath(secondaryContractFile.FileSignatureUrl, filePath);
            var fileSignPdf = fileResult.FullPath.Replace(".pdf", "-Sign.pdf");
            if (!File.Exists(fileSignPdf))
            {
                throw new FaultException(new FaultReason($"File đã ký của hợp đồng {contractTemplate.Name} không tồn tại: {fileSignPdf}"), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName.Replace(".pdf", "-Sign.pdf");
            var fullPath = fileSignPdf;
            byte[] pdfFileBytes = File.ReadAllBytes(fullPath);
            // Đọc file pdf vừa chuyển đổi để ký   
            dto.FilePdfByteArray = pdfFileBytes;
            dto.pageSign = secondaryContractFile.PageSign;
            //lưu chữ ký điện tử
            var fileSign = _sharedSignServerApiUtils.RequestSignPdf(dto);
            if (fileSign == null)
            {
                throw new FaultException(new FaultReason($"Ký điện tử không thành công. Vui lòng kiểm tra lại cấu hình!"), new FaultCode(((int)ErrorCode.CoreSignPdfFailed).ToString()), "");
            }
            var fileSignName = ContractDataUtils.GenerateNewFileName(fileName);
            //tạo đường dẫn lưu file chữ ký điện tử
            string filePathSign = Path.Combine(filePath, fileResult.Folder, fileSignName);
            File.WriteAllBytes(filePathSign, fileSign);
            result.FileName = fileSignName;
            result.FileSignatureUrl = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileSignName);
            result.FilePathToBeDeleted = fileResult.FullPath; //trả ra file file pdf để xóa
            return result;
        }

        private RequestSignPdfDto GetSignPdfDto(int tradingProviderId)
        {
            var tradingProvider = _tradingProviderRepository.FindById(tradingProviderId);
            if (tradingProvider == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy đại lý sơ cấp"), new FaultCode(((int)ErrorCode.TradingProviderNotFound).ToString()), "");
            }
            var businessCustomerTrading = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomerTrading == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin đại lý sơ cấp: {tradingProvider.BusinessCustomerId}"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
            }
            var xPoint = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "X_POINT")?.VarValue);
            var yPoint = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "Y_POINT")?.VarValue);
            var width = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "WIDTH")?.VarValue);
            var height = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "HEIGHT")?.VarValue);


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

        private void FillStampImage(int tradingProviderId, MainDocumentPart mainPart, string filePath)
        {
            var tradingProvider = _tradingProviderRepository.FindById(tradingProviderId);
            if (tradingProvider == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy đại lý sơ cấp"), new FaultCode(((int)ErrorCode.TradingProviderNotFound).ToString()), "");
            }

            if (tradingProvider.StampImageUrl != null)
            {
                var stampImage = FileUtils.GetPhysicalPath(tradingProvider.StampImageUrl, filePath);
                if (File.Exists(stampImage.FullPath))
                {
                    var images = File.ReadAllBytes(stampImage.FullPath);
                    Stream imageStream = new MemoryStream(images);
                    var picture = mainPart.Document.Descendants<DocumentFormat.OpenXml.Drawing.Pictures.Picture>().FirstOrDefault(p => p.NonVisualPictureProperties.NonVisualDrawingProperties.Name.Value.Contains("Picture"));
                    if (picture != null)
                    {
                        var blip = picture.BlipFill.Blip;
                        ImagePart newImagePath = mainPart.AddImagePart(ImagePartType.Png);
                        newImagePath.FeedData(imageStream);
                        blip.Embed = mainPart.GetIdOfPart(newImagePath);
                    }
                }
            }
        }

        /// <summary>
        /// tải file tạm app
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="policyDetailId"></param>
        /// <param name="BankAccId"></param>
        /// <param name="identificationId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExportContractApp(decimal totalValue, int policyDetailId, int BankAccId, int identificationId, int contractTemplateId, int? investorId = null)
        {
            string filePath = _fileConfig.Value.Path;
            var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(policyDetailId);
            if (bondPolicyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
            }

            var bondPolicy = _productBondSecondaryRepository.FindPolicyById(bondPolicyDetail.PolicyId, bondPolicyDetail.TradingProviderId);
            if (bondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(bondPolicy.SecondaryId, bondPolicy.TradingProviderId);
            if (bondSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }
            var result = new ExportResultDto();
            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, bondSecondary.TradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var contractCode = contractTemplate.Code;
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTempUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replateTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replateTexts = GetDataContractFileApp(totalValue, policyDetailId, BankAccId, identificationId, investorId,"");
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
                    //docText = mainPart.Document.Body.InnerText;
                    docText = FindAndReplace(docText, replateTexts);
                    using (StreamWriter sw = new StreamWriter(mainPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }

                    #region In ra table đối với hợp đồng phụ lục dòng tiền
                    if (contractTemplate.ContractType == ContractTemplateType.BMHTNTTPVKQDT)
                    {
                        var cashFlows = _bondSharedService.GetCashFlow(totalValue, policyDetailId);
                        CreateTableCashFlow(cashFlows, mainPart);
                    }
                    #endregion

                    try
                    {
                        mainPart.Document.Save();
                    }
                    catch (XmlException)
                    {
                        _logger.LogError("file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                result.fileData = ms.ToArray();
                var fileNameNew = ContractDataUtils.GenerateNewFileName(fileName);
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
                var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                string filePathPdf = Path.Combine(filePath, fileResult.Folder, fileNamePdf);
                result.fileDownloadName = fileNamePdf;
                result.filePath = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileNamePdf);
                //result.FilePathOld = fileNameNew;
                //convert sang file pdf
                await _sharedMediaApiUtils.ConvertWordToPdfAsync(filePathNew, filePathPdf);
                if (File.Exists(filePathPdf))
                {
                    //Trả về file pdf
                    result.fileData = File.ReadAllBytes(filePathPdf);
                    //xóa luôn file pdf
                    File.Delete(filePathPdf);
                }
                if (File.Exists(filePathNew))
                {
                    //xóa file word
                    File.Delete(filePathNew);
                }
            }
            return result;
        }


        #region WEB

        /// <summary>
        /// Fill data hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private List<ReplaceTextDto> GetDataContractFile(int orderId, int tradingProviderId, bool isSignature)
        {
            List<ReplaceTextDto> replateTexts = new List<ReplaceTextDto>(); 
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var createdDate = _bondSharedService.NextWorkDay(order.PaymentFullDate ?? DateTime.Now, tradingProviderId);
            HopDongSoVaNgayLap(replateTexts, order.ContractCode, createdDate);
            DateTime paymentFullDate = order.PaymentFullDate ?? DateTime.Now;
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TranDate}}",
                ReplaceText = paymentFullDate.ToString("dd/MM/yyyy")
            });
            ThongTinNhaDauTu(replateTexts, order, isSignature);
            var productPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById((int)order.PolicyDetailId, tradingProviderId);
            if (productPolicyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy kỳ hạn: {order.PolicyDetailId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var bondPolicy = _productBondSecondaryRepository.FindPolicyById(productPolicyDetail.PolicyId, productPolicyDetail.TradingProviderId);
            if (bondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(bondPolicy.SecondaryId, bondPolicy.TradingProviderId);
            if (bondSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }
            var bondInfo = _productBondInfoRepository.FindById((int)order.BondId);
            if (bondInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin trái phiếu: {order.BondId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var cashFlows = _bondSharedService.GetCashFlowContract((order.TotalValue), paymentFullDate, productPolicyDetail, bondPolicy, bondSecondary, bondInfo);
            DaiLySoCap(replateTexts, productPolicyDetail.TradingProviderId);

            DaiLyLuKy(replateTexts, bondInfo.DepositProviderId);

            TaiKhoanThuHuongDaiLySoCap(replateTexts, bondSecondary.BusinessCustomerBankAccId);

            LoaiSanPham(replateTexts, bondPolicy.Classify);

            DateTime? transferOwnershipDate = null;

            if (cashFlows.StartDate < paymentFullDate)
            {
                transferOwnershipDate = paymentFullDate;
            }
            else
            {
                transferOwnershipDate = cashFlows.StartDate;
            }

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TransferOwnershipDate}}",
                ReplaceText = transferOwnershipDate?.ToString("dd/MM/yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestBond}}",
                ReplaceText = cashFlows.TotalValue.ToString("N0").Replace(",", ".")
            });

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestBondText}}",
                ReplaceText = NumberToText.ConvertNumberToText((double)cashFlows.TotalValue)
            });
            var issuer = _issuerRepository.FindById((int)bondInfo.IssuerId);
            if (issuer == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy tổ chức phát hành: {bondInfo.IssuerId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
            if (businessCustomerIssuer == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin tổ chức phát hành: {issuer.BusinessCustomerId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            replateTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto
                {
                    FindText = "{{BondCode}}",
                    ReplaceText = bondInfo.BondCode
                },
                new ReplaceTextDto
                {
                    FindText = "{{InterestPeriod}}",
                    ReplaceText = ContractDataUtils.GetInterestPeriodTypeName(productPolicyDetail.InterestType, productPolicyDetail.InterestPeriodQuantity, productPolicyDetail.InterestPeriodType)
                },
                new ReplaceTextDto
                {
                    FindText = "{{Interest}}",
                    ReplaceText = cashFlows.InterestRate.ToString("#.#").Replace(".", ",")
                },
                new ReplaceTextDto
                {
                    FindText = "{{IssuerName}}",
                    ReplaceText = businessCustomerIssuer.Name
                },
                new ReplaceTextDto
                {
                    FindText = "{{IssuerCode}}",
                    ReplaceText = businessCustomerIssuer.Code
                },
                new ReplaceTextDto
                {
                    FindText = "{{IssuerAddress}}",
                    ReplaceText = businessCustomerIssuer.Address
                },
                new ReplaceTextDto
                {
                    FindText = "{{DayIssueDate}}",
                    ReplaceText = bondInfo.IssueDate?.ToString("dd")
                },
                new ReplaceTextDto
                {
                    FindText = "{{MonthIssueDate}}",
                    ReplaceText = bondInfo.IssueDate?.ToString("MM")
                },
                new ReplaceTextDto
                {
                    FindText = "{{YearIssueDate}}",
                    ReplaceText = bondInfo.IssueDate?.ToString("yyyy")
                },
                new ReplaceTextDto
                {
                    FindText = "{{DayDueDate}}",
                    ReplaceText = bondInfo.DueDate?.ToString("dd")
                },
                new ReplaceTextDto
                {
                    FindText = "{{MonthDueDate}}",
                    ReplaceText = bondInfo.DueDate?.ToString("MM")
                },
                new ReplaceTextDto
                {
                    FindText = "{{YearDueDate}}",
                    ReplaceText = bondInfo.DueDate?.ToString("yyyy")
                },
                new ReplaceTextDto
                {
                    FindText = "{{ParValue}}",
                    ReplaceText = bondInfo.ParValue.ToString("N0").Replace(",", ".")
                },

            });

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{ParValueText}}",
                ReplaceText = NumberToText.ConvertNumberToText((double)bondInfo.ParValue)
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{BondQuantity}}",
                ReplaceText = cashFlows.Quantity.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{BondPrice}}",
                ReplaceText = cashFlows.UnitPrice.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{BuyDate}}",
                ReplaceText = order.BuyDate?.ToString("dd/MM/yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{Tenor}}",
                ReplaceText = $"{productPolicyDetail.PeriodQuantity} {ContractDataUtils.GetNameDateType(productPolicyDetail.PeriodType)}"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{StartDate}}",
                ReplaceText = cashFlows.StartDate?.ToString("dd/MM/yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{EndDate}}",
                ReplaceText = cashFlows.EndDate?.ToString("dd/MM/yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{DayContractEnd}}",
                ReplaceText = cashFlows.EndDate?.ToString("dd")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{MonthContractEnd}}",
                ReplaceText = cashFlows.EndDate?.ToString("MM")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{YearContractEnd}}",
                ReplaceText = cashFlows.EndDate?.ToString("yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{SumTongNhan}}",
                ReplaceText = cashFlows.ActuallyProfit.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{FinalIncome}}",
                ReplaceText = cashFlows.ActuallyProfit.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestDate}}",
                ReplaceText = cashFlows.NumberOfDays.ToString()
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{Coupon}}",
                ReplaceText = cashFlows.Coupon.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{ActuallyProfit}}",
                ReplaceText = cashFlows.ActuallyProfit.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TotalPrice}}",
                ReplaceText = cashFlows.ActuallyProfit.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{FinalPeriod}}",
                ReplaceText = cashFlows.FinalPeriod.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TotalReceiveValue}}",
                ReplaceText = cashFlows.TotalReceiveValue.ToString("N0").Replace(",", ".")
            });
            return replateTexts;
        }

        #endregion

        private CashFlowDto GetCashFlow(int orderId, int tradingProviderId)
        {
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            DateTime paymentFullDate = order.PaymentFullDate ?? DateTime.Now;
            var productPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById((int)order.PolicyDetailId, tradingProviderId);
            if (productPolicyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy kỳ hạn: {order.PolicyDetailId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var bondPolicy = _productBondSecondaryRepository.FindPolicyById(productPolicyDetail.PolicyId, productPolicyDetail.TradingProviderId);
            if (bondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(bondPolicy.SecondaryId, bondPolicy.TradingProviderId);
            if (bondSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }
            var bondInfo = _productBondInfoRepository.FindById((int)order.BondId);
            if (bondInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin trái phiếu: {order.BondId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            return _bondSharedService.GetCashFlowContract(order.TotalValue, paymentFullDate, productPolicyDetail, bondPolicy, bondSecondary, bondInfo);

        }


        #region APP

        /// <summary>
        /// Lấy data hợp đồng tạm trên mobile
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="policyDetailId"></param>
        /// <param name="bankAccId"></param>
        /// <param name="identificationId"></param>
        /// <param name="contractCode"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataContractFileApp(decimal totalValue, int policyDetailId, int bankAccId, int identificationId, int? investorId,string contractCode)
        {
            var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(policyDetailId);
            if (bondPolicyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
            }

            var bondPolicy = _productBondSecondaryRepository.FindPolicyById(bondPolicyDetail.PolicyId, bondPolicyDetail.TradingProviderId);
            if (bondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(bondPolicy.SecondaryId, bondPolicy.TradingProviderId);
            if (bondSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }
            //lấy thông tin lô
            var bondInfo = _productBondInfoRepository.FindById(bondSecondary.BondId);
            if (bondInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
            }
            var cashFlows = _bondSharedService.GetCashFlowContract(totalValue, DateTime.Now, bondPolicyDetail, bondPolicy, bondSecondary, bondInfo);
            List<ReplaceTextDto> replateTexts = new List<ReplaceTextDto>();
            var createdDate = DateTime.Now;
            HopDongSoVaNgayLap(replateTexts, contractCode, createdDate);
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TranDate}}",
                ReplaceText = createdDate.ToString("dd/MM/yyyy")
            });
            DaiLySoCap(replateTexts, bondPolicyDetail.TradingProviderId);

            DaiLyLuKy(replateTexts, bondInfo.DepositProviderId);

            TaiKhoanThuHuongDaiLySoCap(replateTexts, bondSecondary.BusinessCustomerBankAccId);

            LoaiSanPham(replateTexts, bondPolicy.Classify);
            if(investorId == null)
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            ThongTinNhaDauTuCaNhan(replateTexts, investorId ?? 0, identificationId, bankAccId);

            DateTime? transferOwnershipDate = null;

            if (cashFlows.StartDate < DateTime.Now)
            {
                transferOwnershipDate = DateTime.Now;
            }
            else
            {
                transferOwnershipDate = cashFlows.StartDate;
            }
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TransferOwnershipDate}}",
                ReplaceText = transferOwnershipDate?.ToString("dd/MM/yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestBond}}",
                ReplaceText = totalValue.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestBondText}}",
                ReplaceText = NumberToText.ConvertNumberToText((double)totalValue)
            });

            var issuer = _issuerRepository.FindById((int)bondInfo.IssuerId);
            if (issuer == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy tổ chức phát hành: {bondInfo.IssuerId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
            if (businessCustomerIssuer == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin tổ chức phát hành: {issuer.BusinessCustomerId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            replateTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto
                {
                    FindText = "{{InterestPeriod}}",
                    ReplaceText = ContractDataUtils.GetInterestPeriodTypeName(bondPolicyDetail.InterestType, bondPolicyDetail.InterestPeriodQuantity, bondPolicyDetail.InterestPeriodType)
                },
                new ReplaceTextDto
                {
                    FindText = "{{BondCode}}",
                    ReplaceText = bondInfo.BondCode
                },
                new ReplaceTextDto
                {
                    FindText = "{{Interest}}",
                    ReplaceText = cashFlows.InterestRate.ToString("#.#").Replace(".", ",")
                },
                new ReplaceTextDto
                {
                    FindText = "{{IssuerName}}",
                    ReplaceText = businessCustomerIssuer.Name
                },
                new ReplaceTextDto
                {
                    FindText = "{{IssuerCode}}",
                    ReplaceText = businessCustomerIssuer.Code
                },
                new ReplaceTextDto
                {
                    FindText = "{{IssuerAddress}}",
                    ReplaceText = businessCustomerIssuer.Address
                },
                new ReplaceTextDto
                {
                    FindText = "{{ParValue}}",
                    ReplaceText = bondInfo.ParValue.ToString("N0").Replace(",", ".")
                },
                new ReplaceTextDto
                {
                    FindText = "{{DayIssueDate}}",
                    ReplaceText = bondInfo.IssueDate?.ToString("dd")
                },
                new ReplaceTextDto
                {
                    FindText = "{{MonthIssueDate}}",
                    ReplaceText = bondInfo.IssueDate?.ToString("MM")
                },
                new ReplaceTextDto
                {
                    FindText = "{{YearIssueDate}}",
                    ReplaceText = bondInfo.IssueDate?.ToString("yyyy")
                },
                new ReplaceTextDto
                {
                    FindText = "{{DayDueDate}}",
                    ReplaceText = bondInfo.DueDate?.ToString("dd")
                },
                new ReplaceTextDto
                {
                    FindText = "{{MonthDueDate}}",
                    ReplaceText = bondInfo.DueDate?.ToString("MM")
                },
                new ReplaceTextDto
                {
                    FindText = "{{YearDueDate}}",
                    ReplaceText = bondInfo.DueDate?.ToString("yyyy")
                },
            });

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{ParValueText}}",
                ReplaceText = NumberToText.ConvertNumberToText((double)bondInfo.ParValue)
            });

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{BondQuantity}}",
                ReplaceText = cashFlows.Quantity.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{BondPrice}}",
                ReplaceText = cashFlows.UnitPrice.ToString("N0").Replace(",", ".")
            });


            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{Tenor}}",
                ReplaceText = $"{bondPolicyDetail.PeriodQuantity} {ContractDataUtils.GetNameDateType(bondPolicyDetail.PeriodType)}"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{BuyDate}}",
                ReplaceText = DateTime.Now.ToString("dd/MM/yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{StartDate}}",
                ReplaceText = cashFlows.StartDate?.ToString("dd/MM/yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{EndDate}}",
                ReplaceText = cashFlows.EndDate?.ToString("dd/MM/yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{DayContractEnd}}",
                ReplaceText = cashFlows.EndDate?.ToString("dd")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{MonthContractEnd}}",
                ReplaceText = cashFlows.EndDate?.ToString("MM")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{YearContractEnd}}",
                ReplaceText = cashFlows.EndDate?.ToString("yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{SumTongNhan}}",
                ReplaceText = cashFlows.ActuallyProfit.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{FinalIncome}}",
                ReplaceText = cashFlows.ActuallyProfit.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestDate}}",
                ReplaceText = cashFlows.NumberOfDays.ToString()
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{Coupon}}",
                ReplaceText = cashFlows.Coupon.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{ActuallyProfit}}",
                ReplaceText = cashFlows.ActuallyProfit.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TotalPrice}}",
                ReplaceText = cashFlows.ActuallyProfit.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{FinalPeriod}}",
                ReplaceText = cashFlows.FinalPeriod.ToString("N0").Replace(",", ".")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TotalReceiveValue}}",
                ReplaceText = cashFlows.TotalReceiveValue.ToString("N0").Replace(",", ".")
            });
            replateTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto
                {
                    FindText = "{{Signature}}",
                    ReplaceText = ""
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerNameSignature}}",
                    ReplaceText = ""
                }
            });
            return replateTexts;
        }
        #endregion

        public ExportResultDto ExportFileScanContract(int orderId, int contractTemplateId, int secondaryContractFileId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var secondaryContractFile = _bondSecondaryContractRepository.FindById(secondaryContractFileId, tradingProviderId: tradingProviderId);
            if (secondaryContractFile == null || secondaryContractFile?.FileScanUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy file scan hợp đồng: {secondaryContractFileId}"), new FaultCode(((int)ErrorCode.BondOrderContractFileNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(secondaryContractFile.FileScanUrl);
            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                throw new FaultException(new FaultReason($"File file scan hợp đồng: {secondaryContractFileId} không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }

        public ExportResultDto ExportContractTemp(int orderId, int contractTemplateId, int secondaryContractFileId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var secondaryContractFile = _bondSecondaryContractRepository.FindById(secondaryContractFileId, tradingProviderId: tradingProviderId);
            if (secondaryContractFile == null || secondaryContractFile?.FileTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy file tạm hợp đồng id: {secondaryContractFileId}"), new FaultCode(((int)ErrorCode.BondOrderContractFileNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(secondaryContractFile.FileTempUrl);
            var folder = path["folder"];
            var fileName = path["file"];
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                throw new FaultException(new FaultReason($"File tạm hợp đồng id: {secondaryContractFileId} không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            result.fileDownloadName = fileName;
            return result;
        }

        public ExportResultDto ExportContractSignature(int orderId, int contractTemplateId, int secondaryContractFileId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var secondaryContractFile = _bondSecondaryContractRepository.FindById(secondaryContractFileId, tradingProviderId: tradingProviderId);
            if (secondaryContractFile == null || secondaryContractFile?.FileSignatureUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy file đã ký hợp đồng id: {secondaryContractFileId}"), new FaultCode(((int)ErrorCode.BondOrderContractFileNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(secondaryContractFile.FileSignatureUrl);
            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                throw new FaultException(new FaultReason($"File đã ký hợp đồng id: {secondaryContractFileId} không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }

        public ExportResultDto ExportContractSignatureApp(int secondaryContractFileId)
        {
            var result = new ExportResultDto();
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var secondaryContractFile = _bondSecondaryContractRepository.FindById(secondaryContractFileId, investorId);
            if (secondaryContractFile == null || secondaryContractFile?.FileSignatureUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy file đã ký hợp đồng: {secondaryContractFileId}"), new FaultCode(((int)ErrorCode.BondOrderContractFileNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(secondaryContractFile.FileSignatureUrl);
            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                throw new FaultException(new FaultReason($"File đã ký hợp đồng id: {secondaryContractFileId} không tồn tại."), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }

        private TableProperties GetBorder()
        {
            TableProperties tblProperties = new TableProperties();

            //// Create Table Borders

            TableBorders tblBorders = new TableBorders();



            TopBorder topBorder = new TopBorder();

            topBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            topBorder.Color = "CC0000";

            tblBorders.AppendChild(topBorder);



            BottomBorder bottomBorder = new BottomBorder();

            bottomBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            bottomBorder.Color = "000000";

            tblBorders.AppendChild(bottomBorder);



            RightBorder rightBorder = new RightBorder();

            rightBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            rightBorder.Color = "000000";

            tblBorders.AppendChild(rightBorder);



            LeftBorder leftBorder = new LeftBorder();

            leftBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            leftBorder.Color = "000000";

            tblBorders.AppendChild(leftBorder);



            InsideHorizontalBorder insideHBorder = new InsideHorizontalBorder();

            insideHBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            insideHBorder.Color = "000000";

            tblBorders.AppendChild(insideHBorder);



            InsideVerticalBorder insideVBorder = new InsideVerticalBorder();

            insideVBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            insideVBorder.Color = "000000";

            tblBorders.AppendChild(insideVBorder);



            //// Add the table borders to the properties

            tblProperties.AppendChild(tblBorders);

            return tblProperties;
        }

        private void CreateTableCashFlow(CashFlowDto cashFlows, MainDocumentPart mainPart)
        {
            Body bod = mainPart.Document.Body;
            var table = bod.Descendants<Table>().FirstOrDefault(tbl => tbl.InnerText.Contains("Số tiền tạm ứng (VND)"));

            if (table != null)
            {
                //// Create the table properties
                var tblProperties = GetBorder();

                //// Add the table properties to the table
                table.AppendChild(tblProperties);
                //table.Append(new TableRow(new TableCell(new Paragraph(new Run(new Text("test"))))));
                decimal sumValue = 0;
                foreach (var cashFlow in cashFlows.CashFlow)
                {
                    TableRow row = new TableRow();
                    List<TableCell> cell = new List<TableCell>()
                    {
                        new TableCell(new Paragraph(new Run(new Text($"{cashFlow.ReceiveDate?.ToString("dd/MM/yyyy")}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center },
                                
                            }
                        }),
                        new TableCell(new Paragraph(new Run(new Text($"{cashFlow.ReceiveValue.ToString("N0").Replace(",", ".")}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
                            }
                        })
                    };
                    sumValue += cashFlow.ReceiveValue;
                    row.Append(cell);
                    table.Append(row);
                }
                TableRow rowFooter = new TableRow();
                List<TableCell> cellFooter = new List<TableCell>()
                {
                    new TableCell(new Paragraph(new Run(new Text($"Tổng")))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                    new TableCell(new Paragraph(new Run(new Text($"{sumValue.ToString("N0").Replace(",", ".")}")))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    })
                };
                rowFooter.Append(cellFooter);
                table.Append(rowFooter);

            }

            var tableCouponInfos = bod.Descendants<Table>().FirstOrDefault(tbl => tbl.InnerText.Contains("Số tiền thực nhận (VND)"));

            if (tableCouponInfos != null)
            {
                //// Create the table properties
                var tblProperties = GetBorder();
                //// Add the table properties to the table
                tableCouponInfos.AppendChild(tblProperties);
                //table.Append(new TableRow(new TableCell(new Paragraph(new Run(new Text("test"))))));
                decimal sumValue = 0;
                foreach (var couponInfo in cashFlows.CouponInfos)
                {
                    TableRow row = new TableRow();
                    List<TableCell> cell = new List<TableCell>()
                    {
                        new TableCell(new Paragraph(new Run(new Text($"{couponInfo.PayDate.ToString("dd/MM/yyyy")}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center },
                              
                            }
                        }),
                        new TableCell(new Paragraph(new Run(new Text($"{couponInfo.Coupon.ToString("N0").Replace(",", ".")}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
                            }
                        })
                    };
                    sumValue += couponInfo.Coupon;
                    row.Append(cell);
                    tableCouponInfos.Append(row);
                }
                TableRow rowFooter = new TableRow();
                List<TableCell> cellFooter = new List<TableCell>()
                {
                    new TableCell(new Paragraph(new Run(new Text($"Tổng")))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                    new TableCell(new Paragraph(new Run(new Text($"{sumValue.ToString("N0").Replace(",", ".")}")))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    })
                };
                rowFooter.Append(cellFooter);
                tableCouponInfos.Append(rowFooter);
            }
        }


        #region Receive contract file
        /// <summary>
        /// Lấy data hợp đồng giao nhan
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataReceiveContractFile(BondOrder order, int? tradingProviderId = null)
        {
            List<ReplaceTextDto> replateTexts = new List<ReplaceTextDto>();
            var tradingProviderIds = tradingProviderId ?? CommonUtils.GetCurrentTradingProviderId(_httpContext);
            DaiLySoCap(replateTexts, tradingProviderIds);
            ThongTinNhaDauTu(replateTexts, order, false);
            replateTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto
                    {
                         FindText = "{{ContractCode}}",
                         ReplaceText = order.ContractCode
                    },
                });
            return replateTexts;
        }

        public async Task<ExportResultDto> ExportContractReceive(int orderId, int bondSecondaryId, int tradingProviderId, int source)
        {
            string filePath = _fileConfig.Value.Path;
            var result = new ExportResultDto();
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.BondOrderNotFound).ToString()), "");
            }

            var contractTemplate = _receiveContractTemplateRepository.FindAll(bondSecondaryId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.FileUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng giao nhận của bán theo kỳ hạn id: bondSecondaryId"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.FileUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng giao nhận không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replateTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replateTexts = GetDataReceiveContractFile(order, tradingProviderId);
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
                    //docText = mainPart.Document.Body.InnerText;
                    docText = FindAndReplace(docText, replateTexts);
                    using (StreamWriter sw = new StreamWriter(mainPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }
                    string text; 
                    string urlConfirmReceiveContract = _urlConfirmReceiveContract.Value.Url;
                    text = $"{urlConfirmReceiveContract}/{order.DeliveryCode}";
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
                    try
                    {
                        mainPart.Document.Save();
                    }
                    catch (XmlException)
                    {
                        _logger.LogError("file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng giao nhận lỗi. Vui lòng upload lại mẫu hợp đồng giao nhận khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                result.fileData = ms.ToArray();
                var fileNameNew = ContractDataUtils.GenerateNewFileName(fileName);
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
                var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                string filePathPdf = Path.Combine(filePath, fileResult.Folder, fileNamePdf);
                result.fileDownloadName = fileNamePdf;
                result.filePath = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileNamePdf);
                //result.FilePathOld = fileNameNew;
                //convert sang file pdf
                await _sharedMediaApiUtils.ConvertWordToPdfAsync(filePathNew, filePathPdf);
                if (File.Exists(filePathPdf))
                {
                    //Trả về file pdf
                    result.fileData = File.ReadAllBytes(filePathPdf);
                    //xóa luôn file pdf
                    File.Delete(filePathPdf);
                }
                if (File.Exists(filePathNew))
                {
                    //xóa file word
                    File.Delete(filePathNew);
                }
            }
            return result;
        }

        #endregion

        #region Test fill data file mẫu

        private List<ReplaceTextDto> GetDataContractFileTest()
        {
            List<ReplaceTextDto> replateTexts = new List<ReplaceTextDto>();
            var createdDate = DateTime.Now;
            replateTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto
                {
                    FindText = "{{ContractCode}}",
                    ReplaceText = "EB000000"
                },
                new ReplaceTextDto
                {
                    FindText = "{{TranContent}}",
                    ReplaceText = "EB000000"
                },
                new ReplaceTextDto
                {
                    FindText = "{{DayContract}}",
                    ReplaceText = createdDate.ToString("dd")
                },
                new ReplaceTextDto
                {
                    FindText = "{{MonthContract}}",
                    ReplaceText = createdDate.ToString("MM")
                },
                new ReplaceTextDto
                {
                    FindText = "{{YearContract}}",
                    ReplaceText = createdDate.ToString("yyyy")
                },
                 new ReplaceTextDto
                {
                    FindText = "{{ContractDate}}",
                    ReplaceText = createdDate.ToString("dd/MM/yyyy")
                }
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TranDate}}",
                ReplaceText = createdDate.ToString("dd/MM/yyyy")
            });

            replateTexts.AddRange(new List<ReplaceTextDto>()
                {
                     new ReplaceTextDto
                    {
                        FindText = "{{InterestPeriod}}",
                        ReplaceText = "Cuối kỳ"
                    },
                    new ReplaceTextDto
                    {
                        FindText = "{{Signature}}",
                        ReplaceText = $"Đã ký {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}"
                    },
                    new ReplaceTextDto
                    {
                        FindText = "{{CustomerNameSignature}}",
                        ReplaceText = "Nguyễn Văn A"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerName}}",
                         ReplaceText = "Nguyễn Văn A"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerRepPosition}}",
                         ReplaceText = "Nguyễn Văn B"
                    }, new ReplaceTextDto
                    {
                         FindText = "{{CustomerAddress}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerRepAddress}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerTradingAddress}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{BusinessCustomerName}}",
                         ReplaceText = "Công ty TNHH ABC"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdNo}}",
                         ReplaceText = "0123456789"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdType}}",
                         ReplaceText = "CCCD"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdDate}}",
                         ReplaceText = "01/01/2022"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdIssuer}}",
                         ReplaceText = "CA Thành Phố Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdExpiredDate}}",
                         ReplaceText = "01/01/2022"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerPhone}}",
                         ReplaceText = "03624121786"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerSex}}",
                         ReplaceText = "Nam"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBirthDate}}",
                         ReplaceText = "01/01/1999"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerResidentAddress}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerEmail}}",
                         ReplaceText = "example@gmail.com"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerTaxCode}}",
                         ReplaceText = "12345676"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankAccNo}}",
                         ReplaceText = "123456789123456"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankAccName}}",
                         ReplaceText = "Nguyen Van A"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankName}}",
                         ReplaceText = "BankName"
                    },
                     new ReplaceTextDto
                    {
                         FindText = "{{CustomerFullBankName}}",
                         ReplaceText = "Ngân hàng TMCP Xuất Nhập khẩu Việt Nam"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankBranch}}",
                         ReplaceText = "Chi nhánh bank"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerContactAddress}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerNationality}}",
                         ReplaceText = "Việt Nam"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerDecisionNo}}",
                         ReplaceText = "A123456"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerDecisionDate}}",
                         ReplaceText = "12/02/2022"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerLicenseIssuer}}",
                         ReplaceText = "TP Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerLicenseDate}}",
                         ReplaceText = "12/02/2022"
                    },
                });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderName}}",
                ReplaceText = "Công ty cổ phần đầu tư ABC"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderNameUpperCase}}",
                ReplaceText = "CÔNG TY CỔ PHẦN ĐẦU TƯ ABC"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderShortName}}",
                ReplaceText = "DinhThienTu"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderTaxCode}}",
                ReplaceText = "2423535745"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderPhone}}",
                ReplaceText = "0123456789"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderFax}}",
                ReplaceText = "387492382423"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderLicenseIssuer}}",
                ReplaceText = "TP Hà Nội"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderLicenseDate}}",
                ReplaceText = "23/08/2015"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderAddress}}",
                ReplaceText = "Số 123, Trung Kính"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderRepName}}",
                ReplaceText = "Nguyễn Văn C"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderRepPosition}}",
                ReplaceText = "Giám đốc"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderDecisionNo}}",
                ReplaceText = "73627563223"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankAccName}}",
                ReplaceText = "Nguyen Van C"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankAccNo}}",
                ReplaceText = "37264236756"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankName}}",
                ReplaceText = "BankName"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{PolicyClassifyName}}",
                ReplaceText = "PNOTE"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{DepositName}}",
                ReplaceText = "Công ty ABC"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{DepositNameUpperCase}}",
                ReplaceText = "CÔNG TY ABC"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{DepositShortName}}",
                ReplaceText = "ABC"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{Interest}}",
                ReplaceText = "10%"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{StartDate}}",
                ReplaceText = "31/08/2022"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{EndDate}}",
                ReplaceText = "31/08/2024"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestMoney}}",
                ReplaceText = "1.000.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TaxProfit}}",
                ReplaceText = "10.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TotalReceiveValue}}",
                ReplaceText = "1.000.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{ActuallyProfit}}",
                ReplaceText = "1.000.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TotalReceiveValue}}",
                ReplaceText = "1.100.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestMoneyText}}",
                ReplaceText = "một tỷ"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{Tenor}}",
                ReplaceText = "24 Tháng"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TransferOwnershipDate}}",
                ReplaceText = "16/10/2022"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestBond}}",
                ReplaceText = "1.000.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestBondText}}",
                ReplaceText = "Một tỷ"
            });
            replateTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto
                {
                    FindText = "{{InterestPeriod}}",
                    ReplaceText = "Cuối kỳ"
                },
                new ReplaceTextDto
                {
                    FindText = "{{BondCode}}",
                    ReplaceText = "EB123456"
                },
                new ReplaceTextDto
                {
                    FindText = "{{Interest}}",
                    ReplaceText = "10"
                },
                new ReplaceTextDto
                {
                    FindText = "{{IssuerName}}",
                    ReplaceText = "Công ty DEH"
                },
                new ReplaceTextDto
                {
                    FindText = "{{IssuerCode}}",
                    ReplaceText = "DEH"
                },
                new ReplaceTextDto
                {
                    FindText = "{{IssuerAddress}}",
                    ReplaceText = "TP Hà Nội"
                },
                new ReplaceTextDto
                {
                    FindText = "{{ParValue}}",
                    ReplaceText = "1.000.000.000"
                },
                new ReplaceTextDto
                {
                    FindText = "{{DayIssueDate}}",
                    ReplaceText = "26"
                },
                new ReplaceTextDto
                {
                    FindText = "{{MonthIssueDate}}",
                    ReplaceText = "04"
                },
                new ReplaceTextDto
                {
                    FindText = "{{YearIssueDate}}",
                    ReplaceText = "2018"
                },
                new ReplaceTextDto
                {
                    FindText = "{{DayDueDate}}",
                    ReplaceText = "26"
                },
                new ReplaceTextDto
                {
                    FindText = "{{MonthDueDate}}",
                    ReplaceText = "04"
                },
                new ReplaceTextDto
                {
                    FindText = "{{YearDueDate}}",
                    ReplaceText = "2020"
                },
            });

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{ParValueText}}",
                ReplaceText = "Một tỷ"
            });

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{BondQuantity}}",
                ReplaceText = "20.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{BondPrice}}",
                ReplaceText = "200.000"
            });


            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{Tenor}}",
                ReplaceText = $"24 tháng"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{BuyDate}}",
                ReplaceText = "12/10/2018"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{StartDate}}",
                ReplaceText = "26/04/2018"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{EndDate}}",
                ReplaceText = "26/04/2020"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{DayContractEnd}}",
                ReplaceText = "26"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{MonthContractEnd}}",
                ReplaceText = "04"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{YearContractEnd}}",
                ReplaceText = "2020"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{SumTongNhan}}",
                ReplaceText = "1.000.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{FinalIncome}}",
                ReplaceText = "1.000.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{InvestDate}}",
                ReplaceText = "26/08/2018"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{Coupon}}",
                ReplaceText = "1.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{ActuallyProfit}}",
                ReplaceText = "1.000.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TotalPrice}}",
                ReplaceText = "1.000.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{FinalPeriod}}",
                ReplaceText = "1.000.000.000"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TotalReceiveValue}}",
                ReplaceText = "1.000.000.000"
            });
            return replateTexts;
        }

        public async Task<ExportResultDto> ExportContractPdfTest(int tradingProviderId, int contractTemplateId)
        {
            string filePath = _fileConfig.Value.Path;

            var result = new ExportResultDto();
            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTempUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replateTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replateTexts = GetDataContractFileTest();
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
                    //docText = mainPart.Document.Body.InnerText;
                    docText = FindAndReplace(docText, replateTexts);
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
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                result.fileData = ms.ToArray();
                var fileNameNew = ContractDataUtils.GenerateNewFileName(fileName);
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
                var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                string filePathPdf = Path.Combine(filePath, fileResult.Folder, fileNamePdf);
                result.fileDownloadName = fileNamePdf;
                result.filePath = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileNamePdf);
                //result.FilePathOld = fileNameNew;
                //convert sang file pdf
                await _sharedMediaApiUtils.ConvertWordToPdfAsync(filePathNew, filePathPdf);
                if (File.Exists(filePathPdf))
                {
                    //Trả về file pdf
                    result.fileData = File.ReadAllBytes(filePathPdf);
                    //xóa luôn file pdf
                    File.Delete(filePathPdf);
                }
                if (File.Exists(filePathNew))
                {
                    //xóa file word
                    File.Delete(filePathNew);
                }
            }
            return result;
        }
        public ExportResultDto ExportContractWordTest(int tradingProviderId, int contractTemplateId)
        {
            string filePath = _fileConfig.Value.Path;

            var result = new ExportResultDto();
            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTempUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replateTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replateTexts = GetDataContractFileTest();
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
                    //docText = mainPart.Document.Body.InnerText;
                    docText = FindAndReplace(docText, replateTexts);
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
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                result.fileData = ms.ToArray();
                var fileNameNew = ContractDataUtils.GenerateNewFileName(fileName);
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
                result.fileDownloadName = fileNameNew;
                result.filePath = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileNameNew);
                if (File.Exists(filePathNew))
                {
                    //xóa file word
                    File.Delete(filePathNew);
                }
            }
            return result;
        }
        #endregion

        #region Test fill data file hợp đồng giao nhận

        private List<ReplaceTextDto> GetDataContractFileReceive()
        {
            List<ReplaceTextDto> replateTexts = new List<ReplaceTextDto>();
            var createdDate = DateTime.Now;
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderName}}",
                ReplaceText = "Công ty cổ phần ABC"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderShortName}}",
                ReplaceText = "ABC"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderTaxCode}}",
                ReplaceText = "123456"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderPhone}}",
                ReplaceText = "0364126734"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderFax}}",
                ReplaceText = "fef2144124"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderLicenseIssuer}}",
                ReplaceText = "TP Hà Nội"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderLicenseDate}}",
                ReplaceText = "26/03/2018"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderAddress}}",
                ReplaceText = "Số 172, Quận Hai Bà Trưng"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderRepName}}",
                ReplaceText = "Mai Thành Tỷ Phú"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderRepPosition}}",
                ReplaceText = "Chủ tịch"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderDecisionNo}}",
                ReplaceText = "786243612"
            });
            replateTexts.AddRange(new List<ReplaceTextDto>()
                {
                     new ReplaceTextDto
                    {
                        FindText = "{{InterestPeriod}}",
                        ReplaceText = "Cuối kỳ"
                    },
                    new ReplaceTextDto
                    {
                        FindText = "{{Signature}}",
                        ReplaceText = $"Đã ký {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}"
                    },
                    new ReplaceTextDto
                    {
                        FindText = "{{CustomerNameSignature}}",
                        ReplaceText = "Nguyễn Văn A"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerName}}",
                         ReplaceText = "Nguyễn Văn A"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerRepPosition}}",
                         ReplaceText = "Nguyễn Văn B"
                    }, new ReplaceTextDto
                    {
                         FindText = "{{CustomerAddress}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerRepAddress}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerTradingAddress}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{BusinessCustomerName}}",
                         ReplaceText = "Công ty TNHH ABC"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdNo}}",
                         ReplaceText = "0123456789123"
                    }, 
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdType}}",
                         ReplaceText = "CCCD"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdDate}}",
                         ReplaceText = "13/09/2017"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdIssuer}}",
                         ReplaceText = "CA Thành Phố Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdExpiredDate}}",
                         ReplaceText = "13/09/2022"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerPhone}}",
                         ReplaceText = "03624121786"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerSex}}",
                         ReplaceText = "Nam"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBirthDate}}",
                         ReplaceText = "29/07/1999"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerResidentAddress}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerEmail}}",
                         ReplaceText = "example@gmail.com"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerTaxCode}}",
                         ReplaceText = "12345676"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankAccNo}}",
                         ReplaceText = "23219217132141"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankAccName}}",
                         ReplaceText = "Nguyen Van A"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankName}}",
                         ReplaceText = "BankName"
                    },
                     new ReplaceTextDto
                    {
                         FindText = "{{CustomerFullBankName}}",
                         ReplaceText = "Ngân hàng TMCP Xuất Nhập khẩu Việt Nam"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankBranch}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerContactAddress}}",
                         ReplaceText = "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerNationality}}",
                         ReplaceText = "Việt Nam"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerDecisionNo}}",
                         ReplaceText = "A123456"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerDecisionDate}}",
                         ReplaceText = "12/02/2022"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerLicenseIssuer}}",
                         ReplaceText = "TP Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerLicenseDate}}",
                         ReplaceText = "12/02/2022"
                    },
                });
            return replateTexts;
        }

        public async Task<ExportResultDto> ExportContractReceivePdfTest(int tradingProviderId, int contractTemplateId)
        {
            string filePath = _fileConfig.Value.Path;

            var result = new ExportResultDto();
            var contractTemplate = _receiveContractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.FileUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.FileUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replateTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replateTexts = GetDataContractFileReceive();
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
                    //docText = mainPart.Document.Body.InnerText;
                    docText = FindAndReplace(docText, replateTexts);
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
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                result.fileData = ms.ToArray();
                var fileNameNew = ContractDataUtils.GenerateNewFileName(fileName);
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
                var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                string filePathPdf = Path.Combine(filePath, fileResult.Folder, fileNamePdf);
                result.fileDownloadName = fileNamePdf;
                result.filePath = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileNamePdf);
                //result.FilePathOld = fileNameNew;
                //convert sang file pdf
                await _sharedMediaApiUtils.ConvertWordToPdfAsync(filePathNew, filePathPdf);
                if (File.Exists(filePathPdf))
                {
                    //Trả về file pdf
                    result.fileData = File.ReadAllBytes(filePathPdf);
                    //xóa luôn file pdf
                    File.Delete(filePathPdf);
                }
                if (File.Exists(filePathNew))
                {
                    //xóa file word
                    File.Delete(filePathNew);
                }
            }
            return result;
        }

        #endregion
    }
}
