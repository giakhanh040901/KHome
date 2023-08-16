using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.FileEntities.Settings;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.GarnerEntities.Dto.GarnerOrderContractFile;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerOrderContractFileServices : IGarnerOrderContractFileServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerOrderContractFileServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerContractTemplateEFRepository _garnerContractTemplateEFRepository;
        private readonly GarnerOrderContractFileEFRepository _garnerOrderContractFileEFRepository;
        private readonly GarnerHistoryUpdateEFRepository _garnerHistoryUpdateEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly IGarnerContractTemplateServices _garnerContractTemplateServices;
        private readonly IGarnerContractDataServices _garnerContractDataServices;
        private readonly IGarnerContractCodeServices _garnerContractCodeServices;

        public GarnerOrderContractFileServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerOrderContractFileServices> logger,
            IOptions<FileConfig> fileConfig,
            IHttpContextAccessor httpContextAccessor,
            IGarnerContractTemplateServices garnerContractTemplateServices,
            IGarnerContractDataServices garnerContractDataServices,
            IGarnerContractCodeServices garnerContractCodeServices
        )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerContractTemplateEFRepository = new GarnerContractTemplateEFRepository(dbContext, logger);
            _garnerOrderContractFileEFRepository = new GarnerOrderContractFileEFRepository(dbContext, logger);
            _garnerHistoryUpdateEFRepository = new GarnerHistoryUpdateEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _garnerContractTemplateServices = garnerContractTemplateServices;
            _garnerContractDataServices = garnerContractDataServices;
            _garnerContractCodeServices = garnerContractCodeServices;
            _fileConfig = fileConfig;
        }

        public void UpdateContractFileSignPdf(long orderId)
        {
            _logger.LogInformation($"{nameof(UpdateContractFileSignPdf)}: orderId = {orderId}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            //Lấy ra danh sách hợp đồng
            var contractTemplates = _garnerContractTemplateServices.FindAllByOrder(order.Id, order.TradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerContractTemplateNotFound);
            contractTemplates = contractTemplates
                .Where(c => c.DisplayType != ContractTemplateDisplayType.AFTER && c.ContractType == ContractTypes.DAT_LENH)
                .ToList();
            if (contractTemplates == null)
            {
                return;
            }

            foreach (var contract in contractTemplates)
            {
                if (contract.IsSign == IsSignPdf.No)
                {
                    var saveFileApp = _garnerContractDataServices.SignContractPdf(contract.OrderContractFileId ?? 0, contract.Id, order.TradingProviderId);
                    if (contract.OrderContractFileId != null)
                    {
                        var orderContract = _garnerOrderContractFileEFRepository.FindById(contract.OrderContractFileId ?? 0, order.TradingProviderId);
                        if (orderContract.FileSignatureUrl != null)
                        {
                            string filePath = _fileConfig.Value.Path;
                            var fileResult = FileUtils.GetPhysicalPathNoCheckExists(orderContract.FileSignatureUrl, filePath);
                            var fileSignPathOld = fileResult.FullPath.Replace(".pdf", "-Sign.pdf");
                            if (File.Exists(fileSignPathOld)) //xóa file pdf có ảnh con dấu
                            {
                                File.Delete(fileSignPathOld);
                            }
                        }

                        //Lưu đường dẫn file vừa ký 
                        orderContract.FileSignatureUrl = saveFileApp.FileSignatureUrl;
                        orderContract.IsSign = IsSignPdf.Yes;
                        orderContract.FileSignatureStampUrl = null;
                        _garnerOrderContractFileEFRepository.Update(orderContract, username, order.TradingProviderId);
                    }
                }
            }
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                RealTableId = orderId,
                Action = ActionTypes.CAP_NHAT,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CONTRACT_FILE_SIGN_PDF,
            }, username);
            _dbContext.SaveChanges();
        }

        public async Task UpdateContractFileUpdateSource(long orderId)
        {
            string filePath = _fileConfig.Value.Path;
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var orderContractFiles = _garnerOrderContractFileEFRepository.FindAll(orderId, tradingProviderId);
            //Xóa các file hợp đồng offline của order
            foreach (var contracFile in orderContractFiles)
            {
                FileUtils.RemoveOrderContractFile(new OrderContractFileRemoveDto
                {
                    FileScanUrl = contracFile.FileScanUrl,
                    FileTempPdfUrl = contracFile.FileTempPdfUrl,
                    FileSignatureUrl = contracFile.FileSignatureUrl,
                    FileTempUrl = contracFile.FileTempUrl,
                }, filePath);
            }

            //Xóa tất cả hợp dồng offline của lệnh
            _garnerOrderContractFileEFRepository.DeleteContractFileByOrderId(orderId);
            //sinh lại hợp đồng
            await UpdateContractFile(orderId);
        }

        /// <summary>
        /// Update hợp đồng, fill data hợp đồng (sinh lại file hợp đồng)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task UpdateContractFile(long orderId)
        {
            _logger.LogInformation($"{nameof(UpdateContractFile)}: orderId = {orderId}");
            string filePath = _fileConfig.Value.Path;
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var product = _garnerProductEFRepository.FindById(order.ProductId).ThrowIfNull(_dbContext, ErrorCode.GarnerProductNotFound);
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            var contractTemplates = _garnerContractTemplateEFRepository.FindAllForUpdateContractFile(order.PolicyId, contractTemplateType, tradingProviderId, null, null, order.Source, Status.ACTIVE).ThrowIfNull(_dbContext, ErrorCode.GarnerContractTemplateNotFound);
            if (contractTemplates.Count < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng dành cho nhà đầu tư là {GetNameINVType(contractTemplateType)}"), new FaultCode(((int)ErrorCode.GarnerContractTemplateNotFound).ToString()), "");
            }
            foreach (var contractTemplate in contractTemplates)
            {
                //lấy data để fill hợp đồng
                var data = _garnerContractDataServices.GetDataContractFile(order, tradingProviderId, true, contractTemplate.ConfigContractId);
                //Lấy ra danh sách hợp đồng theo order
                var contractFiles = _garnerOrderContractFileEFRepository.FindByContractTemplateAndOrder(order.Id, contractTemplate.Id, tradingProviderId);
                var saveFileApp = new SaveFileDto();
                if (contractFiles.Count > 0)
                {
                    //update các hợp đồng đã có (đặt lệnh và rút tiền)
                    foreach (var contract in contractFiles)
                    {
                        if (contract.IsSign == IsSignPdf.No || contract.IsSign == null)
                        {
                            if (contract.WithdrawalId != null)
                            {
                                //Lấy thêm data cho hợp đồng rút tiền
                                data.AddRange(_garnerContractDataServices.GetDataWithdrawalContractFile(order, contract.WithdrawalId, null));
                            }
                            //Fill hợp đồng và lưu trữ
                            saveFileApp = await _garnerContractDataServices.SaveContract(contractTemplateType, contract.ContractTempId, data, tradingProviderId);
                            FileUtils.RemoveOrderContractFile(new OrderContractFileRemoveDto
                            {
                                FileScanUrl = contract.FileScanUrl,
                                FileTempPdfUrl = contract.FileTempPdfUrl,
                                FileSignatureUrl = contract.FileSignatureUrl,
                                FileTempUrl = contract.FileTempUrl,
                            }, filePath);
                            //Lưu đường dẫn vào bảng Disrtibution Contract  
                            contract.FileTempUrl = saveFileApp.FileTempUrl;
                            contract.FileSignatureUrl = saveFileApp?.FileSignatureUrl ?? contract.FileSignatureUrl;
                            contract.FileTempPdfUrl = saveFileApp?.FileSignatureUrl ?? contract.FileTempPdfUrl;
                            contract.PageSign = saveFileApp?.PageSign ?? contract.PageSign;
                            contract.Deleted = YesNo.NO;
                            _garnerOrderContractFileEFRepository.Update(contract, userName, tradingProviderId);
                        }
                    }
                }
                else
                {
                    //Không sinh mới file đối với mẫu rút tiền, tái tục khi cập nhật hợp đồng
                    if (contractTemplate.ContractType == ContractTypes.RUT_TIEN || contractTemplate.ContractType == ContractTypes.TAI_TUC_GOC) { continue; }
                    //trường hợp sinh mới file hợp đồng đặt lệnh từ mẫu hợp đồng
                    //Fill hợp đồng và lưu trữ
                    saveFileApp = await _garnerContractDataServices.SaveContract(contractTemplateType, contractTemplate.Id, data, tradingProviderId);
                    var contractCode = _garnerContractCodeServices.GetContractCode(order, product, policy, contractTemplate.ConfigContractId);
                    //Lưu đường dẫn vào bảng order contract file
                    var orderContractFile = new GarnerOrderContractFile
                    {
                        ContractTempId = contractTemplate.Id,
                        FileTempUrl = saveFileApp?.FileTempUrl,
                        FileSignatureUrl = saveFileApp?.FileSignatureUrl,
                        FileTempPdfUrl = saveFileApp?.FileSignatureUrl,
                        OrderId = order.Id,
                        TradingProviderId = tradingProviderId,
                        PageSign = saveFileApp?.PageSign ?? 1,
                        ContractCodeGen = contractCode
                    };
                    _garnerOrderContractFileEFRepository.Add(orderContractFile, userName, tradingProviderId);
                }
            }

            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                RealTableId = orderId,
                Action = ActionTypes.CAP_NHAT,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CONTRACT_FILE,
            }, userName);

            _dbContext.SaveChanges();

        }

        #region Hợp đồng
        /// <summary>
        /// sinh hợp đồng khi đặt lệnh
        /// </summary>
        /// <returns></returns>
        public async Task CreateContractFileByOrderAdd(GarnerOrder order, List<ReplaceTextDto> data)
        {
            _logger.LogInformation($"{nameof(CreateContractFileByOrderAdd)}: order = {JsonSerializer.Serialize(order)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId, order.TradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            //Lấy thông tin bán theo kỳ hạn
            var product = _garnerProductEFRepository.FindById(order.ProductId).ThrowIfNull(_dbContext, ErrorCode.GarnerProductNotFound);
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            var businesscustomer = _businessCustomerEFRepository.FindById(cifCode.BusinessCustomerId ?? 0);
            if (cifCode.InvestorId != null)
            {
                var investorId = _investorEFRepository.GetIdentificationById(order.InvestorIdenId ?? 0);
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            //Lấy ra danh sách mẫu hợp đồng khi đặt lệnh
            var contractTemplates = from ct in _dbContext.GarnerContractTemplates
                                    join ctt in _dbContext.GarnerContractTemplateTemps
                                    on ct.ContractTemplateTempId equals ctt.Id
                                    where (ct.PolicyId == order.PolicyId && ct.TradingProviderId == tradingProviderId && ct.Deleted == YesNo.NO && ct.Status == Status.ACTIVE
                                    && (ctt.ContractType == ContractTypes.DAT_LENH)
                                    && (ctt.ContractSource == order.Source || ctt.ContractSource == ContractSources.ALL))
                                    select new GarnerContractTemplateForOrderDto
                                    {
                                        PolicyId = ct.PolicyId,
                                        ContractSource = ctt.ContractSource,
                                        TradingProviderId = ct.TradingProviderId,
                                        ContractTemplateTempId = ctt.Id,
                                        ContractTemplateUrl = contractTemplateType == SharedContractTemplateType.INVESTOR ? ctt.FileInvestor : ctt.FileBusinessCustomer,
                                        ContractType = ctt.ContractType,
                                        DisplayType = ct.DisplayType,
                                        Name = ctt.Name,
                                        FileBusinessCustomer = ctt.FileBusinessCustomer,
                                        FileInvestor = ctt.FileInvestor,
                                        ConfigContractId = ct.ConfigContractId,
                                        StartDate = ct.StartDate,
                                        Id = ct.Id,
                                        Status = ct.Status,
                                        CreatedDate = ct.CreatedDate
                                    };
            if (!contractTemplates.Any())
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng đặt lệnh dành cho nhà đầu tư là {GetNameINVType(contractTemplateType)}"), new FaultCode(((int)ErrorCode.GarnerContractTemplateNotFound).ToString()), "");
            }
            foreach (var contract in contractTemplates)
            {
                var saveFileApp = new SaveFileDto();
                var contractCode = _garnerContractCodeServices.GetContractCode(order, product, policy, contract.ConfigContractId);
                data.AddRange(new List<ReplaceTextDto>(){
                    new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                    new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                });
                //Fill hợp đồng và lưu trữ
                saveFileApp = await _garnerContractDataServices.SaveContract(contractTemplateType, contract.Id, data, tradingProviderId);
                
                //Lưu đường dẫn vào bảng Order Contract File
                var orderContractFile = new GarnerOrderContractFile
                {
                    Id = (long)_garnerOrderContractFileEFRepository.NextKey(),
                    ContractTempId = contract.Id,
                    FileTempUrl = saveFileApp.FileTempUrl,
                    FileSignatureUrl = saveFileApp?.FileSignatureUrl,
                    FileTempPdfUrl = saveFileApp?.FileSignatureUrl,
                    OrderId = order.Id,
                    TradingProviderId = tradingProviderId,
                    PageSign = saveFileApp?.PageSign ?? 1,
                    ContractCodeGen = contractCode,
                    FileSignatureStampUrl = saveFileApp?.FileSignatureStampUrl
                };
                _dbContext.GarnerOrderContractFiles.Add(orderContractFile);
                _dbContext.SaveChanges();
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// sinh hợp đồng khi đặt lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task CreateContractFileByOrderAdd(long orderId, List<ReplaceTextDto> data)
        {
            _logger.LogInformation($"{nameof(CreateContractFileByOrderAdd)}: orderId = {orderId}");
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            await CreateContractFileByOrderAdd(order, data);
        }

        /// <summary>
        /// sinh hợp đồng khi rút tiền
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="withDrawalId"></param>
        /// <param name="data">data hợp đồng</param>
        /// <returns></returns>
        public async Task CreateContractFileByWithdrawal(long orderId, int? withDrawalId, List<ReplaceTextDto> data)
        {
            _logger.LogInformation($"{nameof(CreateContractFileByWithdrawal)}: orderId = {orderId}, withDrawalId = {withDrawalId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId, order.TradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            //Lấy thông tin bán theo kỳ hạn
            var product = _garnerProductEFRepository.FindById(order.ProductId).ThrowIfNull(_dbContext, ErrorCode.GarnerProductNotFound);
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            var businesscustomer = _businessCustomerEFRepository.FindById(cifCode.BusinessCustomerId ?? 0);
            if (cifCode.InvestorId != null)
            {
                var investorId = _investorEFRepository.GetIdentificationById(order.InvestorIdenId ?? 0);
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            //Lấy ra danh sách mẫu hợp đồng rút tiền
            var contractTemplates = from ct in _dbContext.GarnerContractTemplates
                                    join ctt in _dbContext.GarnerContractTemplateTemps
                                    on ct.ContractTemplateTempId equals ctt.Id
                                    where (ct.PolicyId == order.PolicyId && ct.TradingProviderId == tradingProviderId && ct.Deleted == YesNo.NO && ct.Status == Status.ACTIVE
                                    && (ctt.ContractType == ContractTypes.RUT_TIEN)
                                    && (ctt.ContractSource == order.Source || ctt.ContractSource == ContractSources.ALL))
                                    select new GarnerContractTemplateForOrderDto
                                    {
                                        PolicyId = ct.PolicyId,
                                        ContractSource = ctt.ContractSource,
                                        TradingProviderId = ct.TradingProviderId,
                                        ContractTemplateTempId = ctt.Id,
                                        ContractTemplateUrl = contractTemplateType == SharedContractTemplateType.INVESTOR ? ctt.FileInvestor : ctt.FileBusinessCustomer,
                                        ContractType = ctt.ContractType,
                                        DisplayType = ct.DisplayType,
                                        Name = ctt.Name,
                                        FileBusinessCustomer = ctt.FileBusinessCustomer,
                                        FileInvestor = ctt.FileInvestor,
                                        ConfigContractId = ct.ConfigContractId,
                                        StartDate = ct.StartDate,
                                        Id = ct.Id,
                                        Status = ct.Status,
                                        CreatedDate = ct.CreatedDate
                                    };
            if (contractTemplates.Any())
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng rút tiền dành cho nhà đầu tư là {GetNameINVType(contractTemplateType)}"), new FaultCode(((int)ErrorCode.GarnerContractTemplateNotFound).ToString()), "");
            }
            foreach (var contract in contractTemplates)
            {
                //Lấy thêm data cho hợp đồng rút tiền
                data.AddRange(_garnerContractDataServices.GetDataWithdrawalContractFile(order, withDrawalId, null));
                var contractCode = _garnerContractCodeServices.GetContractCode(order, product, policy, contract.ConfigContractId);
                data.AddRange(
                    new List<ReplaceTextDto>(){
                        new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                        new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                });
                //Fill hợp đồng và lưu trữ
                var saveFileApp = await _garnerContractDataServices.SaveContract(contractTemplateType, contract.Id, data, tradingProviderId);
                //Lưu đường dẫn vào bảng BondOrder contractFile
                var orderContractFile = new GarnerOrderContractFile
                {
                    ContractTempId = contract.Id,
                    FileTempUrl = saveFileApp?.FileTempUrl,
                    FileSignatureUrl = saveFileApp?.FileSignatureUrl,
                    FileTempPdfUrl = saveFileApp?.FileSignatureUrl,
                    OrderId = orderId,
                    TradingProviderId = tradingProviderId,
                    PageSign = saveFileApp?.PageSign ?? 1,
                    WithdrawalId = withDrawalId,
                    ContractCodeGen = contractCode
                };
                _garnerOrderContractFileEFRepository.Add(orderContractFile, userName, tradingProviderId);
            }
            _dbContext.SaveChanges();
        }

        public void UpdateFileScanContract(UpdateOrderContractFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderContract = _garnerOrderContractFileEFRepository.FindById(input.Id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderContractFileNotFound);
            orderContract.FileScanUrl = input.FileScanUrl ?? orderContract.FileScanUrl;
            _garnerOrderContractFileEFRepository.Update(orderContract, username, tradingProviderId);
            _dbContext.SaveChanges();
        }
        #endregion

    }
}