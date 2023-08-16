using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.CoreDataUtils;
using EPIC.CoreSharedEntities.Dto.BusinessCustomer;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.Sale;
using EPIC.FileEntities.Settings;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestRepositories;
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
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace EPIC.InvestDomain.Implements
{
    public partial class ContractDataServices : IContractDataServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<ContractDataServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly SharedMediaApiUtils _sharedMediaApiUtils;
        private InvestContractTemplateEFRepository _contractTemplateRepository;
        private readonly InvestOrderRepository _orderRepository;
        private readonly DistributionRepository _distributionRepository;
        private InvestPolicyRepository _policyRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly BankRepository _bankRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly InvestorBankAccountRepository _investorBankAccountRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly SaleRepository _saleRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;
        private readonly GeneralContractorRepository _generalContractorRepository;
        private readonly ReceiveContractTemplateRepository _receiveContractTemplateRepository;
        private readonly SysVarRepository _sysVarRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly InvestOrderPaymentRepository _investOrderPaymentRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly InvestOrderContractFileEFRepository _investOrderContractFileEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly SharedSignServerApiUtils _sharedSignServerApiUtils;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly IOptions<UrlConfirmReceiveContract> _urlConfirmReceiveContract;
        private readonly OwnerRepository _ownerRepository;

        public ContractDataServices(
            EpicSchemaDbContext dbContext,
            ILogger<ContractDataServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            SharedMediaApiUtils sharedMediaApiUtils,
            SharedSignServerApiUtils sharedSignServerApiUtils,
            IOptions<FileConfig> fileConfig,
            IOptions<UrlConfirmReceiveContract> urlConfirmReceiveContract,
            IInvestSharedServices investSharedServices
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _contractTemplateRepository = new InvestContractTemplateEFRepository(dbContext, _logger);
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _investorBankAccountRepository = new InvestorBankAccountRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _sharedMediaApiUtils = sharedMediaApiUtils;
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _ownerRepository = new OwnerRepository(_connectionString, _logger);
            _sysVarRepository = new SysVarRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _investOrderContractFileEFRepository = new InvestOrderContractFileEFRepository(dbContext, logger);
            _investOrderPaymentRepository = new InvestOrderPaymentRepository(_connectionString, _logger);
            _receiveContractTemplateRepository = new ReceiveContractTemplateRepository(_connectionString, _logger);
            _generalContractorRepository = new GeneralContractorRepository(_connectionString, _logger);
            _investSharedServices = investSharedServices;
            _sharedSignServerApiUtils = sharedSignServerApiUtils;
            _httpContext = httpContext;
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


        //public SaveFileDto SaveContract(InvOrder order, int contractTemplateId)
        //{
        //    var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
        //    var result = new SaveFileDto();
        //    var contractTemplate = _contractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
        //    if (contractTemplate == null || contractTemplate?.ContractTempUrl == null)
        //    {
        //        throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
        //    }
        //    string filePath = _fileConfig.Value.Path;

        //    var fileResult = FileUtils.GetFullPathFile(contractTemplate.ContractTempUrl, filePath);
        //    if (!File.Exists(fileResult.FullPath))
        //    {
        //        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
        //    }
        //    var fileName = fileResult.FileName;
        //    var fullPath = fileResult.FullPath;

        //    var cashFlows = GetCashFlow(order, tradingProviderId);

        //    //load file
        //    var replaceTexts = new List<ReplaceTextDto>();
        //    replaceTexts = GetDataContractFile(order, tradingProviderId, false);
        //    byte[] byteArray = File.ReadAllBytes(fullPath);
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        ms.Write(byteArray, 0, byteArray.Length);
        //        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
        //        {
        //            var mainPart = wordDoc.MainDocumentPart;
        //            string docText = null;
        //            using (StreamReader sr = new StreamReader(mainPart.GetStream()))
        //            {
        //                docText = sr.ReadToEnd();
        //            }
        //            docText = FindAndReplace(docText, replaceTexts);

        //            using (StreamWriter sw = new StreamWriter(mainPart.GetStream(FileMode.Create)))
        //            {
        //                sw.Write(docText);
        //            }

        //            //In ra table đối với hợp đồng phụ lục dòng tiền
        //            CreateTableCashFlow(cashFlows, mainPart);

        //            try
        //            {
        //                mainPart.Document.Save();
        //            }
        //            catch (XmlException)
        //            {
        //                _logger.LogError("file word lỗi dạng XML");
        //                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
        //            }
        //        }
        //        var fileNameNew = ContractDataUtils.GetFileNameNewToSave(fileName);
        //        result.FileName = fileNameNew;
        //        result.FileSignatureUrl = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileNameNew);
        //        string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
        //        File.WriteAllBytes(filePathNew, ms.ToArray());
        //    }
        //    return result;
        //}

        /// <summary>
        /// sinh hợp đồng và trả về đường dẫn
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="policyDetailId"></param>
        /// <param name="replaceTexts">data hợp đồng</param>
        /// <returns></returns>
        public async Task<SaveFileDto> SaveContractApp(long orderId, int contractTemplateId, int policyDetailId, List<ReplaceTextDto> replaceTexts)
        {
            var order = _orderRepository.FindById(orderId, null,null, false);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }
            return await SaveContractApp(order, contractTemplateId, policyDetailId, replaceTexts);
        }

        /// <summary>
        /// sinh hợp đồng và trả về đường dẫn
        /// </summary>
        /// <param name="order"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="policyDetailId"></param>
        /// <param name="replaceTexts">data hợp đồng</param>
        /// <returns></returns>
        public async Task<SaveFileDto> SaveContractApp(InvOrder order, int contractTemplateId, int policyDetailId, List<ReplaceTextDto> replaceTexts)
        {
            var policyDetail = _policyRepository.FindPolicyDetailById(policyDetailId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }

            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy cif code: {cifCode}"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
            }
            var result = new SaveFileDto();
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            var contractTemplate = _contractTemplateRepository.FindByIdForUpdateContractFile(contractTemplateId, contractTemplateType, policy.TradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTemplateUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }

            string filePath = _fileConfig.Value.Path;
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTemplateUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File vật lý của mẫu của hợp đồng {contractTemplate.Name} không còn tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }

            var cashFlows = GetCashFlow(order, policy.TradingProviderId);

            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            List<Task> tasks = new();

            //load file
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
                docText = FindAndReplace(docText, replaceTexts);
                using (StreamWriter sw = new(mainPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
                //In ra table đối với hợp đồng phụ lục dòng tiền               
                CreateTableCashFlow(cashFlows, mainPart);
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
            var fileNameNew = ContractDataUtils.GenerateNewFileName(fileName);
            byte[] byteArrayThuong = memoryStreamFileThuong.ToArray();

            tasks.Add(File.WriteAllBytesAsync(Path.Combine(filePath, fileResult.Folder, fileNameNew), byteArrayThuong));

            var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
            result.FileTempUrl = ContractDataUtils.GetEndPoint(fileResult.Folder, fileNameNew);
            result.FileName = fileNamePdf;
            result.FileSignatureUrl = ContractDataUtils.GetEndPoint(fileResult.Folder, fileNamePdf);

            tasks.Add(_sharedMediaApiUtils.ConvertWordToPdfAsync(byteArrayThuong, Path.Combine(filePath, fileResult.Folder, fileNamePdf)));
            #endregion

            #region xử lý ảnh con dấu
            var tradingProvider = _tradingProviderRepository.FindById(policy.TradingProviderId);
            if (tradingProvider == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy đại lý sơ cấp"), new FaultCode(((int)ErrorCode.TradingProviderNotFound).ToString()), "");
            }

            tasks.Add(Task.Run(async () =>
            {
                MemoryStream memoryStreamFileConDau = new();
                await memoryStreamFileConDau.WriteAsync(byteArrayThuong, 0, byteArrayThuong.Length);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memoryStreamFileConDau, true))
                {
                    wordDoc.ChangeDocumentType(WordprocessingDocumentType.Document);

                    var mainPart = wordDoc.MainDocumentPart;
                    //Fill con dấu
                    await FillStampImage(tradingProvider.StampImageUrl, mainPart, filePath);
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
                        int.Parse(wordDoc.ExtendedFilePropertiesPart.Properties.Pages.Text.Trim());
                    }
                    else
                    {
                        //Nếu Page null mặc định lấy trang ký là trang đầu tiên
                        _logger.LogError("wordDoc.ExtendedFilePropertiesPart.Properties.Pages = null");
                    }
                }
                
                var fileNameSignPdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_PDF);
                await _sharedMediaApiUtils.ConvertWordToPdfAsync(memoryStreamFileConDau.ToArray(), Path.Combine(filePath, fileResult.Folder, fileNameSignPdf));
                //File pdf có con dấu
                result.FileSignatureStampUrl = ContractDataUtils.GetEndPoint(fileResult.Folder, fileNameSignPdf);
            }));
            #endregion
            result.PageSign = pageCount;
            //đẩy các nhiệm vụ xử lý file ra ngoài
            await Task.WhenAll(tasks);
            return result;
        }

        private async Task FillStampImage(string stampImageUrl, MainDocumentPart mainPart, string filePath)
        {
            if (stampImageUrl != null)
            {
                var stampImage = FileUtils.GetPhysicalPath(stampImageUrl, filePath);
                if (File.Exists(stampImage.FullPath))
                {
                    var images = await File.ReadAllBytesAsync(stampImage.FullPath);
                    MemoryStream imageStream = new(images);
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
        /// Ký điện tử và trả về đường dẫn
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public SaveFileDto SignContractPdf(int orderContractFileId, int contractTemplateId, int tradingProviderId)
        {
            var request = InitRequestSignPdf(tradingProviderId);
            var result = new SaveFileDto();
            var contractTemplate = _contractTemplateRepository.FindByIdForUpdateContractFile(contractTemplateId, SharedContractTemplateType.INVESTOR, tradingProviderId);
            if (contractTemplate == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var orderContractFile = _investOrderContractFileEFRepository.FindById(orderContractFileId);
            if (orderContractFile == null || orderContractFile?.FileSignatureUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy bản ghi file của hợp đồng: {contractTemplate.Name}"), new FaultCode(((int)ErrorCode.InvestOrderContractFileNotFound).ToString()), "");
            }
            string initPath = _fileConfig.Value.Path;

            var resultPhysicalPath = FileUtils.GetPhysicalPath(orderContractFile.FileSignatureUrl, initPath);
            var filePathSignPdf = resultPhysicalPath.FullPath.Replace(ContractFileExtensions.PDF, ContractFileExtensions.SIGN_PDF);
            if (!File.Exists(filePathSignPdf))
            {
                throw new FaultException(new FaultReason($"File lưu trữ của hợp đồng {contractTemplate.Name} không tồn tại (file {ContractFileExtensions.SIGN_PDF})."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            var fileNameSignPdf = resultPhysicalPath.FileName.Replace(ContractFileExtensions.PDF, ContractFileExtensions.SIGN_PDF);
            // Đọc file pdf vừa chuyển đổi để ký   
            request.FilePdfByteArray = File.ReadAllBytes(filePathSignPdf);
            request.pageSign = orderContractFile.PageSign;

            var requestCopy = new RequestSignPdfDto
            {
                FilePdfByteArray = null,
                Base64Image = null,
                FileDownloadName = fileNameSignPdf,
                AccessKey = request.AccessKey,
                Height = request.Height,
                SecretKey = request.SecretKey,
                pageSign = request.pageSign,
                Server = request.Server,
                SignatureName = request.SignatureName,
                TextOut = request.TextOut,
                TypeSign = request.TypeSign,
                Width = request.Width,
                XPoint = request.XPoint,
                YPoint = request.YPoint
            };
            _logger.LogInformation($"gọi api ký: {JsonSerializer.Serialize(requestCopy)}");
            //lưu chữ ký điện tử
            byte[] fileSignedByteArray;
            try
            {
                fileSignedByteArray = _sharedSignServerApiUtils.RequestSignPdf(request) 
                    ?? throw new Exception("fileSignedByteArray == null");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ký điện tử không thành công. Vui lòng kiểm tra lại cấu hình!");
                throw new FaultException(new FaultReason($"Ký điện tử không thành công. Vui lòng kiểm tra lại cấu hình!"), new FaultCode(((int)ErrorCode.CoreSignPdfFailed).ToString()), "");
            }

            var fileSignedName = ContractDataUtils.GenerateNewFileName(fileNameSignPdf);
            File.WriteAllBytes(Path.Combine(initPath, resultPhysicalPath.Folder, fileSignedName), fileSignedByteArray);
            result.FileName = fileSignedName;
            result.FileSignatureUrl = ContractDataUtils.GetEndPoint(resultPhysicalPath.Folder, fileSignedName);
            result.FilePathToBeDeleted = resultPhysicalPath.FullPath;
            return result;
        }

        /// <summary>
        /// Lấy các thông tin cấu hình ký cho request ký
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        private RequestSignPdfDto InitRequestSignPdf(int tradingProviderId)
        {
            var tradingProvider = _tradingProviderRepository.FindById(tradingProviderId)
                ?? throw new FaultException(new FaultReason($"Không tìm thấy đại lý sơ cấp"), new FaultCode(((int)ErrorCode.TradingProviderNotFound).ToString()), "");
            var businessCustomerTrading = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId)
                ?? throw new FaultException(new FaultReason($"Không tìm thấy thông tin đại lý sơ cấp: {tradingProvider.BusinessCustomerId}"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
            
            var xPoint = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "X_POINT")?.VarValue);
            var yPoint = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "Y_POINT")?.VarValue);
            var width = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "WIDTH")?.VarValue);
            var height = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "HEIGHT")?.VarValue);

            var initRequest = new RequestSignPdfDto()
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
            if (initRequest.AccessKey == null || initRequest.SecretKey == null || initRequest.Server == null)
            {
                throw new FaultException(new FaultReason($"Không thể ký điện tử do khách hàng doanh nghiệp thiếu cấu hình"), new FaultCode(((int)ErrorCode.CoreBusinessCustomerSettingSignPdfNotFound).ToString()), "");
            }
            if (!Uri.IsWellFormedUriString(initRequest.Server, UriKind.Absolute))
            {
                throw new FaultException(new FaultReason($"Cấu hình server ký điện tử không hợp lệ"), new FaultCode(((int)ErrorCode.CoreSignPdfServerNotValid).ToString()), "");
            }
            return initRequest;
        }

        #region tải file
        /// <summary>
        /// Tải file lưu trữ (scan)
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <returns></returns>
        public ExportResultDto ExportFileScanContract(int orderContractFileId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var secondaryContractFile = _orderContractFileRepository.FindById(orderContractFileId);
            if (secondaryContractFile == null || secondaryContractFile?.FileScanUrl == null)
            {
                throw new FaultException(new FaultReason($"File scan hợp đồng sinh lỗi: {orderContractFileId}"), new FaultCode(((int)ErrorCode.InvestOrderContractFileNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(secondaryContractFile.FileScanUrl);
            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                _logger.LogError($"FileScanUrl không tồn tại: fullPath = {fullPath}, orderContractFileId = {orderContractFileId}");
                throw new FaultException(new FaultReason($"File scan hợp đồng không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }

        /// <summary>
        /// Tải file word
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <returns></returns>
        public ExportResultDto ExportContractTemp(int orderContractFileId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var orderContractFile = _orderContractFileRepository.FindById(orderContractFileId);
            if (orderContractFile == null || orderContractFile?.FileTempUrl == null)
            {
                throw new FaultException(new FaultReason($"File word bị sinh lỗi: {orderContractFileId}"), new FaultCode(((int)ErrorCode.InvestOrderContractFileNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(orderContractFile.FileTempUrl);
            var folder = path["folder"];
            var fileName = path["file"];
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                _logger.LogError($"FileTempUrl không tồn tại: fullPath = {fullPath}, orderContractFileId = {orderContractFileId}");
                throw new FaultException(new FaultReason($"Đường dẫn file không tồn tại"), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            result.fileDownloadName = fileName;
            return result;
        }

        /// <summary>
        /// Tải file pdf
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <returns></returns>
        public ExportResultDto ExportContractTempPdf(int orderContractFileId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var orderContractFile = _orderContractFileRepository.FindById(orderContractFileId);
            if (orderContractFile == null || orderContractFile?.FileTempPdfUrl == null)
            {
                throw new FaultException(new FaultReason($"File pdf bị sinh lỗi: {orderContractFileId}"), new FaultCode(((int)ErrorCode.InvestOrderContractFileNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(orderContractFile.FileTempPdfUrl);
            var folder = path["folder"];
            var fileName = path["file"];
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                _logger.LogError($"FileTempPdfUrl không tồn tại: fullPath = {fullPath}, orderContractFileId = {orderContractFileId}");
                throw new FaultException(new FaultReason($"File hợp đồng mẫu không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            result.fileDownloadName = fileName;
            return result;
        }

        /// <summary>
        /// Tải file đã ký
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <returns></returns>
        public ExportResultDto ExportContractSignature(int orderContractFileId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var orderContractFile = _orderContractFileRepository.FindById(orderContractFileId);
            if (orderContractFile == null || orderContractFile?.FileSignatureUrl == null)
            {
                throw new FaultException(new FaultReason($"File hợp đồng đã ký sinh lỗi: {orderContractFileId}"), new FaultCode(((int)ErrorCode.InvestOrderContractFileNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(orderContractFile.FileSignatureUrl);
            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                _logger.LogError($"FileSignatureUrl không tồn tại: fullPath = {fullPath}, orderContractFileId = {orderContractFileId}");
                throw new FaultException(new FaultReason($"File hợp đồng đã ký không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }

        public ExportResultDto ExportContractSignatureApp(int orderContractFileId)
        {
            var result = new ExportResultDto();
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            Dictionary<string, string> path = null;
            var secondaryContractFile = _orderContractFileRepository.FindById(orderContractFileId, investorId);
            if (secondaryContractFile == null)
            {
                throw new FaultException(new FaultReason($"File hợp đồng không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
               
            if (secondaryContractFile.FileScanUrl != null)
            {
                path = ContractDataUtils.GetParams(secondaryContractFile.FileScanUrl);
            } 
            else if (secondaryContractFile.FileSignatureUrl != null)
            {
                path = ContractDataUtils.GetParams(secondaryContractFile.FileSignatureUrl);
            } 
            else
            {
                throw new FaultException(new FaultReason($"File hợp đồng đã ký không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                _logger.LogError($"FileScanUrl không tồn tại: fullPath = {fullPath}, orderContractFileId = {orderContractFileId}");
                throw new FaultException(new FaultReason($"File scan hợp đồng không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }

        /// <summary>
        /// Tải file hợp đồng xem tạm
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="policyDetailId"></param>
        /// <param name="BankAccId"></param>
        /// <param name="identificationId"></param>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExportContractApp(decimal totalValue, int policyDetailId, int BankAccId, int identificationId, int contractTemplateId, List<ReplaceTextDto> replaceTexts, int? investorId = null)
        {
            string filePath = _fileConfig.Value.Path;
            var policyDetail = _policyRepository.FindPolicyDetailById(policyDetailId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }

            var bondPolicy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (bondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var distribution = _distributionRepository.FindById(bondPolicy.DistributionId, bondPolicy.TradingProviderId);
            if (distribution == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.InvestDistributionNotFound).ToString()), "");
            }
            var result = new ExportResultDto();
            var contractTemplate = _contractTemplateRepository.FindByIdForUpdateContractFile(contractTemplateId, SharedContractTemplateType.INVESTOR, distribution.TradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTemplateUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            //var contractCode = contractTemplate.Code;
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTemplateUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;

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
                    //Dòng tiền
                    CreateTableCashFlow(_investSharedServices.GetCashFlow(totalValue, policyDetailId), mainPart);
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
                var fileNamePdf = ContractDataUtils.GenerateNewFileName(fileName).Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                result.fileDownloadName = fileNamePdf;
                result.filePath = ContractDataUtils.GetEndPoint(fileResult.Folder, fileNamePdf);
                //convert sang file pdf
                result.fileData = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
            }
            return result;
        }
        #endregion

        /// <summary>
        /// Lấy dữ liệu dòng tiền
        /// </summary>
        /// <param name="order"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        private CashFlowDto GetCashFlow(InvOrder order, int tradingProviderId)
        {
            DateTime paymentFullDate = order.PaymentFullDate ?? DateTime.Now;
            var policyDetail = _policyRepository.FindPolicyDetailById((int)order.PolicyDetailId, tradingProviderId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy kỳ hạn: {order.PolicyDetailId}"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }
            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
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
            return _investSharedServices.GetCashFlowContract(order.InitTotalValue, paymentFullDate, policyDetail, policy, distribution, project);
        }

        private void CreateTableCashFlow(CashFlowDto cashFlows, MainDocumentPart mainPart)
        {
            Body bod = mainPart.Document.Body;
            var table = bod.Descendants<Table>().FirstOrDefault(tbl => tbl.InnerText.Contains("Giá trị (sau thuế)"));

            if (table != null)
            {
                //// Add the table properties to the table
                table.AppendChild(CreateTableBorder());
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
                //// Add the table properties to the table
                tableProfit.AppendChild(CreateTableBorder());
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
                //// Add the table properties to the table
                tableSplitProfit.AppendChild(CreateTableBorder());
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
        }

        private TableProperties CreateTableBorder()
        {
            TableBorders tblBorders = new();
            tblBorders.AppendChild(new TopBorder()
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "CC0000"
            });

            tblBorders.AppendChild(new BottomBorder()
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            });

            tblBorders.AppendChild(new RightBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            });

            tblBorders.AppendChild(new LeftBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            });

            tblBorders.AppendChild(new InsideHorizontalBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            });

            tblBorders.AppendChild(new InsideVerticalBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            });

            TableProperties tblProperties = new();
            //// Add the table borders to the properties
            tblProperties.AppendChild(tblBorders);
            return tblProperties;
        }

        #region Receive contract file
        /// <summary>
        /// Lấy data hợp đồng giao nhận
        /// </summary>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataReceiveContractFile(InvOrder order, int? tradingProviderId = null)
        {
            List<ReplaceTextDto> replaceTexts = new List<ReplaceTextDto>();
            var tradingProviderIds = tradingProviderId ?? CommonUtils.GetCurrentTradingProviderId(_httpContext);
            DaiLySoCap(replaceTexts, tradingProviderIds);
            ThongTinNhaDauTu(replaceTexts, order, false);

            string contactCode = order.ContractCode;
            var orderContractFile = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO);
            if (orderContractFile.Any())
            {
                if (orderContractFile.GroupBy(e => e.ContractCodeGen).Count() == 1)
                {
                    contactCode = orderContractFile.First().ContractCodeGen;
                }
            }
            replaceTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contactCode)
                });
            return replaceTexts;
        }

        public async Task<ExportResultDto> ExportContractReceive(int orderId, int distributionId, int tradingProviderId, int source)
        {
            string filePath = _fileConfig.Value.Path;
            var result = new ExportResultDto();
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }
            var contractTemplate = _receiveContractTemplateRepository.FindAll(distributionId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.FileUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng giao nhận của bán theo kỳ hạn id: bondSecondaryId"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }

            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.FileUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng giao nhận không tồn tại."), new FaultCode(((int)ErrorCode.FileNotFound).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            // GetData
            var replaceTexts = new List<ReplaceTextDto>();
            replaceTexts = GetDataReceiveContractFile(order, tradingProviderId);

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
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng giao nhận lỗi. Vui lòng upload lại mẫu hợp đồng giao nhận khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                var fileNamePdf = ContractDataUtils.GenerateNewFileName(fileName).Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                result.fileDownloadName = fileNamePdf;
                result.filePath = ContractDataUtils.GetEndPoint(fileResult.Folder, fileNamePdf);
                //convert sang file pdf
                result.fileData = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
            }
            return result;
        }
        #endregion


        #region Test fill data file mẫu
        /// <summary>
        /// Fill data vào hợp đồng pdf (Mẫu hợp đồng phân phối)
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <param name="type">hợp đồng dành cho cá nhân hay doanh nghiệp</param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExportContractPdfTest(int contractTemplateId, string type)
        {
            string filePath = _fileConfig.Value.Path;
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var contractTemplate = _contractTemplateRepository.FindByIdForUpdateContractFile(contractTemplateId, type, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTemplateUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTemplateUrl, filePath);
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
            replaceTexts = CoreBaseData.GetDataContractFileTest();
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

                var fileNamePdf = ContractDataUtils.GenerateNewFileName(fileName).Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                result.fileDownloadName = fileNamePdf;
                result.filePath = ContractDataUtils.GetEndPoint(fileResult.Folder, fileNamePdf);
                //convert sang file pdf
                result.fileData = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
            }
            return result;
        }

        /// <summary>
        /// Fill tạm data vào hợp đồng word (Mẫu hợp đồng phân phối)
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public ExportResultDto ExportContractWordTest(int contractTemplateId, string type)
        {
            string filePath = _fileConfig.Value.Path;
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var contractTemplate = _contractTemplateRepository.FindByIdForUpdateContractFile(contractTemplateId, type, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.ContractTemplateUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.ContractTemplateUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //get data
            var replaceTexts = CoreBaseData.GetDataContractFileTest();

            byte[] byteArray = File.ReadAllBytes(fullPath);
            using MemoryStream ms = new();
            ms.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
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
            result.fileDownloadName = fileNameNew;
            result.filePath = ContractDataUtils.GetEndPoint(fileResult.Folder, fileNameNew);
            return result;
        }

        /// <summary>
        /// Fill tạm data vào hợp dồng word (ẫu hợp đồng cài đặt)
        /// </summary>
        /// <param name="contractTemplateTempId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public ExportResultDto ExportContractTempWordTest(int contractTemplateTempId, string type)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string filePath = _fileConfig.Value.Path;
            var result = new ExportResultDto();
            var contractTemplateTemp = _dbContext.InvestContractTemplateTemps.FirstOrDefault(e => e.Id == contractTemplateTempId && e.TradingProviderId == tradingProviderId);
            if (contractTemplateTemp == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateTempId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            string contractTemplateUrl = type == SharedContractTemplateType.INVESTOR ? contractTemplateTemp.FileInvestor : contractTemplateTemp.FileBusinessCustomer;
            if (contractTemplateUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateTempId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplateUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplateTemp.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //GetData
            var replaceTexts = new List<ReplaceTextDto>();
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
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplateTemp.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                result.fileData = ms.ToArray();
                var fileNameNew = ContractDataUtils.GenerateNewFileName(fileName);
                result.fileDownloadName = fileNameNew;
                result.filePath = ContractDataUtils.GetEndPoint(fileResult.Folder, fileNameNew);
            }
            return result;
        }

        /// <summary>
        /// Fill data vào hợp đồng pdf (Mẫu hợp đồng cài đặt)
        /// </summary>
        /// <param name="contractTemplateTempId"></param>
        /// <param name="type">hợp đồng dành cho cá nhân hay doanh nghiệp</param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExportContractTempPdfTest(int contractTemplateTempId, string type)
        {
            string filePath = _fileConfig.Value.Path;
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new ExportResultDto();
            var contractTemplateTemp = _dbContext.InvestContractTemplateTemps.FirstOrDefault(e => e.Id == contractTemplateTempId && e.TradingProviderId == tradingProviderId);
            if (contractTemplateTemp == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateTempId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            string contractTemplateUrl = type == SharedContractTemplateType.INVESTOR ? contractTemplateTemp.FileInvestor : contractTemplateTemp.FileBusinessCustomer;
            if (contractTemplateUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateTempId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplateUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplateTemp.Name} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replaceTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replaceTexts = CoreBaseData.GetDataContractFileTest();
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
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplateTemp.Name} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                var fileNamePdf = ContractDataUtils.GenerateNewFileName(fileName).Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                result.fileDownloadName = fileNamePdf;
                result.filePath = ContractDataUtils.GetEndPoint(fileResult.Folder, fileNamePdf);
                //convert sang file pdf
                result.fileData = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
            }
            return result;
        }
        #endregion

        #region Test fill data file hợp đồng giao nhận
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
            var replaceTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replaceTexts = CoreBaseData.GetDataContractFileTest();
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
                var fileNamePdf = ContractDataUtils.GenerateNewFileName(fileName).Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                result.fileDownloadName = fileNamePdf;
                result.filePath = ContractDataUtils.GetEndPoint(fileResult.Folder, fileNamePdf);
                //convert sang file pdf
                //Trả về file pdf
                 result.fileData = await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray());
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Lấy thêm dữ liệu để fill data hợp đồng rút tiền
        /// </summary>
        /// <param name="order"></param>
        /// <param name="policy"></param>
        /// <param name="policyDetail"></param>
        /// <param name="tongTienCondauTu"></param>
        /// <param name="soTienRut"></param>
        /// <param name="ngayRut"></param>
        /// <param name="isKhachCaNhan"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataWithdrawalContractFile(InvOrder order, Policy policy, PolicyDetail policyDetail, decimal tongTienCondauTu, decimal soTienRut, DateTime ngayRut, bool isKhachCaNhan, DateTime? distributionCloseSellDate)
        {
            _logger.LogInformation($"{nameof(GetDataWithdrawalContractFile)}: order = {JsonSerializer.Serialize(order)}, tongTienCondauTu = {tongTienCondauTu}, soTienRut = {soTienRut}");
            List<ReplaceTextDto> replaceTexts = new();
            var rutVonData = _investSharedServices.RutVonInvest(order, policy, policyDetail, tongTienCondauTu, soTienRut, ngayRut, isKhachCaNhan, distributionCloseSellDate);

            replaceTexts.AddRange(
            new List<ReplaceTextDto>()
            {
                #region tính toán
                new ReplaceTextDto(PropertiesContractFile.ACTUALLY_PROFIT, (double?)rutVonData?.ActuallyProfit, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.AMOUNT_RECEIVED, (double?)rutVonData?.ActuallyAmount, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.AMOUNT_MONEY, (double?)rutVonData?.AmountMoney, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TAX, (double?)rutVonData?.IncomeTax, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.DEDUCTIBLE_PROFIT, (double?)rutVonData?.ProfitReceived, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.PROFIT, (double?)rutVonData?.ProfitRate, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.WITH_DRAWAL_FEE, (double?)rutVonData?.WithdrawalFee, EnumReplaceTextFormat.NumberVietNam)
                #endregion
            });
            return replaceTexts;
        }

        #region Get Data hợp đồng tái tục
        /// <summary>
        /// Lấy thêm dữ liệu để fill data hợp đồng tái tục
        /// </summary>
        /// <param name="policyDetail"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataRenewalsContractFile(PolicyDetail policyDetail)
        {
            _logger.LogInformation($"{nameof(GetDataWithdrawalContractFile)}: policyDetail = {JsonSerializer.Serialize(policyDetail)}");
            List<ReplaceTextDto> replaceTexts = new();
            replaceTexts.AddRange(
            new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.INTEREST_PERIOD_RENEWALS, ContractDataUtils.GetInterestPeriodTypeName(policyDetail.InterestType ?? 1, policyDetail.InterestPeriodQuantity, policyDetail.InterestPeriodType))
            });
            return replaceTexts;
        }
        #endregion

        #region Get data cho hợp đồng
        /// <summary>
        /// Get data hợp đồng
        /// </summary>
        /// <param name="order"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="isSignature"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataContractFile(InvOrder order, int tradingProviderId, bool isSignature)
        {
            List<ReplaceTextDto> replaceTexts = new List<ReplaceTextDto>();
            DateTime createdDate = DateTime.Now;
            if (order.PaymentFullDate == null)
            {
                var tranDate = _dbContext.InvestOrderPayments.Where(op => op.OrderId == order.Id && op.Deleted == YesNo.NO && op.Status == OrderPaymentStatus.DA_THANH_TOAN);
                if (tranDate.Any())
                {
                    createdDate = tranDate.Max(e => e.TranDate).Value;
                }
            }
            else
            {
                createdDate = (DateTime)order.PaymentFullDate;
            }
            DateTime paymentFullDate = order.PaymentFullDate ?? DateTime.Now;
            InvestorDataForContractDto investor = new();
            BusinessCustomerForContractDto businessCustomer = new();
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            if (cifCode.InvestorId != null)
            {
                var investorId = cifCode.InvestorId;
                investor = _investorEFRepository.GetDataInvestorForContract(investorId ?? 0, order.InvestorIdenId ?? 0, order.InvestorBankAccId ?? 0, order.ContractAddressId ?? 0);
                businessCustomer = null;
            }
            else
            {
                var businessCustomerId = cifCode.BusinessCustomerId;
                businessCustomer = _businessCustomerEFRepository.GetBusinessCustomerForContract(businessCustomerId ?? 0, order.InvestorBankAccId ?? 0);
                investor = null;
            }
            var policyDetail = _policyRepository.FindPolicyDetailById(order.PolicyDetailId, tradingProviderId, false);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy kỳ hạn: {order.PolicyDetailId}"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }
            var department = _departmentRepository.FindById(order.DepartmentId ?? 0, policyDetail.TradingProviderId);
            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId, false);
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
            int? distributeTradingBank = 0;
            var orderPayment = _investOrderPaymentRepository.GetListPaymentSuccess((int)order.Id).FirstOrDefault();
            distributeTradingBank = orderPayment?.TradingBankAccId;
            if (orderPayment == null)
            {
                //nếu payment null thì lấy first trong inv_dis_trading_bank
                var distributeBank = _tradingProviderRepository.FindBankByTrading(tradingProviderId, distribution.Id).FirstOrDefault();
                distributeTradingBank = distributeBank?.BusinessCustomerBankAccId;
            }
            var tradingProvider = _tradingProviderEFRepository.GetTradingProviderForContract(order.TradingProviderId, distributeTradingBank ?? 0);
            replaceTexts.AddRange(CoreBaseData.GetBaseDataForContract(createdDate, investor, businessCustomer, tradingProvider, order.Source, isSignature));

            //đại lý sơ cấp
            var project = _projectRepository.FindById(order.ProjectId, null);
            if (project == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy dự án"), new FaultCode(((int)ErrorCode.InvestProjectNotFound).ToString()), "");
            }
            //chủ đầu tư
            var owner = _ownerRepository.FindById(project.OwnerId);
            if (owner == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chủ đầu tư"), new FaultCode(((int)ErrorCode.InvestOwnerNotFound).ToString()), "");
            }

            var businessCustomerOwner = _businessCustomerRepository.FindBusinessCustomerById(owner.BusinessCustomerId);
            if (businessCustomerOwner == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin khách hàng doanh nghiệp là chủ đầu tư"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
            }
            //tổng thầu
            var generalContractor = _generalContractorRepository.FindById(project.GeneralContractorId);
            if (generalContractor == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy tổng thầu"), new FaultCode(((int)ErrorCode.InvestGeneralContractorNotFound).ToString()), "");
            }

            var businessCustomerGeneralContractor = _businessCustomerRepository.FindBusinessCustomerById(generalContractor.BusinessCustomerId);
            if (businessCustomerGeneralContractor == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin khách hàng doanh nghiệp là tổng thầu"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
            }
            var sale = _saleRepository.SaleGetInfoByReferralCode(order.SaleReferralCode);
            var cashFlow = _investSharedServices.GetCashFlowContract(order.TotalValue, paymentFullDate, policyDetail, policy, distribution, project);

            replaceTexts.AddRange(GetDataForContract(businessCustomerOwner, businessCustomerGeneralContractor, project, cashFlow, sale));
            var tenor = $"{policyDetail.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail.PeriodType)}";
            //if (tradingProviderId == 195)
            //{
            //    if (policyDetail.PeriodType == PeriodUnit.MONTH)
            //    {
            //        tenor = policyDetail.PeriodQuantity > 6 ? tenor : PeriodQuantityFix.THIRTY_SIX_MONTH;
            //    }
            //    else if (policyDetail.PeriodType == PeriodUnit.DAY)
            //    {
            //        tenor = policyDetail.PeriodQuantity > 180 ? tenor : PeriodQuantityFix.THIRTY_SIX_MONTH;
            //    }
            //}
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto
                {
                    FindText = "{{TranDate}}",
                    ReplaceText = paymentFullDate.ToString("dd/MM/yyyy")
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerSaleReferralCode}}",
                    ReplaceText = order.SaleReferralCode
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerSaleName}}",
                    ReplaceText = sale?.Fullname
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerSalePhone}}",
                    ReplaceText = sale?.Phone
                },
                new ReplaceTextDto
                {
                    FindText = "{{PolicyName}}",
                    ReplaceText = policy?.Name
                },
                new ReplaceTextDto
                {
                    FindText = "{{DepartmentName}}",
                    ReplaceText = department?.DepartmentName
                },
                new ReplaceTextDto
                {
                    FindText = "{{InterestPeriod}}",
                    ReplaceText = ContractDataUtils.GetInterestPeriodTypeName(policyDetail.InterestType ?? 1, policyDetail.InterestPeriodQuantity, policyDetail.InterestPeriodType)

                },
                new ReplaceTextDto
                {
                    FindText = "{{Tenor}}",
                    ReplaceText = $"{policyDetail.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail.PeriodType)}"
                },
                new ReplaceTextDto
                {
                    FindText = "{{TenorFix36Month}}",
                    ReplaceText = tenor
                },
            });

            return replaceTexts;
        }

        /// <summary>
        /// Fill data hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<ReplaceTextDto> GetDataContractFile(int orderId, int tradingProviderId, bool isSignature)
        {
            List<ReplaceTextDto> replaceTexts = new List<ReplaceTextDto>();
            var order = _orderRepository.FindById(orderId, tradingProviderId, null, false);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }
            return GetDataContractFile(order, tradingProviderId, isSignature);
        }

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
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }

            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
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
            //nếu payment null thì lấy first trong inv_dis_trading_bank
            var distributeBank = _tradingProviderRepository.FindBankByTrading(policy.TradingProviderId, distribution.Id).FirstOrDefault();
            var distributeTradingBank = distributeBank?.BusinessCustomerBankAccId;
            var tradingProvider = _tradingProviderEFRepository.GetTradingProviderForContract(policy.TradingProviderId, distributeTradingBank ?? 0);
            //dự án
            var project = _projectRepository.FindById(distribution.ProjectId, null);
            if (project == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy dự án"), new FaultCode(((int)ErrorCode.InvestProjectNotFound).ToString()), "");
            }

            var cashFlow = _investSharedServices.GetCashFlowContract(totalValue, DateTime.Now, policyDetail, policy, distribution, project);
            List<ReplaceTextDto> replaceTexts = new List<ReplaceTextDto>();

            var createdDate = DateTime.Now;
            replaceTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TranDate}}",
                ReplaceText = createdDate.ToString("dd/MM/yyyy")
            });
            //chủ đầu tư
            var owner = _ownerRepository.FindById(project.OwnerId);
            if (owner == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chủ đầu tư"), new FaultCode(((int)ErrorCode.InvestOwnerNotFound).ToString()), "");
            }

            var businessCustomerOwner = _businessCustomerRepository.FindBusinessCustomerById(owner.BusinessCustomerId);
            if (businessCustomerOwner == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin khách hàng doanh nghiệp là chủ đầu tư"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
            }
            //tổng thầu
            var generalContractor = _generalContractorRepository.FindById(project.GeneralContractorId);
            if (generalContractor == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy tổng thầu"), new FaultCode(((int)ErrorCode.InvestGeneralContractorNotFound).ToString()), "");
            }

            var businessCustomerGeneralContractor = _businessCustomerRepository.FindBusinessCustomerById(generalContractor.BusinessCustomerId);
            if (businessCustomerGeneralContractor == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin khách hàng doanh nghiệp là tổng thầu"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
            }
            if (investorId == null)
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            var investor = _investorEFRepository.GetDataInvestorForContract(investorId ?? 0, identificationId, bankAccId);
            replaceTexts.AddRange(CoreBaseData.GetBaseDataForContract(createdDate, investor, null, tradingProvider, SourceOrder.ONLINE, false));
            replaceTexts.AddRange(GetDataForContract(businessCustomerOwner, businessCustomerGeneralContractor, project, cashFlow, null));
            replaceTexts.Add(new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode));
            var tenor = $"{policyDetail.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail.PeriodType)}";
           
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto
                {
                   FindText = "{{PolicyName}}",
                    ReplaceText = ""

                },
                 new ReplaceTextDto
                {
                    FindText = "{{DepartmentName}}",
                    ReplaceText = ""

                },
                new ReplaceTextDto
                {
                    FindText = "{{InterestPeriod}}",
                    ReplaceText = ContractDataUtils.GetInterestPeriodTypeName(policyDetail.InterestType ?? 1, policyDetail.InterestPeriodQuantity, policyDetail.InterestPeriodType)

                },
                new ReplaceTextDto
                {
                    FindText = "{{Tenor}}",
                    ReplaceText = $"{policyDetail.PeriodQuantity} {ContractDataUtils.GetNameDateType(policyDetail.PeriodType)}"
                },
                new ReplaceTextDto
                {
                    FindText = "{{TenorFix36Month}}",
                    ReplaceText = tenor
                },
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
            return replaceTexts;
        }

        public List<ReplaceTextDto> GetDataForContract(Entities.DataEntities.BusinessCustomer businessCustomerOwner, Entities.DataEntities.BusinessCustomer businessCustomerGeneralContractor, Project project, CashFlowDto cashFlow, SaleInvestorDto sale)
        {
            List<ReplaceTextDto> replaceTexts = new();
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                #region chủ đầu tư
                new ReplaceTextDto(PropertiesContractFile.OWNER_NAME, businessCustomerOwner?.Name),
                new ReplaceTextDto(PropertiesContractFile.OWNER_SHORT_NAME, businessCustomerOwner?.ShortName),    
                #endregion
                
                #region tổng thầu
                new ReplaceTextDto(PropertiesContractFile.GENERAL_CONTRACTOR_NAME, businessCustomerGeneralContractor?.Name),
                new ReplaceTextDto(PropertiesContractFile.GENERAL_CONTRACTOR_SHORT_NAME, businessCustomerGeneralContractor?.ShortName),    
                #endregion
                
                #region Dự án
                new ReplaceTextDto(PropertiesContractFile.PROJECT_NAME, project?.InvName),
                new ReplaceTextDto(PropertiesContractFile.PROJECT_ADDRESS, project?.LocationDescription),    
                #endregion 
                
                #region Dự án
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_SALE_NAME, sale?.Fullname),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_SALE_PHONE, sale?.Phone),    
                #endregion
           

                #region tính toán, dòng tiền
                new ReplaceTextDto(PropertiesContractFile.INTEREST_CASHFLOW, (double)cashFlow?.InterestRate, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.START_DATE_CASHFLOW, cashFlow?.StartDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.END_DATE_CASHFLOW, cashFlow?.EndDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.DAY_END_DATE_CASHFLOW, cashFlow?.EndDate, "dd"),
                new ReplaceTextDto(PropertiesContractFile.MONTH_END_DATE_CASHFLOW, cashFlow?.EndDate, "MM"),
                new ReplaceTextDto(PropertiesContractFile.YEAR_END_DATE_CASHFLOW, cashFlow?.EndDate, "yyyy"),
                new ReplaceTextDto(PropertiesContractFile.INVEST_MONEY_CASHFLOW, (double)cashFlow?.TotalValue, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TAX_PROFIT_CASHFLOW, (double)cashFlow?.TaxProfit, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TAX, (double)cashFlow?.Tax, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TOTAL_RECEIVE_VALUE_CASHFLOW, (double)cashFlow?.TotalReceiveValue, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.ACTUALLY_PROFIT_CASHFLOW, (double)cashFlow?.ActuallyProfit, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.BEFORE_TAX_PROFIT_CASHFLOW, (double)cashFlow?.BeforeTaxProfit, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.INVEST_MONEY_TEXT_CASHFLOW, (double)cashFlow?.TotalValue, EnumReplaceTextFormat.NumberToWord),
                #endregion
            });
            return replaceTexts;
        }
        #endregion
    }
}
