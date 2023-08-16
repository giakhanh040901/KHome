using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.FileEntities.Settings;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SignPdfDto;
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

namespace EPIC.CompanySharesDomain.Implements
{
    public partial class ContractDataServices : IContractDataServices
    {
        private readonly ILogger<ContractDataServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly SharedMediaApiUtils _sharedMediaApiUtils;
        private ContractTemplateRepository _contractTemplateRepository;
        private readonly OrderRepository _orderRepository;
        private readonly CpsSecondaryRepository _secondaryRepository;
        private readonly CpsPolicyRepository _policyRepository;
        private readonly CpsInfoRepository _cpsInfoRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly BankRepository _bankRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly InvestorBankAccountRepository _investorBankAccountRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;
        //private readonly GeneralContractorRepository _generalContractorRepository;
        //private readonly BondReceiveContractTemplateRepository _receiveContractTemplateRepository;
        private readonly SysVarRepository _sysVarRepository;
        //private readonly IInvestSharedServices _investSharedServices;
        private readonly SharedSignServerApiUtils _sharedSignServerApiUtils;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly IOptions<UrlConfirmReceiveContract> _urlConfirmReceiveContract;
        //private readonly OwnerRepository _ownerRepository;

        public ContractDataServices(
            ILogger<ContractDataServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            SharedMediaApiUtils sharedMediaApiUtils,
            SharedSignServerApiUtils sharedSignServerApiUtils,
            IOptions<FileConfig> fileConfig,
            IOptions<UrlConfirmReceiveContract> urlConfirmReceiveContract
        //IInvestSharedServices investSharedServices
        )
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _contractTemplateRepository = new ContractTemplateRepository(_connectionString, _logger);
            _orderRepository = new OrderRepository(_connectionString, _logger);
            _secondaryRepository = new CpsSecondaryRepository(_connectionString, _logger);
            _policyRepository = new CpsPolicyRepository(_connectionString, _logger);
            _cpsInfoRepository = new CpsInfoRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _investorBankAccountRepository = new InvestorBankAccountRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _sharedMediaApiUtils = sharedMediaApiUtils;
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            //_ownerRepository = new OwnerRepository(_connectionString, _logger);
            _sysVarRepository = new SysVarRepository(_connectionString, _logger);
            //_investSharedServices = investSharedServices;
            _sharedSignServerApiUtils = sharedSignServerApiUtils;
            _httpContext = httpContext;
            _fileConfig = fileConfig;
            _urlConfirmReceiveContract = urlConfirmReceiveContract;
        }

        #region sinh hợp đồng mẫu, đã ký, tải file

        /// <summary>
        /// Save contract không ký, sinh file word, pdf, pdf chưa ký
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public async Task<SaveFileDto> SaveContract(int orderId, int contractTemplateId, string isSign, int? tradingProviderId = null)
        {
            if (tradingProviderId == null)
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var result = new SaveFileDto();

            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, tradingProviderId ?? 0);
            if (contractTemplate == null || contractTemplate?.ContractTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            string filePath = _fileConfig.Value.Path;
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTempUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }

            //var cashFlows = GetCashFlow(orderId, bondPolicy.TradingProviderId);

            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            List<Task> tasks = new();

            //load file
            var replaceTexts = new List<ReplaceTextDto>();
            replaceTexts = GetDataContractFile(orderId, tradingProviderId ?? 0, true);
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

                //docText = mainPart.Document.Body.InnerText;
                docText = FindAndReplace(docText, replaceTexts);
                using (StreamWriter sw = new(mainPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                //In ra table đối với hợp đồng phụ lục dòng tiền               
                //CreateTableCashFlow(cashFlows, mainPart);

                try
                {
                    mainPart.Document.Save();
                }
                catch (XmlException ex)
                {
                    _logger.LogError(ex, "file word lỗi dạng XML");
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
            var fileNameNew = GenerateNewFileName(fileName);

            string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
            byte[] byteArrayThuong = memoryStreamFileThuong.ToArray();
            tasks.Add(File.WriteAllBytesAsync(filePathNew, byteArrayThuong));

            var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
            string filePathPdf = Path.Combine(filePath, fileResult.Folder, fileNamePdf);

            result.FileTempUrl = GetEndPoint("file/get", fileResult.Folder, fileNameNew);
            result.FileName = fileNamePdf;
            result.FileSignatureUrl = GetEndPoint("file/get", fileResult.Folder, fileNamePdf);
            result.FilePathToBeDeleted = filePathNew;


            tasks.Add(_sharedMediaApiUtils.ConvertWordToPdfAsync(byteArrayThuong, filePathPdf));
            #endregion

            #region xử lý ảnh con dấu
            var tradingProvider = _tradingProviderRepository.FindById(tradingProviderId ?? 0);
            if (tradingProvider == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy đại lý sơ cấp"), new FaultCode(((int)ErrorCode.TradingProviderNotFound).ToString()), "");
            }

            var businessCustomerTrading = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomerTrading == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin đại lý sơ cấp: {tradingProvider.BusinessCustomerId}"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
            }

            tasks.Add(Task.Run(async () =>
            {
                MemoryStream memoryStreamFileConDau = new();
                await memoryStreamFileConDau.WriteAsync(byteArrayThuong, 0, byteArrayThuong.Length);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memoryStreamFileConDau, true))
                {
                    wordDoc.ChangeDocumentType(WordprocessingDocumentType.Document);

                    var mainPart = wordDoc.MainDocumentPart;
                    await FillStampImage(businessCustomerTrading.StampImageUrl, mainPart, filePath);
                    try
                    {
                        mainPart.Document.Save();
                    }
                    catch (XmlException ex)
                    {
                        _logger.LogError(ex, "file word lỗi dạng XML");
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
            result.PageSign = pageCount;
            //đẩy các nhiệm vụ xử lý file ra ngoài
            await Task.WhenAll(tasks);
            return result;
        }

        /// <summary>
        /// Get data hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private List<ReplaceTextDto> GetDataContractFile(int orderId, int tradingProviderId, bool isSignature)
        {
            List<ReplaceTextDto> replaceTexts = new List<ReplaceTextDto>();
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.CpsOrderNotFound).ToString()), "");
            }
            var createdDate = order.PaymentFullDate ?? DateTime.Now;
            HopDongSoVaNgayLap(replaceTexts, order.ContractCode, createdDate);
            DateTime paymentFullDate = order.PaymentFullDate ?? DateTime.Now;
            ThongTinNhaDauTu(replaceTexts, order, isSignature);
            var policyDetail = _policyRepository.FindPolicyDetailById(order.CpsPolicyDetailId ?? 0, tradingProviderId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy kỳ hạn: {order.CpsPolicyDetailId ?? 0}"), new FaultCode(((int)ErrorCode.CpsPolicyDetailNotFound).ToString()), "");
            }
            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.CpsPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var secondary = _secondaryRepository.FindSecondaryById(policy.SecondaryId, policy.TradingProviderId);
            if (secondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.CpsSecondaryNotFound).ToString()), "");
            }
            //đại lý sơ cấp
            //var cashFlow = _investSharedServices.GetCashFlowContract(order.TotalValue ?? 0, paymentFullDate, policyDetail, policy, distribution, project);
            DaiLySoCap(replaceTexts, policyDetail.TradingProviderId);
            TaiKhoanThuHuongDaiLySoCap(replaceTexts, secondary.BusinessCustomerBankAccId ?? 0);
            //DuAn(replaceTexts, project);
            //DongTien(replaceTexts, cashFlow);  
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.TRAN_DATE, paymentFullDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.INTEREST_PERIOD, GetInterestPeriodTypeName(policyDetail.InterestType ?? 1, policyDetail.InterestPeriodQuantity, policyDetail.InterestPeriodType)),
                new ReplaceTextDto(PropertiesContractFile.TENOR, $"{policyDetail.PeriodQuantity} {GetNameDateType(policyDetail.PeriodType)}"),
            });

            return replaceTexts;
        }

        /// <summary>
        /// Tải hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public ExportResultDto ExportFileContract(int id, string contractType)
        {
            var result = new ExportResultDto();
            var secondaryContractFile = _orderContractFileRepository.FindById(id);
            string fileNotExists = "File hợp đồng không tồn tại.";
            if (secondaryContractFile == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy hợp đồng: {id}"), new FaultCode(((int)ErrorCode.CpsOrderContractFileNotFound).ToString()), "");
            }
            Dictionary<string, string> path = null;
            if (contractType == ContractFileTypes.TEMP)
            {
                if (secondaryContractFile.FileTempUrl == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy file word tạm của hợp đồng: {id}"), new FaultCode(((int)ErrorCode.CpsOrderContractFileNotFound).ToString()), "");
                }
                path = GetParams(secondaryContractFile.FileTempUrl);
                fileNotExists = "File hợp đồng word tạm không tồn tại.";
            }
            else if (contractType == ContractFileTypes.PDF)
            {
                if (secondaryContractFile.FileTempPdfUrl == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy file pdf tạm của hợp đồng: {id}"), new FaultCode(((int)ErrorCode.CpsOrderContractFileNotFound).ToString()), "");
                }
                path = GetParams(secondaryContractFile.FileTempPdfUrl);
                fileNotExists = "File hợp đồng pdf tạm không tồn tại.";
            }
            else if (contractType == ContractFileTypes.SCAN)
            {
                if (secondaryContractFile.FileScanUrl == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy file scan của hợp đồng: {id}"), new FaultCode(((int)ErrorCode.CpsOrderContractFileNotFound).ToString()), "");
                }
                path = GetParams(secondaryContractFile.FileScanUrl);
                fileNotExists = "File hợp đồng scan tạm không tồn tại.";
            }
            else if (contractType == ContractFileTypes.SIGNATURE)
            {
                if (secondaryContractFile.FileSignatureUrl == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy file đã ký của hợp đồng: {id}"), new FaultCode(((int)ErrorCode.CpsOrderContractFileNotFound).ToString()), "");
                }
                path = GetParams(secondaryContractFile.FileSignatureUrl);
                fileNotExists = "File hợp đồng đã ký không tồn tại.";
            }


            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName;
            string filePath = _fileConfig.Value.Path;
            var fullPath = GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                throw new FaultException(new FaultReason($"{fileNotExists}"), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }

        #endregion

        #region ký điện tử
        /// <summary>
        /// Ký điện tử
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public SaveFileDto SignContractPdf(int orderContractFileId, int contractTemplateId, int tradingProviderId)
        {
            var dto = GetSignPdfDto(tradingProviderId);
            var result = new SaveFileDto();
            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var orderContractFile = _orderContractFileRepository.FindById(orderContractFileId);
            if (orderContractFile == null || orderContractFile?.FileSignatureUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy bản ghi file hợp đồng: {contractTemplate.Name}"), new FaultCode(((int)ErrorCode.InvestOrderContractFileNotFound).ToString()), "");
            }
            string filePath = _fileConfig.Value.Path;

            var fileResult = FileUtils.GetPhysicalPath(orderContractFile.FileSignatureUrl, filePath);
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

        /// <summary>
        /// Get cấu hình chữ ký số theo đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
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
                AccessKey = businessCustomerTrading.Key,
                SecretKey = businessCustomerTrading.Secret,
                Server = businessCustomerTrading.Server,
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
        /// Lấy data hợp đồng tạm trên mobile
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="policyDetailId"></param>
        /// <param name="bankAccId"></param>
        /// <param name="identificationId"></param>
        /// <param name="contractCode"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataContractFileApp(decimal totalValue, int policyDetailId, int bankAccId, int identificationId, int? investorId, string contractCode)
        {
            var policyDetail = _policyRepository.FindPolicyDetailById(policyDetailId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.CpsPolicyDetailNotFound).ToString()), "");
            }

            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.CpsPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var distribution = _secondaryRepository.FindSecondaryById(policy.SecondaryId, policy.TradingProviderId);
            if (distribution == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.InvestDistributionNotFound).ToString()), "");
            }
            //dự án

            //var cashFlow = _investSharedServices.GetCashFlowContract(totalValue, DateTime.Now, policyDetail, policy, distribution, project);
            List<ReplaceTextDto> replaceTexts = new List<ReplaceTextDto>();

            var createdDate = DateTime.Now;
            HopDongSoVaNgayLap(replaceTexts, contractCode, createdDate);
            DaiLySoCap(replaceTexts, policyDetail.TradingProviderId);
            TaiKhoanThuHuongDaiLySoCap(replaceTexts, distribution.BusinessCustomerBankAccId ?? 0);
            //DuAn(replaceTexts, project);
            if (investorId == null)
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            ThongTinNhaDauTuCaNhan(replaceTexts, investorId ?? 0, identificationId, bankAccId);
            //DongTien(replaceTexts, cashFlow);
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.TRAN_DATE, createdDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.INTEREST_PERIOD, GetInterestPeriodTypeName(policyDetail.InterestType ?? 1, policyDetail.InterestPeriodQuantity, policyDetail.InterestPeriodType)),
                new ReplaceTextDto(PropertiesContractFile.TENOR, $"{policyDetail.PeriodQuantity} {GetNameDateType(policyDetail.PeriodType)}"),
                new ReplaceTextDto(PropertiesContractFile.SIGNATURE, $""),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME_SIGNATURE, $""),
            });
            return replaceTexts;
        }

        /// <summary>
        /// Fill Data file tạm app
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="policyDetailId"></param>
        /// <param name="BankAccId"></param>
        /// <param name="identificationId"></param>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExportContractApp(decimal totalValue, int policyDetailId, int BankAccId, int identificationId, int contractTemplateId, int? investorId = null)
        {
            string filePath = _fileConfig.Value.Path;
            var policyDetail = _policyRepository.FindPolicyDetailById(policyDetailId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.CpsPolicyDetailNotFound).ToString()), "");
            }

            var bondPolicy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (bondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.CpsPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var secondary = _secondaryRepository.FindSecondaryById(bondPolicy.SecondaryId, bondPolicy.TradingProviderId);
            if (secondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.InvestDistributionNotFound).ToString()), "");
            }
            var result = new ExportResultDto();
            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, secondary.TradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
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
            var replaceTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replaceTexts = GetDataContractFileApp(totalValue, policyDetailId, BankAccId, identificationId, investorId, "");
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
                    docText = FindAndReplace(docText, replaceTexts);
                    using (StreamWriter sw = new StreamWriter(mainPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }

                    #region In ra table đối với hợp đồng phụ lục dòng tiền
                    //var cashFlows = _investSharedServices.GetCashFlow(totalValue, policyDetailId);
                    //CreateTableCashFlow(cashFlows, mainPart);
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
                var fileNameNew = GenerateNewFileName(fileName);
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
                var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                string filePathPdf = Path.Combine(filePath, fileResult.Folder, fileNamePdf);
                result.fileDownloadName = fileNamePdf;
                result.filePath = GetEndPoint("file/get", fileResult.Folder, fileNamePdf);
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

        #region dòng tiền
        /*private CashFlowDto GetCashFlow(int orderId, int tradingProviderId)
        {
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }
            DateTime paymentFullDate = order.PaymentFullDate ?? DateTime.Now;
            var policyDetail = _policyRepository.FindPolicyDetailById((int)order.PolicyDetailId, tradingProviderId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy kỳ hạn: {order.PolicyDetailId}"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }
            var policy = _policyRepository.FindPolicyById(policyDetail.Id, policyDetail.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var distribution = _distributionRepository.FindById(policy.DistributionId, policy.TradingProviderId);
            if (distribution == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.InvestDistributionNotFound).ToString()), "");
            }
            //dự án
            var project = _projectRepository.FindById(distribution.ProjectId, null);
            if (project == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy dự án"), new FaultCode(((int)ErrorCode.InvestProjectNotFound).ToString()), "");
            }
            return _investSharedServices.GetCashFlowContract(order.TotalValue ?? 0, paymentFullDate, policyDetail, policy, distribution, project);
        }*/

        /*private void CreateTableCashFlow(CashFlowDto cashFlows, MainDocumentPart mainPart)
        {
            Body bod = mainPart.Document.Body;
            var table = bod.Descendants<Table>().FirstOrDefault(tbl => tbl.InnerText.Contains("Giá trị (sau thuế)"));

            if (table != null)
            {
                //// Create the table properties
                var tblProperties = GetBorder();
                //// Add the table properties to the table
                table.AppendChild(tblProperties);
                //table.Append(new TableRow(new TableCell(new Paragraph(new Run(new Text("test"))))));
                decimal sumValue = 0;
                int i = 1;
                foreach (var cashFlow in cashFlows.CashFlow)
                {
                    TableRow row = new TableRow();
                    string content = "Lợi nhuận đợt";
                    if (i == cashFlows.CashFlow.Count)
                    {
                        content = "Giá trị đầu tư + Lợi nhuận đợt";
                    }
                    List<TableCell> cell = new List<TableCell>()
                    {
                        new TableCell(new Paragraph(new Run(new Text($"{content} {i}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
                            }
                        }),
                        new TableCell(new Paragraph(new Run(new Text($"{cashFlow.ReceiveDate?.ToString("dd/MM/yyyy")}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
                            }
                        }),
                        new TableCell(new Paragraph(new Run(new Text($"{cashFlow.ReceiveValue.ToString("N0").Replace(",", ".")}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
                            }
                        }),
                        new TableCell(new Paragraph(new Run(new Text($"VNĐ")))
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
                    i++;
                }
                TableRow rowFooter = new TableRow();
                List<TableCell> cellFooter = new List<TableCell>()
                {
                    new TableCell(new Paragraph(new Run(new Text($"Tổng"), new RunProperties(new Bold())))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                    new TableCell(new Paragraph(new Run(new Text($"")))
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
                    }),
                    new TableCell(new Paragraph(new Run(new Text($"VNĐ")))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                };
                rowFooter.Append(cellFooter);
                table.Append(rowFooter);
            }

            var tableProfit = bod.Descendants<Table>().FirstOrDefault(tbl => tbl.InnerText.Contains("SỐ TIỀN LỢI TỨC THỰC NHẬN (VND)"));

            if (tableProfit != null)
            {
                //// Create the table properties
                var tblProperties = GetBorder();
                //// Add the table properties to the table
                tableProfit.AppendChild(tblProperties);
                //table.Append(new TableRow(new TableCell(new Paragraph(new Run(new Text("test"))))));
                decimal sumValue = 0;
                int i = 1;
                foreach (var cashFlow in cashFlows.CashFlow)
                {
                    TableRow row = new TableRow();
                    List<TableCell> cell = new List<TableCell>()
                    {
                        new TableCell(new Paragraph(new Run(new Text($"{i}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
                            }
                        }),
                        new TableCell(new Paragraph(new Run(new Text($"{cashFlow.ReceiveDate?.ToString("dd/MM/yyyy")}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
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
                    tableProfit.Append(row);
                    i++;
                }
                TableRow rowFooter = new TableRow();
                List<TableCell> cellFooter = new List<TableCell>()
                {
                    new TableCell(new Paragraph(new Run(new Text($"Tổng"), new RunProperties(new Bold())))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                    new TableCell(new Paragraph(new Run(new Text($"")))
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
                tableProfit.Append(rowFooter);
            }

            var tableSplitProfit = bod.Descendants<Table>().FirstOrDefault(tbl => tbl.InnerText.Contains("SỐ TIỀN THỰC NHẬN (VND)"));

            if (tableSplitProfit != null)
            {
                //// Create the table properties
                var tblProperties = GetBorder();
                //// Add the table properties to the table
                tableSplitProfit.AppendChild(tblProperties);
                //table.Append(new TableRow(new TableCell(new Paragraph(new Run(new Text("test"))))));
                decimal sumValue = 0;
                int i = 1;
                DateTime? endDate = DateTime.Now;
                foreach (var cashFlow in cashFlows.CashFlow)
                {
                    TableRow row = new TableRow();
                    string content = "Lợi tức";
                    var receiveValue = cashFlow.ReceiveValue;
                    if (i == cashFlows.CashFlow.Count)
                    {
                        receiveValue = cashFlow.ReceiveValue - cashFlows.TotalValue;
                        endDate = cashFlow.ReceiveDate;
                    }
                    List<TableCell> cell = new List<TableCell>()
                    {
                        new TableCell(new Paragraph(new Run(new Text($"{i}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
                            }
                        }),
                        new TableCell(new Paragraph(new Run(new Text($"{content}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
                            }
                        }),
                        new TableCell(new Paragraph(new Run(new Text($"{cashFlow.ReceiveDate?.ToString("dd/MM/yyyy")}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
                            }
                        }),
                        new TableCell(new Paragraph(new Run(new Text($"{receiveValue.ToString("N0").Replace(",", ".")}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification() { Val = JustificationValues.Center }
                            }
                        })
                    };
                    sumValue += cashFlow.ReceiveValue;
                    row.Append(cell);
                    tableSplitProfit.Append(row);
                    i++;
                }
                TableRow endDateRow = new TableRow();
                List<TableCell> endDateCell = new List<TableCell>()
                {
                    new TableCell(new Paragraph(new Run(new Text($"{i}"), new RunProperties(new Bold())))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                    new TableCell(new Paragraph(new Run(new Text($"Hoàn trả Khoản Đầu Tư")))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                    new TableCell(new Paragraph(new Run(new Text($"{endDate?.ToString("dd/MM/yyyy")}")))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                    new TableCell(new Paragraph(new Run(new Text($"{cashFlows.TotalValue.ToString("N0").Replace(",", ".")}")))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                };
                endDateRow.Append(endDateCell);
                tableSplitProfit.Append(endDateRow);
                TableRow rowFooter = new TableRow();
                List<TableCell> cellFooter = new List<TableCell>()
                {
                    new TableCell(new Paragraph(new Run(new Text($"Tổng số tiền nhận:"), new RunProperties(new Bold())))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                    new TableCell(new Paragraph(new Run(new Text($"")))
                    {
                        ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    }),
                    new TableCell(new Paragraph(new Run(new Text($"")))
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
                TableCell tc1 = new TableCell(new Paragraph(new Run(new Text($"Tổng số tiền nhận:"), new RunProperties(new Bold())))
                {
                    ParagraphProperties = new ParagraphProperties
                    {
                        Justification = new Justification() { Val = JustificationValues.Center }
                    }
                });
                tc1.TableCellProperties = new TableCellProperties();
                tc1.TableCellProperties.HorizontalMerge = new HorizontalMerge { Val = MergedCellValues.Restart };
                TableCell tc2 = new TableCell(new Paragraph(new Run(new Text($""), new RunProperties(new Bold())))
                {
                    ParagraphProperties = new ParagraphProperties
                    {
                        Justification = new Justification() { Val = JustificationValues.Center }
                    }
                });
                tc2.TableCellProperties = new TableCellProperties();
                tc2.TableCellProperties.HorizontalMerge = new HorizontalMerge { Val = MergedCellValues.Continue };
                TableCell tc3 = new TableCell(new Paragraph(new Run(new Text($""), new RunProperties(new Bold())))
                {
                    ParagraphProperties = new ParagraphProperties
                    {
                        Justification = new Justification() { Val = JustificationValues.Center }
                    }
                });
                tc3.TableCellProperties = new TableCellProperties();
                tc3.TableCellProperties.HorizontalMerge = new HorizontalMerge { Val = MergedCellValues.Continue };
                TableCell tc4 = new TableCell(new Paragraph(new Run(new Text($"{sumValue.ToString("N0").Replace(",", ".")}"), new RunProperties(new Bold())))
                {
                    ParagraphProperties = new ParagraphProperties
                    {
                        Justification = new Justification() { Val = JustificationValues.Center }
                    }
                });
                tc4.TableCellProperties = new TableCellProperties();
                tc4.TableCellProperties.HorizontalMerge = new HorizontalMerge { Val = MergedCellValues.Restart };

                rowFooter.Append(tc1);
                rowFooter.Append(tc2);
                rowFooter.Append(tc3);
                rowFooter.Append(tc4);

                tableSplitProfit.Append(rowFooter);


            }
        }*/

        #endregion

        #region Test fill data file mẫu

        private List<ReplaceTextDto> GetDataContractFileTest()
        {
            List<ReplaceTextDto> replaceTexts = new List<ReplaceTextDto>();
            var createdDate = DateTime.Now;
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, "EB123456"),
                new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, "EB123456"),
                new ReplaceTextDto(PropertiesContractFile.DAY_CONTRACT, createdDate, "dd"),
                new ReplaceTextDto(PropertiesContractFile.MONTH_CONTRACT, createdDate, "MM"),
                new ReplaceTextDto(PropertiesContractFile.YEAR_CONTRACT, createdDate, "yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_DATE, createdDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.TRAN_DATE, createdDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.INTEREST_PERIOD, "Cuối kỳ"),
                new ReplaceTextDto(PropertiesContractFile.SIGNATURE, $"Đã ký {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME_SIGNATURE, $"Nguyễn Văn A"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME, $"Nguyễn Văn A"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_REP_POSITION, $"Giám đốc"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ADDRESS, $"123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_REP_ADDRESS, $"123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_TRADING_ADDRESS, $"123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.BUSINESS_CUSTOMER_NAME, "Công ty TNHH ABC"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_NO, "0123456789123"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_TYPE, "CCCD"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_DATE, "13/09/2017"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_EXPIRED_DATE, "13/09/2022"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_ISSUER, "CA Thành Phố Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_PHONE, "0123456789"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_SEX, "Nam"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BIRTH_DATE, "01/01/1999"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_RESIDENT_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_EMAIL, "example@gmail.com"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_TAX_CODE, "12345676"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NO, "23219217132141"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NAME, "Nguyen Van A"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_NAME, "BankName"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_FULL_NAME, "Ngân hàng TMCP Xuất Nhập khẩu Việt Nam"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_BRANCH, "456 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_CONTRACT_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NATIONNALITY, "Việt Nam"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_DECISION_NO, "A123456"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_DECISION_DATE, "12/02/2022"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_LICENSE_DATE, "12/02/2022"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_LICENSE_ISSUER, "TP Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_NAME, "Công ty cổ phần đầu tư ABC"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_SHORT_NAME, "ABC"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_TAX_CODE, "2423535745"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_PHONE, "0123456789"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_FAX, "387492382423"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_LICENSE_ISSUER, "TP Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_LICENSE_DATE, "23/08/2015"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_REP_NAME, "Nguyễn Văn C"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_REP_POSITION, "Giám đốc"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_DECISION_NO, "73627563223"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_ACC_NAME, "Nguyen Van C"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_ACC_NO, "37264236756"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_NAME, "BankName"),
            });
            return replaceTexts;
        }

        public async Task<ExportResultDto> ExportContractPdfTest(int tradingProviderId, int contractTemplateId)
        {
            string filePath = _fileConfig.Value.Path;

            var result = new ExportResultDto();
            var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.CpsContractTemplateNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTempUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.CpsContractTemplateNotFound).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replaceTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replaceTexts = GetDataContractFileTest();
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
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                result.fileData = ms.ToArray();
                var fileNameNew = GenerateNewFileName(fileName);
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
                var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                string filePathPdf = Path.Combine(filePath, fileResult.Folder, fileNamePdf);
                result.fileDownloadName = fileNamePdf;
                result.filePath = GetEndPoint("file/get", fileResult.Folder, fileNamePdf);
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
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.CpsContractTemplateNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTempUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.CpsContractTemplateNotFound).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replaceTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replaceTexts = GetDataContractFileTest();
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
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                result.fileData = ms.ToArray();
                var fileNameNew = GenerateNewFileName(fileName);
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
                result.fileDownloadName = fileNameNew;
                result.filePath = GetEndPoint("file/get", fileResult.Folder, fileNameNew);
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
