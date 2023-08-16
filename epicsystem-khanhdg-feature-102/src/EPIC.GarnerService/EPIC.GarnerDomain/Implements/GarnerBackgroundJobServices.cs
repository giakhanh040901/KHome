using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Filter;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.InvestDomain.Implements
{
    public class GarnerBackgroundJobServices
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IGarnerContractTemplateServices _garnerContractTemplateServices;
        private readonly IGarnerContractDataServices _garnerContractDataServices;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerOrderContractFileEFRepository _garnerOrderContractFileEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly GarnerContractTemplateEFRepository _garnerContractTemplateEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly IGarnerContractCodeServices _garnerContractCodeServices;

        public GarnerBackgroundJobServices(
            EpicSchemaDbContext dbContext,
            ILogger<GarnerBackgroundJobServices> logger,
            IHttpContextAccessor httpContext,
            DatabaseOptions databaseOptions,
            IGarnerContractTemplateServices garnerContractTemplateServices,
            IGarnerContractDataServices garnerContractDataServices,
            IGarnerContractCodeServices garnerContractCodeServices)
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContext = httpContext;
            _connectionString = databaseOptions.ConnectionString;
            _garnerContractTemplateServices = garnerContractTemplateServices;
            _garnerContractDataServices = garnerContractDataServices;
            _garnerOrderEFRepository = new GarnerOrderEFRepository(_dbContext, _logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(_dbContext, _logger);
            _garnerOrderContractFileEFRepository = new GarnerOrderContractFileEFRepository(_dbContext, _logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(_dbContext, _logger);
            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, _logger);
            _garnerContractTemplateEFRepository = new GarnerContractTemplateEFRepository(_dbContext, _logger);
            _garnerContractCodeServices = garnerContractCodeServices;
        }

        /// <summary>
        /// khi đặt lệnh bằng App thì sinh ra file chữ ký
        /// </summary>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [Queue(HangfireQueues.Shared)]
        [HangfireLogEverything]
        public async Task CreateContractFileByOrderApp(GarnerOrder order, int tradingProviderId, List<ReplaceTextDto> data)
        {
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId, order.TradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            //Lấy thông tin bán theo kỳ hạn
            var product = _garnerProductEFRepository.FindById(order.ProductId).ThrowIfNull(_dbContext, ErrorCode.GarnerProductNotFound);
            //Lấy ra danh sách hợp đồng
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            //Lấy ra danh sách mẫu hợp đồng khi đặt lệnh
            var contractTemplates = _garnerContractTemplateEFRepository.FindAllForUpdateContractFile(order.PolicyId, contractTemplateType, tradingProviderId, ContractTypes.DAT_LENH, null, order.Source, Status.ACTIVE).ThrowIfNull(_dbContext, ErrorCode.GarnerContractTemplateNotFound);
            if (contractTemplates.Count < 1)
            {
                _logger.LogError($"Không tìm thấy mẫu hợp đồng đặt lệnh dành cho nhà đầu tư là {GetNameINVType(contractTemplateType)}");
            }
            //List<Task> taskHandleFiles = new();
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var contract in contractTemplates)
            {
                SaveFileDto saveFile = null;
                var contractCode = _garnerContractCodeServices.GetContractCode(order, product, policy, contract.ConfigContractId);
                data.AddRange(
                new List<ReplaceTextDto>() {
                        new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                        new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                });
                //Fill hợp đồng và lưu trữ
                saveFile = await _garnerContractDataServices.SaveContract(contractTemplateType, contract.Id, data, tradingProviderId);
                //Lưu đường dẫn vào bảng contract file
                _garnerOrderContractFileEFRepository.Add(new GarnerOrderContractFile
                {
                    ContractTempId = contract.Id,
                    FileTempUrl = saveFile.FileTempUrl,
                    FileSignatureUrl = saveFile.FileSignatureUrl,
                    FileTempPdfUrl = saveFile.FileSignatureUrl,
                    OrderId = order.Id,
                    PageSign = saveFile.PageSign,
                    TradingProviderId = tradingProviderId,
                    ContractCodeGen = contractCode
                }, order.CreatedBy, tradingProviderId);
                _dbContext.SaveChanges();
            }

            //xử lý chuyển đổi file chạy tất cả cùng lúc
            //await Task.WhenAll(taskHandleFiles);
            transaction.Commit();
        }

        /// <summary>
        /// khi rút tiền bằng App thì sinh ra file chữ ký
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [Queue(HangfireQueues.Shared)]
        [HangfireLogEverything]
        public async Task CreateOrUpdateContractFileByWithDrawalApp(long orderId, int tradingProviderId, List<ReplaceTextDto> data, long withDrawalId)
        {
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId, order.TradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            //Lấy thông tin bán theo kỳ hạn
            var product = _garnerProductEFRepository.FindById(order.ProductId).ThrowIfNull(_dbContext, ErrorCode.GarnerProductNotFound);
            //Lấy ra danh sách hợp đồng
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            //Lấy ra danh sách mẫu hợp đồng khi rút vốn
            var contractTemplates = _garnerContractTemplateEFRepository.FindAllForUpdateContractFile(order.PolicyId, contractTemplateType, tradingProviderId, ContractTypes.RUT_TIEN, null, order.Source, Status.ACTIVE).ThrowIfNull(_dbContext, ErrorCode.GarnerContractTemplateNotFound);
            //List<Task> taskHandleFiles = new();
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var contract in contractTemplates)
            {
                SaveFileDto saveFile = null;
                var contractCode = _garnerContractCodeServices.GetContractCode(order, product, policy, contract.ConfigContractId);
                //Lấy thêm data cho hợp đồng rút tiền
                data.AddRange(_garnerContractDataServices.GetDataWithdrawalContractFile(order, withDrawalId, null));
                data.AddRange(
                new List<ReplaceTextDto>(){
                        new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                        new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                });
                //Fill hợp đồng và lưu trữ
                saveFile = await _garnerContractDataServices.SaveContract(contractTemplateType, contract.Id, data, tradingProviderId);
                //taskHandleFiles.Add(saveFile.SaveFileTasks);
                var orderWithdrawalFile = _garnerOrderContractFileEFRepository.FindByContractTemplateAndOrderWithdrawal(orderId, contract.Id, withDrawalId);
                if (orderWithdrawalFile != null)
                {
                    orderWithdrawalFile.FileTempUrl = saveFile.FileTempUrl;
                    orderWithdrawalFile.FileTempPdfUrl = saveFile.FileSignatureUrl;
                    orderWithdrawalFile.FileSignatureUrl = saveFile.FileSignatureUrl;
                    orderWithdrawalFile.PageSign = saveFile.PageSign;
                    orderWithdrawalFile.Deleted = YesNo.NO;
                    _garnerOrderContractFileEFRepository.Update(orderWithdrawalFile, userName, tradingProviderId);

                }
                else
                {
                    //Lưu đường dẫn vào bảng Bond Secondary Contract
                    _garnerOrderContractFileEFRepository.Add(new GarnerOrderContractFile
                    {
                        ContractTempId = contract.Id,
                        FileTempUrl = saveFile.FileTempUrl,
                        FileSignatureUrl = saveFile.FileSignatureUrl,
                        FileTempPdfUrl = saveFile.FileSignatureUrl,
                        OrderId = orderId,
                        PageSign = saveFile.PageSign,
                        TradingProviderId = tradingProviderId,
                        WithdrawalId = withDrawalId,
                        ContractCodeGen = contractCode
                    }, userName, tradingProviderId);
                }
                _dbContext.SaveChanges();
            }

            //xử lý chuyển đổi file chạy tất cả cùng lúc
            //await Task.WhenAll(taskHandleFiles);
            transaction.Commit();
        }
    }
}
