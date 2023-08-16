using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using EPIC.FileEntities.Settings;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ContractTemplate;
using EPIC.InvestRepositories;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.EnumType;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.InvestDomain.Implements
{
    public class InvestOrderContractFileServices : IInvestOrderContractFileServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<InvestOrderContractFileServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IContractTemplateServices _contractTemplateServices;
        private readonly IContractDataServices _contractDataServices;
        private readonly IInvestContractCodeServices _investContractCodeServices;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly InvestOrderRepository _orderRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly InvestOrderContractFileEFRepository _investOrderContractFileEFRepository;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly InvestHistoryUpdateEFRepository _investHistoryUpdateEFRepository;

        public InvestOrderContractFileServices(
             EpicSchemaDbContext dbContext,
            ILogger<InvestOrderContractFileServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IOptions<FileConfig> fileConfig,
            IContractTemplateServices contractTemplateServices,
            IContractDataServices contractDataServices,
            IBackgroundJobClient backgroundJobs,
            IInvestContractCodeServices investContractCodeServices
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _fileConfig = fileConfig;
            _backgroundJobs = backgroundJobs;
            _contractTemplateServices = contractTemplateServices;
            _contractDataServices = contractDataServices;
            _investContractCodeServices = investContractCodeServices;
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, _logger);
            _investOrderEFRepository = new InvestOrderEFRepository(_dbContext, _logger);
            _investOrderContractFileEFRepository = new InvestOrderContractFileEFRepository(_dbContext, _logger);
            _investHistoryUpdateEFRepository = new InvestHistoryUpdateEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// Ký điện tử
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="contractType"> nếu null thì ký hết</param>
        public void UpdateContractFileSignPdf(long orderId, int? contractType = null)
        {
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateContractFileSignPdf)}: orderId: {orderId}, contractType: {contractType}");
            var order = _orderRepository.FindById(orderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);

            // Cập nhật lịch sử ký điện tử
            _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate((int)orderId, null, null, null
                , InvestHistoryUpdateTables.INV_ORDER, ActionTypes.CAP_NHAT, "Ký điện tử", DateTime.Now), userName);

            _dbContext.SaveChanges();

            var investPolicy = _policyRepository.FindPolicyById(order.PolicyId, order.TradingProviderId);
            if (investPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }
            //Lấy ra danh sách hợp đồng
            var contractTemplate = (from ct in _dbContext.InvestContractTemplates
                                    join ctt in _dbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id into contracTempTemps
                                    from contractTemp in contracTempTemps.DefaultIfEmpty()
                                    join ocf in _dbContext.InvestOrderContractFile on ct.Id equals ocf.ContractTempId into orderContractFiles
                                    from orderContractFile in orderContractFiles.DefaultIfEmpty()
                                    where (ct.PolicyId == investPolicy.Id && ct.TradingProviderId == order.TradingProviderId && ct.Deleted == YesNo.NO && ct.Status == Utils.Status.ACTIVE
                                    && (contractType == null || ((ct.ContractType == null && contractTemp.ContractType == contractType) || (ct.ContractType != null && ct.ContractType == contractType)))
                                    && ct.DisplayType != ContractTemplateDisplayType.AFTER
                                    && ((contractTemp.ContractSource == order.Source || contractTemp.ContractSource == ContractSources.ALL) || (ct.ContractSource == order.Source || ct.ContractSource == ContractSources.ALL)))
                                    select new ViewContractTemplateByOrder
                                    {
                                        ContractCode = orderContractFile.ContractCode,
                                        Id = ct.Id,
                                        ConfigContractId = ct.ConfigContractId,
                                        ContractType = ct.ContractType ?? contractTemp.ContractType,
                                    }).Distinct();

            if (contractTemplate == null)
            {
                return;
            }
            var removeContractList = new List<string>();
            foreach (var contract in contractTemplate)
            {
                var orderContract = _dbContext.InvestOrderContractFile.FirstOrDefault(e => e.ContractTempId == contract.Id && e.OrderId == order.Id && e.Deleted == YesNo.NO && e.TradingProviderId == order.TradingProviderId);
                //bỏ qua những file đã ký điện tử hoặc không có bản ghi order contract file
                if (orderContract == null || orderContract.IsSign == IsSignPdf.Yes)
                {
                    continue;
                }

                var saveFileApp = _contractDataServices.SignContractPdf(orderContract.Id, contract.Id, order.TradingProviderId);
                if (orderContract != null)
                {
                    if (orderContract.FileSignatureUrl != null)
                    {
                        string filePath = _fileConfig.Value.Path;
                        var fileResult = FileUtils.GetPhysicalPathNoCheckExists(orderContract.FileSignatureUrl, filePath);
                        var fileSignPathOld = fileResult.FullPath.Replace(ContractFileExtensions.PDF, ContractFileExtensions.SIGN_PDF);
                        if (File.Exists(fileSignPathOld)) //xóa file pdf có ảnh con dấu
                        {
                            removeContractList.Add(fileSignPathOld);
                        }
                    }
                    //Lưu đường dẫn vào bảng order contract file 
                    orderContract.FileSignatureUrl = saveFileApp.FileSignatureUrl;
                    orderContract.IsSign = IsSignPdf.Yes;
                    orderContract.ModifiedBy = userName;
                    orderContract.FileSignatureStampUrl = null;
                    _dbContext.SaveChanges();
                }
            }

            foreach (var removeContract in removeContractList)
            {
                File.Delete(removeContract);
            }
        }

        #region sinh hợp đồng tái tục
        /// <summary>
        /// Sinh hợp đồng tái tục
        /// </summary>
        public async Task CreateContractFileRenewals(InvOrder order, long renewalsId, int renewalsTimes, List<ReplaceTextDto> replaceTexts, int settlementMethod)
        {
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policy = _policyRepository.FindPolicyById(order.PolicyId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyNotFound, order.PolicyId);

            // Tìm kiếm CifCode
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound, order.CifCode);
            // Lấy mẫu loại hợp đồng
            string contractTemplateType = (cifCode.InvestorId != null) ? SharedContractTemplateType.INVESTOR : SharedContractTemplateType.BUSINESS_CUSTOMER;

            var contactTemplate = from ct in _dbContext.InvestContractTemplates
                                  join ctt in _dbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id into contracTempTemps
                                  from contractTemp in contracTempTemps.DefaultIfEmpty()
                                  where ct.PolicyId == policy.Id && ct.TradingProviderId == tradingProviderId
                                  && (ct.ContractTemplateTempId == null || contractTemp.Deleted == YesNo.NO)
                                  && ct.Deleted == YesNo.NO
                                  && ct.Status == Status.ACTIVE
                                  && (ct.ContractTemplateTempId == null || contractTemp.Status == Status.ACTIVE)
                                  && (contractTemp.ContractSource == order.Source || contractTemp.ContractSource == ContractSources.ALL || ct.ContractSource == order.Source || ct.ContractSource == ContractSources.ALL)
                                  && ((settlementMethod == SettlementMethod.NHAN_LOI_NHUAN_VA_TAI_TUC_GOC
                                       && ((ct.ContractType == null && contractTemp.ContractType == ContractTypes.TAI_TUC_GOC)
                                           || (ct.ContractType != null && ct.ContractType == ContractTypes.TAI_TUC_GOC)))
                                   || (settlementMethod == SettlementMethod.TAI_TUC_GOC_VA_LOI_NHUAN
                                       && ((ct.ContractType == null && contractTemp.ContractType == ContractTypes.TAI_TUC_GOC_VA_LOI_NHUAN)
                                           || (ct.ContractType != null && ct.ContractType == ContractTypes.TAI_TUC_GOC_VA_LOI_NHUAN))))
                                  select ct;

            foreach (var contract in contactTemplate)
            {
                var saveFileApp = new SaveFileDto();
                string contractCode = null;
                if (policy.RenewalsType == InvestRenewalsType.GIU_HOP_DONG_CU)
                {
                    var orderRenewalContractFile = _dbContext.InvestOrderContractFile.FirstOrDefault(o => o.OrderId == order.RenewalsReferId && o.ContractCodeGen != null && o.Deleted == YesNo.NO);
                    contractCode = orderRenewalContractFile?.ContractCodeGen;
                }
                else if (policy.RenewalsType == InvestRenewalsType.TAO_HOP_DONG_MOI)
                {
                    contractCode = _investContractCodeServices.GetContractCode(order, policy, contract.ConfigContractId);
                }

                replaceTexts.AddRange(
                new List<ReplaceTextDto>() {
                    new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                    new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                });
                //Fill hợp đồng và lưu trữ
                saveFileApp = await _contractDataServices.SaveContractApp(order, contract.Id, order.PolicyDetailId, replaceTexts);
                //Lưu đường dẫn vào bảng order contract file
                var orderContractFile = new OrderContractFile
                {
                    ContractTempId = contract.Id,
                    FileTempUrl = saveFileApp?.FileTempUrl,
                    FileSignatureUrl = saveFileApp?.FileSignatureUrl,
                    FileTempPdfUrl = saveFileApp?.FileSignatureUrl,
                    OrderId = order.Id,
                    TradingProviderId = tradingProviderId,
                    PageSign = saveFileApp?.PageSign ?? 1,
                    RenewalsId = renewalsId,
                    Times = renewalsTimes + 1,
                    ContractCodeGen = contractCode,
                    ContractCode = order.ContractCode,
                    IsSign = YesNo.NO,
                    CreatedBy = userName,
                    FileSignatureStampUrl = saveFileApp?.FileSignatureStampUrl,
                };

                _investOrderContractFileEFRepository.Add(orderContractFile);
            }
        }

        #endregion

        #region sinh mới, cập nhật hợp đồng
        public async Task UpdateContractFile(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var order = _investOrderEFRepository.FindById(orderId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);
            if (order.BackgroundJobId != null)
            {
                IStorageConnection connection = JobStorage.Current.GetConnection();
                JobData jobData = connection.GetJobData(order.BackgroundJobId);
                if (jobData != null && jobData.State == EnumBackgroundJobState.Processing)
                {
                    throw new FaultException(new FaultReason($"Đang sinh hợp đồng, vui lòng chờ trong giây lát"), new FaultCode(((int)ErrorCode.InvestOrderContractFileNotFound).ToString()), "");
                }
            }
            // Cập nhật lịch sử Cập nhật hồ sơ
            _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate((int)order.Id, null, null, null, InvestHistoryUpdateTables.INV_ORDER,
                ActionTypes.CAP_NHAT, "Cập nhật hồ sơ", DateTime.Now), username);
            _dbContext.SaveChanges();
            var data = _contractDataServices.GetDataContractFile(order, tradingProviderId, true);
            await UpdateContractFile(order, data);
        }

        /// <summary>
        /// Cập nhật lại hợp đồng
        /// </summary>
        /// <param name="order"></param>
        /// <param name="replaceTexts">data hợp đồng</param>
        /// <param name="contractType">1: Hợp đồng đặt lệnh, 2: Hợp đồng rút tiền, 3: hợp đồng tái tục</param>
        /// <returns></returns>
        public async Task UpdateContractFile(InvOrder order, List<ReplaceTextDto> replaceTexts, int? contractType = null)
        {
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            string filePath = _fileConfig.Value.Path;
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policy = _policyRepository.FindPolicyById(order.PolicyId, tradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            //Lấy ra danh sách hợp đồng
            var contractTemplate = (from ct in _dbContext.InvestContractTemplates
                                    join ctt in _dbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id into contracTempTemps
                                    from contractTemp in contracTempTemps.DefaultIfEmpty()
                                    join ocf in _dbContext.InvestOrderContractFile on ct.Id equals ocf.ContractTempId into orderContractFiles
                                    from orderContractFile in orderContractFiles.DefaultIfEmpty()
                                    where (ct.PolicyId == policy.Id && ct.TradingProviderId == tradingProviderId && ct.Deleted == YesNo.NO && ct.Status == Utils.Status.ACTIVE
                                    && ((ct.ContractType == null && contractTemp.ContractType == ContractTypes.DAT_LENH) || (ct.ContractType != null && ct.ContractType == ContractTypes.DAT_LENH))
                                    && ((contractTemp.ContractSource == order.Source || contractTemp.ContractSource == ContractSources.ALL) || (ct.ContractSource == order.Source || ct.ContractSource == ContractSources.ALL)))
                                    select new ViewContractTemplateByOrder
                                    {
                                        ContractCode = orderContractFile.ContractCode,
                                        Id = ct.Id,
                                        ConfigContractId = ct.ConfigContractId,
                                        ContractType = ct.ContractType ?? contractTemp.ContractType
                                    }).Distinct();

            if (!contractTemplate.Any())
            {
                throw new FaultException(new FaultReason($"Không tìm thấy danh sách hợp đồng mẫu: orderId = {order.Id}, tradingProviderId = {tradingProviderId}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }

            //Xử lý xóa file trong db nếu file mẫu hợp đồng không được kích hoạt
            var orderContractFilesRemove = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO);
            foreach (var item in orderContractFilesRemove)
            {
                if (!contractTemplate.Any(ct => ct.Id == item.ContractTempId))
                {
                    _dbContext.InvestOrderContractFile.Remove(item);
                    //xóa file hợp đồng cũ
                    FileUtils.RemoveOrderContractFile(new OrderContractFileRemoveDto
                    {
                        FileScanUrl = item.FileScanUrl,
                        FileTempPdfUrl = item.FileTempPdfUrl,
                        FileSignatureUrl = item.FileSignatureUrl,
                        FileTempUrl = item.FileTempUrl,
                    }, filePath);
                    _dbContext.SaveChanges();
                }
            }

            foreach (var contract in contractTemplate)
            {
                var saveFileApp = new SaveFileDto();
                if (contract.IsSign == IsSignPdf.No || contract.IsSign == null)
                {
                    //Khi cập nhật lấy mã hợp đồng trong bảng orderContractFile, nếu null thì lấy theo cấu trúc hợp đồng
                    var contractCode = contract.ContractCode ?? _investContractCodeServices.GetContractCode(order, policy, contract.ConfigContractId);
                    replaceTexts.AddRange(
                    new List<ReplaceTextDto>() {
                        new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                        new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                    });
                    //Fill hợp đồng và lưu trữ
                    saveFileApp = await _contractDataServices.SaveContractApp(order, contract.Id, order.PolicyDetailId, replaceTexts);
                    var orderContract = _dbContext.InvestOrderContractFile.FirstOrDefault(e => e.ContractTempId == contract.Id && e.OrderId == order.Id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId);
                    if (orderContract != null)
                    {
                        //xóa file hợp đồng cũ
                        FileUtils.RemoveOrderContractFile(new OrderContractFileRemoveDto
                        {
                            FileScanUrl = orderContract.FileScanUrl,
                            FileTempPdfUrl = orderContract.FileTempPdfUrl,
                            FileSignatureUrl = orderContract.FileSignatureUrl,
                            FileTempUrl = orderContract.FileTempUrl,
                        }, filePath);

                        //Lưu đường dẫn vào order contract file  
                        orderContract.FileTempUrl = saveFileApp?.FileTempUrl ?? orderContract.FileTempUrl;
                        orderContract.FileSignatureUrl = saveFileApp?.FileSignatureUrl ?? orderContract.FileSignatureUrl;
                        orderContract.FileTempPdfUrl = saveFileApp?.FileSignatureUrl ?? orderContract.FileTempPdfUrl;
                        orderContract.PageSign = saveFileApp?.PageSign ?? orderContract.PageSign;
                        orderContract.ModifiedBy = userName;
                        orderContract.ContractCodeGen = contractCode;
                        orderContract.FileSignatureStampUrl = saveFileApp?.FileSignatureStampUrl ?? orderContract.FileSignatureStampUrl;
                        orderContract.IsSign = YesNo.NO;
                    }
                    else
                    {
                        //Không sinh mới file tái tục, rút tiền khi đặt lệnh
                        if (contract.ContractType == ContractTypes.RUT_TIEN || contract.ContractType == ContractTypes.TAI_TUC_GOC
                            || contract.ContractType == ContractTypes.TAI_TUC_GOC_VA_LOI_NHUAN)
                        {
                            FileUtils.RemoveOrderContractFile(new OrderContractFileRemoveDto
                            {
                                FileSignatureUrl = saveFileApp.FileSignatureUrl,
                                FileTempUrl = saveFileApp.FileTempUrl,
                            }, filePath);
                            continue;
                        }
                        //Lưu đường dẫn vào bảng order contract file
                        var orderContractFile = new OrderContractFile
                        {
                            Id = (int)_investOrderContractFileEFRepository.NextKey(),
                            ContractTempId = contract.Id,
                            FileTempUrl = saveFileApp?.FileTempUrl,
                            FileSignatureUrl = saveFileApp?.FileSignatureUrl,
                            FileTempPdfUrl = saveFileApp?.FileSignatureUrl,
                            OrderId = (int)order.Id,
                            TradingProviderId = tradingProviderId,
                            PageSign = saveFileApp?.PageSign ?? 1,
                            ContractCodeGen = contractCode,
                            CreatedBy = userName,
                            FileSignatureStampUrl = saveFileApp?.FileSignatureStampUrl,
                        };
                        _dbContext.InvestOrderContractFile.Add(orderContractFile);

                    }
                }
            }
            _dbContext.SaveChanges();
        }
        #endregion

    }
}
