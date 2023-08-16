using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.FileEntities.Settings;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestRepositories;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Filter;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using static EPIC.Utils.DataUtils.ContractDataUtils;
using EPIC.InvestEntities.Dto.ContractTemplate;

namespace EPIC.InvestDomain.Implements
{
    public class InvestBackgroundJobService
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly EpicSchemaDbContext _dbContext;

        private readonly IContractTemplateServices _contractTemplateServices;
        private readonly IContractDataServices _contractDataServices;
        private readonly IHttpContextAccessor _httpContext;
        private readonly InvestOrderRepository _orderRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;
        private readonly IInvestContractCodeServices _investContractCodeServices;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestOrderContractFileEFRepository _investOrderContractFileEFRepository;

        public InvestBackgroundJobService(
            EpicSchemaDbContext dbContext,
            ILogger<InvestBackgroundJobService> logger,
            DatabaseOptions databaseOptions,
            IContractTemplateServices contractTemplateServices,
            IContractDataServices contractDataServices,
            IInvestContractCodeServices investContractCodeServices,
             IOptions<FileConfig> fileConfig,
            IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _httpContext = httpContext;
            _connectionString = databaseOptions.ConnectionString;
            _contractTemplateServices = contractTemplateServices;
            _contractDataServices = contractDataServices;
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _investContractCodeServices = investContractCodeServices;
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _fileConfig = fileConfig;
            _investOrderContractFileEFRepository = new InvestOrderContractFileEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// chạy background khi đặt lệnh thì sinh ra các file word, pdf, file đã ký, file đã ký có con dấu
        /// </summary>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [Queue(HangfireQueues.Shared)]
        [HangfireLogEverything]
        public async Task UpdateContractFile(InvOrder order, int tradingProviderId, List<ReplaceTextDto> data, string userName)
        {
            string filePath = _fileConfig.Value.Path;
            try
            {
                var policy = _policyRepository.FindPolicyById(order.PolicyId, tradingProviderId);
                if (policy == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
                }

                //Lấy ra danh sách mẫu hợp đồng kèm theo id hợp đồng
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

                List<string> filePathOldRemove = new();
                foreach (var contract in contractTemplate)
                {
                    SaveFileDto saveFile = new();
                    var contractCode = contract.ContractCode ?? _investContractCodeServices.GetContractCode(order, policy, contract.ConfigContractId);
                    data.AddRange(
                    new List<ReplaceTextDto>() {
                        new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                        new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                    });
                    //Fill hợp đồng và lưu trữ
                    saveFile = await _contractDataServices.SaveContractApp(order, contract.Id, order.PolicyDetailId, data);
                    var orderContractFile = _dbContext.InvestOrderContractFile.FirstOrDefault(e => e.ContractTempId == contract.Id && e.OrderId == order.Id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId);
                    if (orderContractFile != null)
                    {
                        if (orderContractFile == null)
                        {
                            throw new FaultException(new FaultReason($"Không tìm thấy hợp đồng: {contract.OrderContractFileId}"), new FaultCode(((int)ErrorCode.InvestOrderContractFileNotFound).ToString()), "");
                        }
                        //xóa file hợp đồng cũ
                        FileUtils.RemoveOrderContractFile(new OrderContractFileRemoveDto
                        {
                            FileScanUrl = orderContractFile.FileScanUrl,
                            FileTempPdfUrl = orderContractFile.FileTempPdfUrl,
                            FileSignatureUrl = orderContractFile.FileSignatureUrl,
                            FileTempUrl = orderContractFile.FileTempUrl,
                        }, filePath);

                        //Lưu đường dẫn
                        orderContractFile.FileSignatureUrl = saveFile?.FileSignatureUrl ?? orderContractFile.FileSignatureUrl;
                        orderContractFile.FileTempPdfUrl = saveFile?.FileSignatureUrl ?? orderContractFile.FileTempPdfUrl;
                        orderContractFile.FileTempUrl = saveFile?.FileTempUrl ?? orderContractFile.FileTempUrl;
                        orderContractFile.PageSign = saveFile?.PageSign ?? orderContractFile.PageSign;
                        orderContractFile.ModifiedBy = userName;
                        orderContractFile.ContractCodeGen = contractCode;
                        orderContractFile.FileSignatureStampUrl = saveFile?.FileSignatureStampUrl ?? orderContractFile.FileSignatureStampUrl;
                    }
                    else
                    {
                        //Không sinh mới file tái tục, rút tiền khi đặt lệnh
                        if (contract.ContractType == ContractTypes.RUT_TIEN || contract.ContractType == ContractTypes.TAI_TUC_GOC
                            || contract.ContractType == ContractTypes.TAI_TUC_GOC_VA_LOI_NHUAN)
                        {
                            FileUtils.RemoveOrderContractFile(new OrderContractFileRemoveDto
                            {
                                FileSignatureUrl = saveFile.FileSignatureUrl,
                                FileTempUrl = saveFile.FileTempUrl,
                            }, filePath);
                            continue;
                        };
                        //Lưu đường dẫn vào bảng order contract file
                        var orderContractFileInsert = new OrderContractFile
                        {
                            Id = (int)_investOrderContractFileEFRepository.NextKey(),
                            ContractTempId = contract.Id,
                            FileTempUrl = saveFile?.FileTempUrl,
                            FileSignatureUrl = saveFile?.FileSignatureUrl,
                            FileTempPdfUrl = saveFile?.FileSignatureUrl,
                            OrderId = (int)order.Id,
                            TradingProviderId = tradingProviderId,
                            PageSign = saveFile?.PageSign ?? 1,
                            ContractCodeGen = contractCode,
                            CreatedBy = userName,
                            FileSignatureStampUrl = saveFile?.FileSignatureStampUrl,
                        };
                        _dbContext.InvestOrderContractFile.Add(orderContractFileInsert);
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Lỗi trong quá trình sinh file (chi tiết: {ex.Message})");
                //Xóa file nếu trong quá trình sinh file bị lỗi
                var orderContractFile = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO);
                foreach (var item in orderContractFile)
                {
                    if (item.FileTempUrl != null && File.Exists(item.FileTempUrl)) 
                    {
                        File.Delete(item.FileTempUrl);
                    }

                    if (item.FileTempPdfUrl != null && File.Exists(item.FileTempPdfUrl))
                    {
                        File.Delete(item.FileTempPdfUrl);
                    }

                    if (item.FileSignatureUrl != null && File.Exists(item.FileSignatureUrl))
                    {
                        File.Delete(item.FileSignatureUrl);
                    }
                    _dbContext.InvestOrderContractFile.Remove(item);
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thông báo hợp đồng tới hạn tất toán
        /// </summary>
        public void NotifyOrderCloseToDueDate(int tradingProviderId)
        {
            //lấy danh sách hợp đồng sắp tới hạn (thêm hằng số = 3 ngày) tính due date có next work day
            //gọi api push notification
        }

        /// <summary>
        /// chạy background thay đổi show App
        /// </summary>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [Queue(HangfireQueues.Invest)]
        [HangfireLogEverything]
        public void InvestPolicyShowApp(int policyId, string isShowApp)
        {
            var policy = _dbContext.InvestPolicies.FirstOrDefault(e => e.Id == policyId && e.Deleted == YesNo.NO && e.Status == Status.ACTIVE).ThrowIfNull(_dbContext, ErrorCode.InvestPolicyNotFound);
             policy.IsShowApp = isShowApp;
            _dbContext.SaveChanges();
        }
    }
}
