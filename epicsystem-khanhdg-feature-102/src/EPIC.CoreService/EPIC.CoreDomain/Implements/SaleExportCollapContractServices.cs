using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.CollabContract;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.Sale;
using EPIC.Entities.Dto.UploadFile;
using EPIC.FileEntities.Settings;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SignPdfDto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml;

namespace EPIC.CoreDomain.Implements
{
    public partial class SaleExportCollapContractServices : ISaleExportCollapContractServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<SaleExportCollapContractServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICollabContractServices _collabContractServices;
        private readonly string _connectionString;
        private readonly SaleRepository _saleRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ApproveRepository _approveRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly CollabContractTemplateRepository _collabContractTemplateRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly InvestorBankAccountRepository _investorBankAccountRepository;
        private readonly BankRepository _bankRepository;
        private readonly SharedMediaApiUtils _sharedMediaApiUtils;
        private readonly SharedSignServerApiUtils _sharedSignServerApiUtils;
        private readonly SaleCollabContractRepository _saleCollabContractRepository;
        private readonly SysVarRepository _sysVarRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<FileConfig> _fileConfig;

        public SaleExportCollapContractServices(
             EpicSchemaDbContext dbContext,
            ILogger<SaleExportCollapContractServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IOptions<FileConfig> fileConfig,
            SharedMediaApiUtils sharedMediaApiUtils,
            ICollabContractServices collabContractServices,
            SharedSignServerApiUtils sharedSignServerApiUtils
            )
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _collabContractTemplateRepository = new CollabContractTemplateRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _investorBankAccountRepository = new InvestorBankAccountRepository(_connectionString, _logger);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _sharedMediaApiUtils = sharedMediaApiUtils;
            _sharedSignServerApiUtils = sharedSignServerApiUtils;
            _saleCollabContractRepository = new SaleCollabContractRepository(_connectionString, _logger);
            _sysVarRepository = new SysVarRepository(_connectionString, _logger);
            _collabContractServices = collabContractServices;
            _mapper = mapper;
            _fileConfig = fileConfig;
            _saleEFRepository = new SaleEFRepository(_dbContext, _logger);
        }

        public void UpdateContractFile(int saleId)
        {
            using (var transaction = new TransactionScope())
            {
                var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                var sale = _saleRepository.FindById(saleId, tradingProviderId);

                var saleCheck = _saleEFRepository.AppCheckInvetorSaleSource(sale.InvestorId ?? 0, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.CoreSaleNotFound);
                //Lấy ra danh sách hợp đồng
                var contractTemplate = _collabContractServices.FindAllBySale(-1, 1, saleId, tradingProviderId);
                if (contractTemplate != null)
                {
                    foreach (var contract in contractTemplate.Items)
                    {
                        //Fill hợp đồng và lưu trữ
                        var saveFile = SaveContract(contract.Id, saleId, tradingProviderId, sale.InvestorId);
                        var saveFileApp = new SaveFileDto();
                        if (contract.IsSign == IsSignPdf.No || contract.IsSign == null)
                        {
                            saveFileApp = SaveContractPdfNotSign(contract.Id, saleId, tradingProviderId, sale.InvestorId, saleCheck.Source == SaleSource.ONLINE ? true : false)?.Result;
                            if (File.Exists(saveFileApp.FilePathToBeDeleted)) //xóa file word app sinh tạm khi convert sang pdf
                            {
                                File.Delete(saveFileApp.FilePathToBeDeleted);
                            }
                            var filePathSignOld = saveFileApp.FilePathToBeDeleted.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_DOCX);
                            if (File.Exists(filePathSignOld)) //xóa file word app sinh tạm khi convert sang pdf, file có con dấu
                            {
                                File.Delete(filePathSignOld);
                            }
                        }
                        if (contract.CollabContractId != null)
                        {
                            var collabContract = _saleCollabContractRepository.FindById(contract.CollabContractId ?? 0, tradingProviderId);
                            if (collabContract?.FileTempUrl != null)
                            {
                                string filePath = _fileConfig.Value.Path;

                                var fileResult = FileUtils.GetPhysicalPathNoCheckExists(collabContract.FileTempUrl, filePath);
                                if (File.Exists(fileResult.FullPath))
                                {
                                    File.Delete(fileResult.FullPath);
                                }
                            }
                            if (saveFileApp?.FileSignatureUrl != null && collabContract?.FileSignatureUrl != null)
                            {
                                string filePath = _fileConfig.Value.Path;

                                var fileResult = FileUtils.GetPhysicalPathNoCheckExists(collabContract.FileSignatureUrl, filePath);
                                if (File.Exists(fileResult.FullPath))
                                {
                                    File.Delete(fileResult.FullPath);
                                }

                                //xóa file pdf có con dấu
                                var FullSignPath = fileResult.FullPath.Replace(".pdf", "-Sign.pdf");
                                if (File.Exists(FullSignPath))
                                {
                                    File.Delete(FullSignPath);
                                }
                            }

                            //Lưu đường dẫn vào bảng Sale Collab Contract  
                            collabContract.FileTempUrl = saveFile.FileSignatureUrl;
                            collabContract.FileSignatureUrl = saveFileApp?.FileSignatureUrl ?? collabContract.FileSignatureUrl;
                            collabContract.PageSign = saveFileApp?.PageSign ?? collabContract.PageSign;
                            _saleCollabContractRepository.Update(collabContract);
                        }
                        else
                        {
                            //Lưu đường dẫn vào bảng Sale collab Contract
                            var collabContract = new CollabContract
                            {
                                Id = contract.Id,
                                CollabContractTempId = contract.Id,
                                FileTempUrl = saveFile.FileSignatureUrl,
                                FileSignatureUrl = saveFileApp?.FileSignatureUrl,
                                SaleId = saleId,
                                TradingProviderId = tradingProviderId,
                                PageSign = saveFileApp?.PageSign ?? 1
                            };
                            _saleCollabContractRepository.Add(collabContract);
                        }

                    }
                }
                transaction.Complete();
            }
        }

        public void UpdateContractFileSignPdf(int saleId)
        {
            using (var transaction = new TransactionScope())
            {
                var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                var sale = _saleRepository.FindById(saleId, tradingProviderId);

                //Lấy ra danh sách hợp đồng
                var contractTemplate = _collabContractServices.FindAllBySale(-1, 1, saleId, tradingProviderId);
                if (contractTemplate != null)
                {
                    foreach (var contract in contractTemplate.Items)
                    {
                        if (contract.IsSign == IsSignPdf.No || contract.IsSign == null)
                        {

                            var saveFile = SaveContractSignPdf(contract.Id, contract.CollabContractId ?? 0, tradingProviderId);

                            if (contract.CollabContractId != null)
                            {
                                var collabContract = _saleCollabContractRepository.FindById(contract.CollabContractId ?? 0, tradingProviderId);
                                if (collabContract?.FileSignatureUrl != null)
                                {
                                    string filePath = _fileConfig.Value.Path;

                                    var fileResult = FileUtils.GetPhysicalPathNoCheckExists(collabContract.FileSignatureUrl, filePath);
                                    if (File.Exists(fileResult.FullPath))
                                    {
                                        File.Delete(fileResult.FullPath);
                                    }
                                }

                                //Lưu đường dẫn vào bảng Sale Collab Contract
                                collabContract.FileSignatureUrl = saveFile.FileSignatureUrl;
                                collabContract.IsSign = IsSignPdf.Yes;
                                _saleCollabContractRepository.Update(collabContract);
                            }
                        }
                    }
                }
                transaction.Complete();
            }
        }

        /// <summary>
        /// cập nhật trên web sinh lại file pdf không có chữ ký điện tử
        /// </summary>
        /// <param name="collapContractTempId"></param>
        /// <param name="saleId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public async Task<SaveFileDto> SaveContractPdfNotSign(int collapContractTempId, int saleId, int tradingProviderId, int? investorId = null, bool? isSaleOnline = false)
        {
            //var currentInvestorId = investorId ?? CommonUtils.GetCurrentInvestorId(_httpContext);
            var result = new SaveFileDto();
            var sale = _saleRepository.FindSaleById(saleId, tradingProviderId);
            var contractTemplate = _collabContractTemplateRepository.FindById(collapContractTempId, tradingProviderId);
            string filePath = _fileConfig.Value.Path;
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.FileUrl ?? "", filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Title} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            //load file
            var replateTexts = new List<ReplaceTextDto>();
            if (isSaleOnline == true)
            {
                replateTexts = GetDataContractFile(tradingProviderId, saleId, true);
            } else
            {
                replateTexts = GetDataContractFile(tradingProviderId, saleId, false);
            }

            byte[] byteArray = File.ReadAllBytes(fullPath);
            int pageCount = 1;
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
                    catch (XmlException e)
                    {
                        _logger.LogError(e, "file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Title} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
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
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
                var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                string filePathPdf = Path.Combine(filePath, fileResult.Folder, fileNamePdf);
                await _sharedMediaApiUtils.ConvertWordToPdfAsync(filePathNew, filePathPdf);
                #region xử lý ảnh con dấu
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
                {
                    wordDoc.ChangeDocumentType(WordprocessingDocumentType.Document);

                    var mainPart = wordDoc.MainDocumentPart;

                    #region Xử lý ảnh con dấu
                    FillStampImage(tradingProviderId, mainPart, filePath);
                    #endregion
                    try
                    {
                        mainPart.Document.Save();
                    }
                    catch (XmlException e)
                    {
                        _logger.LogError(e, "file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Title} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
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
                File.WriteAllBytes(filePathNewSign, ms.ToArray());
                var fileNameSignPdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_PDF);
                string filePathSignPdf = Path.Combine(filePath, fileResult.Folder, fileNameSignPdf);
                await _sharedMediaApiUtils.ConvertWordToPdfAsync(filePathNewSign, filePathSignPdf);
                #endregion
                result.FileName = fileNamePdf;
                result.FileSignatureUrl = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileNamePdf);
                result.FilePathToBeDeleted = filePathNew;
                result.PageSign = pageCount;
            }
            return result;
        }

        /// <summary>
        /// Sinh file đã ký + dấu mộc trên app
        /// </summary>
        /// <param name="collapContractTempId"></param>
        /// <param name="saleId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public async Task<SaveFileDto> SaveContractPdfSignApp(int collapContractTempId, int saleId, int tradingProviderId, int? investorId = null)
        {
            var dto = GetSignPdfDto(tradingProviderId);
            if (dto.AccessKey == null || dto.SecretKey == null || dto.Server == null)
            {
                throw new FaultException(new FaultReason($"Không thể ký điện tử do khách hàng doanh nghiệp thiếu cấu hình"), new FaultCode(((int)ErrorCode.CoreBusinessCustomerSettingSignPdfNotFound).ToString()), "");
            }
            var currentInvestorId = investorId ?? CommonUtils.GetCurrentInvestorId(_httpContext);
            var result = new SaveFileDto();
            var sale = _saleRepository.FindSaleById(saleId, tradingProviderId);
            var contractTemplate = _collabContractTemplateRepository.FindById(collapContractTempId, tradingProviderId);
            string filePath = _fileConfig.Value.Path;
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.FileUrl ?? "", filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Title} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            //load file
            var replateTexts = new List<ReplaceTextDto>();
            replateTexts = GetDataContractFileApp(currentInvestorId, tradingProviderId, sale?.InvestorBankAccId ?? 0, sale.ContractCode, true);

            byte[] byteArray = File.ReadAllBytes(fullPath);
            int pageCount = 1;
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
                    #region Xử lý ảnh con dấu
                    FillStampImage(tradingProviderId, mainPart, filePath);
                    #endregion
                    try
                    {
                        mainPart.Document.Save();
                    }
                    catch (XmlException e)
                    {
                        _logger.LogError(e, "file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Title} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
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
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                var fileNamePdf = fileNameNew.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.PDF);
                string filePathPdf = Path.Combine(filePath, fileResult.Folder, fileNamePdf);
                await _sharedMediaApiUtils.ConvertWordToPdfAsync(ms.ToArray(), filePathPdf);
                byte[] byteArrPdf = File.ReadAllBytes(filePathPdf);
                dto.FilePdfByteArray = byteArrPdf;
                dto.pageSign = pageCount;
                var fileSign = _sharedSignServerApiUtils.RequestSignPdf(dto);
                if (fileSign == null)
                {
                    throw new FaultException(new FaultReason($"Ký điện tử không thành công."), new FaultCode(((int)ErrorCode.CoreSignPdfFailed).ToString()), "");
                }
                var fileSignName = ContractDataUtils.GenerateNewFileName(fileNamePdf);
                //tạo đường dẫn lưu file chữ ký điện tử
                string filePathSign = Path.Combine(filePath, fileResult.Folder, fileSignName);
                File.WriteAllBytes(filePathSign, fileSign);

                result.FileName = fileSignName;
                result.FileSignatureUrl = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileSignName);
                //result.FilePathOld = fileNameNew;
                result.PageSign = pageCount;

                // xóa file word
                if (File.Exists(filePathNew))
                {
                    File.Delete(filePathNew);
                }

                // xóa file pdf chuyển đổi
                if (File.Exists(filePathPdf))
                {
                    File.Delete(filePathPdf);
                }
            }
            return result;
        }
        private void FillStampImage(int tradingProviderId, MainDocumentPart mainPart, string filePath)
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
            if (businessCustomerTrading.StampImageUrl != null)
            {
                var stampImage = FileUtils.GetPhysicalPath(businessCustomerTrading.StampImageUrl, filePath);
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
        /// lưu file pdf và trả về đường dẫn vật lý
        /// </summary>
        /// <param name="collapContractTempId"></param>
        /// <param name="saleCollabContractId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public SaveFileDto SaveContractSignPdf(int collapContractTempId, int saleCollabContractId, int tradingProviderId)
        {
            var dto = GetSignPdfDto(tradingProviderId);
            if (dto.AccessKey == null || dto.SecretKey == null || dto.Server == null)
            {
                throw new FaultException(new FaultReason($"Không thể ký điện tử do khách hàng doanh nghiệp thiếu cấu hình"), new FaultCode(((int)ErrorCode.CoreBusinessCustomerSettingSignPdfNotFound).ToString()), "");
            }
            var result = new SaveFileDto();
            var contractTemplate = _collabContractTemplateRepository.FindById(collapContractTempId, tradingProviderId);
            string filePath = _fileConfig.Value.Path;
            var contractFile = _saleCollabContractRepository.FindById(saleCollabContractId);
            if (contractFile == null || contractFile.FileSignatureUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy file đã ký hợp đồng {contractTemplate?.Title}"), new FaultCode(((int)ErrorCode.CoreSaleCollabContractNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractFile.FileSignatureUrl, filePath);
            var fileSignPdf = fileResult.FullPath.Replace(".pdf", "-Sign.pdf");
            if (!File.Exists(fileSignPdf))
            {
                throw new FaultException(new FaultReason($"File đã ký của hợp đồng {contractTemplate.Title} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName.Replace(".pdf", "-Sign.pdf");
            var fullPath = fileSignPdf;

            byte[] byteArray = File.ReadAllBytes(fullPath);
            dto.FilePdfByteArray = byteArray;
            dto.pageSign = contractFile.PageSign;
            //lưu chữ ký điện tử
            var fileSign = _sharedSignServerApiUtils.RequestSignPdf(dto);
            if (fileSign == null)
            {
                throw new FaultException(new FaultReason($"Ký điện tử không thành công."), new FaultCode(((int)ErrorCode.CoreSignPdfFailed).ToString()), "");
            }
            var fileSignName = ContractDataUtils.GenerateNewFileName(fileName);
            //tạo đường dẫn lưu file chữ ký điện tử
            string filePathSign = Path.Combine(filePath, fileResult.Folder, fileSignName);
            File.WriteAllBytes(filePathSign, fileSign);
            result.FileName = fileSignName;
            result.FileSignatureUrl = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileSignName);
            result.FilePathToBeDeleted = fullPath; //trả ra file pdf để xóa
            return result;
        }

        private RequestSignPdfDto GetSignPdfDto(int tradingProviderId)
        {
            var tradingProvider = _tradingProviderRepository.FindById(tradingProviderId);
            if (tradingProvider == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy đại lý sơ cấp"), new FaultCode(((int)ErrorCode.TradingProviderNotFound).ToString()), "");
            }
            var digitalSignFind = _tradingProviderRepository.GetDigitalSign(tradingProviderId);
            if (digitalSignFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin đại lý sơ cấp: {tradingProvider.BusinessCustomerId}"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
            }
            var xPoint = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "X_POINT")?.VarValue);
            var yPoint = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "Y_POINT")?.VarValue);
            var width = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "WIDTH")?.VarValue);
            var height = int.Parse(_sysVarRepository.GetVarByName("SIGN_PDF", "HEIGHT")?.VarValue);

            var dto = new RequestSignPdfDto()
            {
                TypeSign = SignPdfType.TYPE_PDFSIGNATURE_TEXT,
                AccessKey = digitalSignFind.Key,
                SecretKey = digitalSignFind.Secret,
                Server = digitalSignFind.Server,
                XPoint = xPoint,
                YPoint = yPoint,
                Width = width,
                Height = height
            };
            return dto;
        }

        /// <summary>
        /// lưu file pdf và trả về đường dẫn vật lý
        /// </summary>
        /// <param name="collapContractTempId"></param>
        /// <param name="saleTempId"></param>
        /// <returns></returns>
        public SaveFileDto SaveContract(int collapContractTempId, int saleId, int tradingProviderId, int? investorId = null)
        {
            var currentInvestorId = investorId ?? CommonUtils.GetCurrentInvestorId(_httpContext);
            var result = new SaveFileDto();
            var sale = _saleRepository.FindSaleById(saleId, tradingProviderId);
            var contractTemplate = _collabContractTemplateRepository.FindById(collapContractTempId, tradingProviderId);
            string filePath = _fileConfig.Value.Path;
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.FileUrl ?? "", filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate?.Title} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            //load file
            var replateTexts = new List<ReplaceTextDto>();
            replateTexts = GetDataContractFile(tradingProviderId, saleId, false);

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
                    catch (XmlException e)
                    {
                        _logger.LogError(e, "file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate?.Title} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
                    }
                }
                var fileNameNew = ContractDataUtils.GenerateNewFileName(fileName);
                string filePathNew = Path.Combine(filePath, fileResult.Folder, fileNameNew);
                File.WriteAllBytes(filePathNew, ms.ToArray());
                result.FileName = fileNameNew;
                result.FileSignatureUrl = ContractDataUtils.GetEndPoint("file/get", fileResult.Folder, fileNameNew);
            }
            return result;
        }

        /// <summary>
        /// khi đặt lệnh bằng App thì sinh ra file chữ ký
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="filePath"></param>
        public async Task<List<CollabContractDto>> UpdateContractFileApp(int saleId, int tradingProviderId)
        {
            var result = new List<CollabContractDto>();
            var sale = _saleRepository.FindById(saleId, tradingProviderId);
            var type = CollabContractTempType.INVESTOR;
            if (sale.BusinessCustomerId != null)
            {
                type = CollabContractTempType.BUSINESS_CUSTOMER;
            }
            //Lấy ra danh sách hợp đồng
            var contractTemplate = _collabContractTemplateRepository.FindAll(-1, 1, CollapContractTemplateStatus.ACTIVE, null, tradingProviderId, type);
            if (contractTemplate != null)
            {
                foreach (var contract in contractTemplate.Items)
                {
                    SaveFileDto saveFile = null;
                    try
                    {
                        //Fill hợp đồng và lưu trữ
                        saveFile = await SaveContractPdfSignApp(contract.Id, saleId, tradingProviderId);
                    }
                    catch (Exception ex) //file lỗi
                    {
                        _logger.LogError(ex, $"Lỗi sinh file hợp đồng sale mobile app: saleId = {saleId}, tradingProviderId = {tradingProviderId}, contractId = {contract.Id}");
                    }

                    if (saveFile != null)
                    {
                        //Lưu đường dẫn vào bảng 
                        var collabContract = new CollabContract
                        {
                            CollabContractTempId = contract.Id,
                            FileSignatureUrl = saveFile.FileSignatureUrl,
                            SaleId = saleId,
                            TradingProviderId = tradingProviderId,
                            PageSign = saveFile.PageSign,
                            IsSign = IsSignPdf.Yes
                        };
                        result.Add(_mapper.Map<CollabContractDto>(collabContract));
                        _saleCollabContractRepository.Add(collabContract);
                    }
                }
            }
            return result;
        }

        public async Task<ExportResultDto> ExportContractApp(int collapContractTempId, int saleTempId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            string filePath = _fileConfig.Value.Path;

            var result = new ExportResultDto();
            var saleTemp = _saleRepository.FindSaleTemp(saleTempId, null);
            var collapContractTemplate = _collabContractTemplateRepository.FindById(collapContractTempId, saleTemp?.TradingProviderId ?? 0);
            if (collapContractTemplate == null)
            {
                //throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            //var contractCode = collapContractTemplate.Code;
            var fileResult = FileUtils.GetPhysicalPath(collapContractTemplate?.FileUrl ?? "", filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {collapContractTemplate.Title} không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var fileName = fileResult.FileName;
            var fullPath = fileResult.FullPath;

            result.fileDownloadName = fileName;
            //load file
            var replateTexts = new List<ReplaceTextDto>();
            #region get data theo phân loại và loại hợp đồng
            replateTexts = GetDataContractFileApp(investorId, saleTemp?.TradingProviderId ?? 0, saleTemp?.InvestorBankAccId ?? 0, "", false);
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
                    catch (XmlException e)
                    {
                        _logger.LogError(e, "file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {collapContractTemplate.Title} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
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

        public ExportResultDto ExportContractSignature(int collabContractId)
        {
            var result = new ExportResultDto();
            var contractFile = _saleCollabContractRepository.FindById(collabContractId);
            if (contractFile == null || contractFile?.FileSignatureUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy file đã ký hợp đồng sale: {collabContractId}"), new FaultCode(((int)ErrorCode.CoreSaleCollabContractNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(contractFile.FileSignatureUrl);
            
            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName;
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                throw new FaultException(new FaultReason($"File đã ký hợp đồng sale không tồn tại."), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }
        public ExportResultDto AppExportContractSignature(int collabContractId, int tradingProviderId)
        {
            var result = new ExportResultDto();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var contractFile = _saleCollabContractRepository.FindById(collabContractId);
            Dictionary<string, string> path = null;
            var saleTemp = _saleEFRepository.AppCheckInvetorSaleSource(investorId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.CoreSaleNotFound);
            if (saleTemp.Source == SaleSource.ONLINE)
            {
                if (contractFile == null || contractFile?.FileSignatureUrl == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy file đã ký hợp đồng sale: {collabContractId}"), new FaultCode(((int)ErrorCode.CoreSaleCollabContractNotFound).ToString()), "");
                }
                path = ContractDataUtils.GetParams(contractFile.FileSignatureUrl);
            }
            else if (saleTemp.Source == SaleSource.OFFLINE)
            {
                if (contractFile == null || contractFile?.FileScanUrl == null)
                {
                    //Nếu không có file scan thì lấy file chưa ký
                    path = ContractDataUtils.GetParams(contractFile.FileSignatureUrl);
                } else 
                {
                    path = ContractDataUtils.GetParams(contractFile.FileScanUrl);
                }
            }

            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName;
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                throw new FaultException(new FaultReason($"File không tồn tại."), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }

        public ExportResultDto ExportContractTemp(int collabContractId)
        {
            var result = new ExportResultDto();
            var contractFile = _saleCollabContractRepository.FindById(collabContractId);
            if (contractFile == null || contractFile?.FileTempUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy file word hợp đồng sale: {collabContractId}, vui lòng cập nhật lại hợp đồng."), new FaultCode(((int)ErrorCode.CoreSaleCollabContractNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(contractFile.FileTempUrl);
            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName;
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                throw new FaultException(new FaultReason($"File tạm hợp đồng sale không tồn tại."), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }

        public ExportResultDto ExportContractScan(int collabContractId)
        {
            var result = new ExportResultDto();
            var contractFile = _saleCollabContractRepository.FindById(collabContractId);
            if (contractFile == null || contractFile?.FileScanUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy file scan hợp đồng sale: {collabContractId}"), new FaultCode(((int)ErrorCode.CoreSaleCollabContractNotFound).ToString()), "");
            }
            var path = ContractDataUtils.GetParams(contractFile.FileScanUrl);
            var folder = path["folder"];
            var fileName = path["file"];
            result.fileDownloadName = fileName;
            string filePath = _fileConfig.Value.Path;
            var fullPath = ContractDataUtils.GetFullPathFile(folder, fileName, filePath);
            if (!File.Exists(fullPath))
            {
                throw new FaultException(new FaultReason($"File scan hợp đồng sale id: {collabContractId} không tồn tại."), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            //load file
            result.fileData = File.ReadAllBytes(fullPath);
            return result;
        }

        public int UpdateCollabContractFileScan(UpdateSaleCollabContractDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var collabContract = _saleCollabContractRepository.FindById(input.CollabContractId);
            if (collabContract == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy hợp đồng: {input.CollabContractId}"), new FaultCode(((int)ErrorCode.CoreSaleCollabContractNotFound).ToString()), "");
            }
            collabContract.FileScanUrl = input.FileScanUrl;
            return _saleCollabContractRepository.Update(collabContract);
        }

        public CollabContract CreateCollabContractFileScan(CreateSaleCollabContractDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var collabContract = new CollabContract
            {
                CollabContractTempId = input.CollabContractTempId,
                FileScanUrl = input.FileScanUrl,
                SaleId = input.SaleId,
                TradingProviderId = tradingProviderId
            };
            return _saleCollabContractRepository.Add(collabContract);
        }

        /// <summary>
        /// Get data hợp đồng app
        /// </summary>
        /// <returns></returns>
        private List<ReplaceTextDto> GetDataContractFileApp(int? investorId, int tradingProviderId, int bankAccId, string contractCode, bool isSignature, int? businessCustomerId = null)
        {
            List<ReplaceTextDto> replateTexts = new List<ReplaceTextDto>();
            var createdDate = DateTime.Now;
            HopDongSoVaNgayLap(replateTexts, contractCode, createdDate);
            ThongTinNhaDauTuCaNhan(replateTexts, investorId ?? 0, bankAccId, isSignature);
            DaiLySoCap(replateTexts, tradingProviderId);

            return replateTexts;
        }

        /// <summary>
        /// Get Data hợp đồng
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="bankAccId"></param>
        /// <param name="contractCode"></param>
        /// <param name="isSignature"></param>
        /// <param name="businessCustomerId"></param>
        /// <returns></returns>
        private List<ReplaceTextDto> GetDataContractFile(int tradingProviderId, int saleId, bool isSignature)
        {
            List<ReplaceTextDto> replateTexts = new List<ReplaceTextDto>();
            var createdDate = DateTime.Now;
            var sale = _saleRepository.FindSaleById(saleId, tradingProviderId);
            HopDongSoVaNgayLap(replateTexts, sale.ContractCode, createdDate);
            ThongTinNhaDauTu(replateTexts, saleId, tradingProviderId, isSignature);
            DaiLySoCap(replateTexts, tradingProviderId);

            return replateTexts;
        }
        public List<AppListCollabContractSignDto> FindAllFileSignatureForApp(int tradingProviderId)
        {
            int saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var contractFileSignatures = _saleCollabContractRepository.FindAllList(saleId, tradingProviderId);
            
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var saleTemp = _saleEFRepository.AppCheckInvetorSaleSource(investorId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.CoreSaleNotFound);

            var contractFiles = new List<AppListCollabContractSignDto>();
            foreach (var contract in contractFileSignatures)
            {
                if (contract.FileSignatureUrl != null)
                {
                    var fileSignatures = new AppListCollabContractSignDto();
                    var template = _collabContractTemplateRepository.FindById(contract.CollabContractTempId, tradingProviderId);
                    if (template != null)
                    {
                        fileSignatures.Title = template.Title;
                        fileSignatures.Id = contract.Id;
                        fileSignatures.TradingProviderId = contract.TradingProviderId;
                        fileSignatures.SaleId = contract.SaleId;
                        fileSignatures.TradingProviderName = contract.TradingProviderName;
                        fileSignatures.FileScanUrl= contract.FileScanUrl;
                        fileSignatures.FileSignatureUrl= contract.FileSignatureUrl;
                        fileSignatures.SaleSource = saleTemp.Source;
                    }
                    contractFiles.Add(fileSignatures);
                }
            }
            return contractFiles;
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
                    ReplaceText = "SALE123456"
                },
                new ReplaceTextDto
                {
                    FindText = "{{TranContent}}",
                    ReplaceText = "EB123456"
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
                        FindText = "{{Signature}}",
                        ReplaceText = $"Đã ký {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}"
                    },
                    new ReplaceTextDto
                    {
                        FindText = "{{SaleNameSignature}}",
                        ReplaceText = "Nguyễn Văn A"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleName}}",
                         ReplaceText = "Nguyễn Văn A"
                    }
                    , new ReplaceTextDto
                    {
                         FindText = "{{SaleAddress}}",
                         ReplaceText = "TP. Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleIdNo}}",
                         ReplaceText = "038099006453"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleIdType}}",
                         ReplaceText = "CCCD"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleIdDate}}",
                         ReplaceText = "13/09/2017"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleIdIssuer}}",
                         ReplaceText = "CA thành phố Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleIdExpiredDate}}",
                         ReplaceText = "13/09/2022"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SalePhone}}",
                         ReplaceText = "03624121786"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleSex}}",
                         ReplaceText = "Nam"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleBirthDate}}",
                         ReplaceText = "29/07/1999"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleResidentAddress}}",
                         ReplaceText = "TP Hà Nội"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleEmail}}",
                         ReplaceText = "anv@gmail.com"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleTaxCode}}",
                         ReplaceText = "12345676"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleBankAccNo}}",
                         ReplaceText = "23219217132141"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleBankAccName}}",
                         ReplaceText = "NGUYEN VAN A"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleBankName}}",
                         ReplaceText = "AGRIBANK"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleBankBranch}}",
                         ReplaceText = "Chi nhánh Hai Bà Trưng"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleContactAddress}}",
                         ReplaceText = "TP Hà Nội"
                    }
                });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderNameUpperCase}}",
                ReplaceText = "CÔNG TY CỔ PHẦN ABC"
            });
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
                ReplaceText = "2423535745"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderPhone}}",
                ReplaceText = "03472647826"
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
                ReplaceText = "TP HÀ Nội"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderPhone}}",
                ReplaceText = "035672834823"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderFax}}",
                ReplaceText = "0325346346"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderRepName}}",
                ReplaceText = "Nguyễn Văn B"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderRepPosition}}",
                ReplaceText = "Chủ tịch"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderDecisionNo}}",
                ReplaceText = "73627563223"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankAccName}}",
                ReplaceText = "NGUYEN VAN B"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankAccNo}}",
                ReplaceText = "37264236756"
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankName}}",
                ReplaceText = "AGRIBANK"
            });

            return replateTexts;
        }

        public async Task<ExportResultDto> ExportContractPdfTest(int tradingProviderId, int contractTemplateId)
        {
            string filePath = _fileConfig.Value.Path;

            var result = new ExportResultDto();
            var contractTemplate = _collabContractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.FileUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.CoreSaleCollabContractNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.FileUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Title} không tồn tại."), new FaultCode(((int)ErrorCode.CoreSaleCollabContractNotFound).ToString()), "");
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
                    catch (XmlException e)
                    {
                        _logger.LogError(e, "file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Title} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
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
            var contractTemplate = _collabContractTemplateRepository.FindById(contractTemplateId, tradingProviderId);
            if (contractTemplate == null || contractTemplate?.FileUrl == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng: {contractTemplateId}"), new FaultCode(((int)ErrorCode.CoreSaleCollabContractNotFound).ToString()), "");
            }
            var fileResult = FileUtils.GetPhysicalPath(contractTemplate.FileUrl, filePath);
            if (!File.Exists(fileResult.FullPath))
            {
                throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Title} không tồn tại."), new FaultCode(((int)ErrorCode.CoreSaleCollabContractNotFound).ToString()), "");
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
                    catch (XmlException e)
                    {
                        _logger.LogError(e, "file word lỗi dạng XML");
                        throw new FaultException(new FaultReason($"File mẫu của hợp đồng {contractTemplate.Title} lỗi. Vui lòng upload lại mẫu hợp đồng khác."), new FaultCode(((int)ErrorCode.FileWordXmlValid).ToString()), "");
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
    }
}
