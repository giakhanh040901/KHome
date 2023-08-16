using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Department;
using EPIC.FileEntities.Settings;
using EPIC.IdentityRepositories;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestRepositories;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.Utils;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.DataEntities;
using DocumentFormat.OpenXml.Drawing.Charts;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestSharedEntites.Dto.Order;
using EPIC.MSB.Services;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.InvestSharedDomain.Interfaces;

namespace EPIC.InvestDomain.Implements
{
    public class InvestOrderShareServices : IInvestOrderShareServices
    {
        private readonly ILogger<InvestOrderShareServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly InvestOrderRepository _orderRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly InvestOrderPaymentRepository _orderPaymentRepository;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly InvestRepositories.CalendarRepository _calendarRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly IMapper _mapper;
        private readonly InvestInterestPaymentRepository _interestPaymentRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly AssetManagerRepository _assetManagerRepository;
        private readonly MsbCollectMoneyServices _msbCollectMoneyServices;
        private readonly SaleRepository _saleRepository;
        private readonly TradingMSBPrefixAccountEFRepository _tradingMSBPrefixAccountEFRepository;
        private readonly IInvestContractCodeServices _investContractCodeServices;
        private readonly InvestOrderEFRepository _investOrderEFRepository;

        public InvestOrderShareServices(
            ILogger<InvestOrderShareServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            EpicSchemaDbContext dbContext,
            IInvestSharedServices investSharedServices,
            MsbCollectMoneyServices msbCollectMoneyServices,
            IInvestContractCodeServices investContractCodeServices)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _dbContext = dbContext;
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _calendarRepository = new InvestRepositories.CalendarRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _orderPaymentRepository = new InvestOrderPaymentRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _interestPaymentRepository = new InvestInterestPaymentRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _assetManagerRepository = new AssetManagerRepository(_connectionString, _logger);
            _investOrderEFRepository = new InvestOrderEFRepository(_dbContext, _logger);
            _msbCollectMoneyServices = msbCollectMoneyServices;
            _investSharedServices = investSharedServices;
            _investContractCodeServices = investContractCodeServices;
            _mapper = mapper;
            _tradingMSBPrefixAccountEFRepository = new TradingMSBPrefixAccountEFRepository(dbContext, _logger);
        }

        /// <summary>
        /// Lấy thông tin sổ lệnh bằng số tài khoản chứng khoán
        /// </summary>
        /// <param name="securityCompany"></param>
        /// <param name="stockTradingAccount"></param>
        /// <returns></returns>
        public List<SCInvestOrderDto> GetListInvestOrderByInvestor(int securityCompany, string stockTradingAccount, DateTime? startDate, DateTime? endDate)
        {
            List<SCInvestOrderDto> result = new();
            var listOrder = _orderRepository.GetListInvestOrderByInvestor(securityCompany, stockTradingAccount, startDate, endDate);
            foreach (var orderItem in listOrder)
            {
                var resultItem = new SCInvestOrderDto();

                var project = _projectRepository.FindById(orderItem.ProjectId, null);
                if (project == null)
                {
                    _logger.LogError($"Không tìm thấy thông tin dự án trên app với orderId = {orderItem.Id}, projectId = {orderItem.ProjectId}");
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin dự án"), new FaultCode(((int)ErrorCode.InvestProjectNotFound).ToString()), "");
                }

                var policyFind = _policyRepository.FindPolicyById(orderItem.PolicyId, orderItem.TradingProviderId);
                if (policyFind == null)
                {
                    _logger.LogError($"Không tìm thấy thông tin chính sách trên app với orderId = {orderItem.Id}, policyId = {orderItem.PolicyId}");
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
                }

                var distribution = _distributionRepository.FindById(policyFind.DistributionId, policyFind.TradingProviderId);
                if (distribution == null)
                {
                    _logger.LogError($"Không tìm thấy thông tin bán theo kỳ hạn trên app với orderId = {orderItem.Id}, distributionId = {policyFind.DistributionId}");
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.InvestDistributionNotFound).ToString()), "");
                }

                var policyDetailFind = _policyRepository.FindPolicyDetailById(orderItem.PolicyDetailId, orderItem.TradingProviderId);
                if (policyDetailFind == null)
                {
                    _logger.LogError($"Không tìm thấy thông tin kỳ hạn trên app với orderId = {orderItem.Id}, policyDetailId = {orderItem.PolicyDetailId}");
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
                }

                resultItem = _mapper.Map<SCInvestOrderDto>(orderItem);
                resultItem.Profit = policyDetailFind.Profit;
                resultItem.DueDate = orderItem.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, orderItem.InvestDate.Value, distribution.CloseCellDate);
                var cashFlow = _investSharedServices.GetCashFlowContract(orderItem.InitTotalValue, orderItem.InvestDate.Value, policyDetailFind, policyFind, distribution, project, (int)orderItem.Id, true);
                if (cashFlow == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin dòng tiền"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                resultItem.AllActuallyProfit = cashFlow.ActuallyProfit;
                resultItem.InvestorBankAccount = _mapper.Map<SCInvestorBankAccountDto>(_managerInvestorRepository.GetBankById(orderItem.InvestorBankAccId ?? 0));
                result.Add(resultItem);
            }
            return result;
        }

        public async Task<AppInvestOrderInvestorDetailDto> AppSaleViewOrder(int orderId)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);

            var findOrder = _orderRepository.AppSaleViewOrder(saleId, orderId);
            if (findOrder == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin hợp đồng với Saler"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }

            if (findOrder.InvestorId == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin nhà đầu tư của hợp đồng"), new FaultCode(((int)ErrorCode.InvestorNotFound).ToString()), "");
            }

            return await ViewOrderDetail(orderId, findOrder.InvestorId ?? 0);
        }

        public async Task<AppInvestOrderInvestorDetailDto> ViewOrderDetail(int orderId, int investorId)
        {
            #region Lấy data từ các Repo
            var orderFind = _orderRepository.AppGetOrderDetail(investorId, orderId);
            if (orderFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }

            var project = _projectRepository.FindById(orderFind.ProjectId ?? 0, null);
            if (project == null)
            {
                _logger.LogError($"Không tìm thấy thông tin dự án trên app với orderId = {orderId}, investorId = {investorId}, projectId = {orderFind.ProjectId}");
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin dự án"), new FaultCode(((int)ErrorCode.InvestProjectNotFound).ToString()), "");
            }

            var policyFind = _policyRepository.FindPolicyById(orderFind.PolicyId ?? 0, orderFind.TradingProviderId ?? 0);
            if (policyFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin chính sách trên app với orderId = {orderId}, investorId = {investorId}, policyId = {orderFind.PolicyId}");
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }

            var distribution = _distributionRepository.FindById(policyFind.DistributionId, policyFind.TradingProviderId);
            if (distribution == null)
            {
                _logger.LogError($"Không tìm thấy thông tin bán theo kỳ hạn trên app với orderId = {orderId}, investorId = {investorId}, distributionId = {policyFind.DistributionId}");
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.InvestDistributionNotFound).ToString()), "");
            }

            var policyDetailFind = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId ?? 0, orderFind.TradingProviderId ?? 0);
            if (policyDetailFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin kỳ hạn trên app với orderId = {orderId}, investorId = {investorId}, policyDetailId = {orderFind.PolicyDetailId}");
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }

            var identificationFind = _investorIdentificationRepository.FindById(orderFind.InvestorIdenId ?? 0);
            if (identificationFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin giấy tờ nhà đầu tư trên app với orderId = {orderId}, investorId = {investorId}, investorIdenId = {orderFind.InvestorIdenId}");
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin giấy tờ nhà đầu tư"), new FaultCode(((int)ErrorCode.InvestorIdentificationNotFound).ToString()), "");
            }

            var investorBank = _managerInvestorRepository.GetBankById(orderFind.InvestorBankAccId ?? 0);
            if (investorBank == null)
            {
                _logger.LogError($"Không tìm thấy thông tin thụ hưởng trên app với orderId = {orderId}, investorId = {investorId}, investorBankAccId = {orderFind.InvestorBankAccId}");
                //throw new FaultException(new FaultReason($"Không tìm thấy thông tin thụ hưởng"), new FaultCode(((int)ErrorCode.InvestorBankAccNotFound).ToString()), "");
            }
            var investorContractAddress = _investorRepository.GetContactAddress(investorId, orderFind.ContractAddressId ?? 0);
            if (investorContractAddress == null)
            {
                _logger.LogError($"Không tìm thấy thông tin địa chỉ giao dịch trong chi tiết order trên app với orderId = {orderId}, investorId = {investorId}, contractAddressId = {orderFind.ContractAddressId}");
                //throw new FaultException(new FaultReason($"Không tìm thấy thông tin địa chỉ giao dịch"), new FaultCode(((int)ErrorCode.InvestorContractAddessNotFound).ToString()), "");
            }

            var paymentInfo = _businessCustomerRepository.FindBusinessCusBankById(orderFind.BusinessCustomerBankAccId ?? 0);
            if (paymentInfo == null)
            {
                _logger.LogError($"Không tìm thấy thông tin tài khoản thụ hưởng dlsc trong chi tiết order trên app với orderId = {orderId}, investorId = {investorId}, BusinessCustomerBankAccId = {orderFind.BusinessCustomerBankAccId}");
                //throw new FaultException(new FaultReason($"Không tìm thấy thông tin ngân hàng thụ hưởng của đại lý sơ cấp"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            #endregion

            var result = _mapper.Map<AppInvestOrderInvestorDetailDto>(orderFind);

            if (orderFind.SaleReferralCodeSub != null)
            {
                result.SaleReferralCode = orderFind.SaleReferralCodeSub;
            }

            var salerFind = _saleRepository.SaleGetInfoByReferralCode(result.SaleReferralCode);
            if (salerFind != null)
            {
                result.SalerName = salerFind.Fullname;
            }


            result.DistributionId = orderFind.DistributionId;
            // Danh sách ngân hàng thụ hưởng của đại lý được cấu hình trong phân phối sản phẩm
            var listBankFind = await GetTradingBankAccountOfDistribution(orderId, distribution.Id);
            if (listBankFind != null)
            {
                result.TradingBankAccounts = listBankFind;
            }

            result.InvCode = project.InvCode;
            result.IconProject = project.Image;
            result.PolicyName = policyFind.Name;
            result.PolicyType = policyFind.Type;
            result.PeriodQuantity = policyDetailFind.PeriodQuantity;
            result.PeriodType = policyDetailFind.PeriodType;
            result.Profit = policyDetailFind.Profit;
            result.InterestPeriodType = policyDetailFind.InterestPeriodType;
            result.InterestType = policyDetailFind.InterestType;
            result.MinWithdrawal = policyFind.MinWithDraw;
            string interestPeriodType = null;
            if (policyDetailFind.InterestPeriodType == PeriodType.NGAY)
            {
                interestPeriodType = "Ngày";
            }
            else if (policyDetailFind.InterestPeriodType == PeriodType.THANG)
            {
                interestPeriodType = "Tháng";
            }
            else if (policyDetailFind.InterestPeriodType == PeriodType.QUY)
            {
                interestPeriodType = "Quý";
            }
            else if (policyDetailFind.InterestPeriodType == PeriodType.NAM)
            {
                interestPeriodType = "Năm";
            }
            result.InterestPeriod = policyDetailFind.InterestPeriodQuantity + " " + interestPeriodType;

            DateTime ngayDauTu = DateTime.Now.Date;
            if (orderFind.BuyDate != null && orderFind.InvestDate == null && orderFind.PaymentFullDate == null)
            {
                ngayDauTu = orderFind.BuyDate.Value.Date;
            }
            if (orderFind.InvestDate != null)
            {
                ngayDauTu = orderFind.InvestDate.Value.Date;
            }
            result.InvestDate = ngayDauTu;
            result.DueDate = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, ngayDauTu, distribution.CloseCellDate);

            var profit = _investSharedServices.CalculateListInterest(project, policyFind, policyDetailFind, ngayDauTu, result.TotalValue, true, distribution.CloseCellDate, orderId, true);
            if (profit == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin trái tức"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            result.ProfitNow = _investSharedServices.ProfitNow(ngayDauTu, result.DueDate.Value, policyDetailFind.Profit ?? 0, orderFind.TotalValue ?? 0);
            result.ContractAddress = investorContractAddress?.ContactAddress;
            result.IdNo = identificationFind?.IdNo;
            result.IdType = identificationFind?.IdType;
            result.BankName = investorBank?.BankName;
            result.BankAccount = investorBank?.BankAccount;
            result.OwnerAccount = investorBank?.OwnerAccount;

            if (identificationFind != null)
            {
                result.FullName = identificationFind.Fullname;
            }    
            var cashFlow = _investSharedServices.GetCashFlowContract(result.InitTotalValue, ngayDauTu, policyDetailFind, policyFind, distribution, project, orderId);
            if (cashFlow == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin dòng tiền"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            result.AllActuallyProfit = cashFlow.ActuallyProfit;
            result.AllProfit = cashFlow.ActuallyProfit;
            result.TotalIncome = result.TotalValue + result.AllActuallyProfit;
            result.AppCashFlow = new();
            result.AppCashFlow = _mapper.Map<AppCashFlowDto>(cashFlow);
            result.PaymentInfo = _mapper.Map<Entities.Dto.Order.AppPaymentInfoDto>(paymentInfo ?? new());
            result.PaymentInfo.PaymentNote = orderFind.PaymentNote;

            if (orderFind.Status == OrderStatus.DANG_DAU_TU && orderFind.PaymentFullDate == null && orderFind.InvestDate == null)
            {
                result.PaymentFullDate = null;
                result.DueDate = null;
                result.AppCashFlow = new();
                result.ProfitNow = null;
                result.AllActuallyProfit = null;
                result.TotalIncome = null;
            }
            //Mã hợp đồng
            var order = _investOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);
            result.ContractCode = _investContractCodeServices.GenOrderContractCodeDefault(new GenInvestContractCodeDto()
            {
                Order = order,
                Policy = policyFind,
            });
            //giao dịch
            result.TransactionList = new();
            result.TransactionList = TransactionListOrder(orderId).OrderByDescending(o => o.TranDate).ToList();
            return result;
        }

        /// <summary>
        /// Lịch sử giao dịch của bondInvest
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<AppInvTransactionListDto> TransactionListOrder(int orderId)
        {
            var result = new List<AppInvTransactionListDto>();

            var tradingRecently = _assetManagerRepository.TradingRecentlyByOrder(orderId);
            result = _mapper.Map<List<AppInvTransactionListDto>>(tradingRecently.Where(r => r.Type == TranClassifies.THANH_TOAN).ToList());
            var resultWithdrawal = tradingRecently.Where(r => r.Type == TranClassifies.RUT_VON).OrderBy(r => r.TranDate);
            int indexWithdrawal = 1;
            foreach (var item in resultWithdrawal)
            {
                var resultItem = new AppInvTransactionListDto();
                resultItem = _mapper.Map<AppInvTransactionListDto>(item);
                resultItem.Description = "Rút vốn lần thứ  " + indexWithdrawal++;
                result.Add(resultItem);
            }
            return result;
        }

        /// <summary>
        /// Thông tin ngân hàng của phân phối
        /// </summary>
        /// <param name="input"></param>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        public async Task<List<AppTradingBankAccountDto>> TradingBankAccountOfDistribution(AppOrderDto input, int distributionId)
        {
            var result = new List<AppTradingBankAccountDto>();
            var listTradingBankAccount = _distributionRepository.GetListBankCollectByDistributionId(distributionId);
            listTradingBankAccount = listTradingBankAccount.GroupBy(b => b.BusinessCustomerBankAccId).Select(b => b.First()).ToList();
            foreach (var bankAccountItem in listTradingBankAccount)
            {
                var resultBankItem = new AppTradingBankAccountDto();
                resultBankItem = bankAccountItem;
                var prefixAcc = _tradingMSBPrefixAccountEFRepository.FindByTradingBankId(bankAccountItem.BusinessCustomerBankAccId);
                if (prefixAcc != null)
                {
                    // Sinh QrCode nếu không sinh được lấy như bình thường
                    try
                    {
                        var findMoney = await _msbCollectMoneyServices.RequestCollectMoney(new MSB.Dto.CollectMoney.RequestCollectMoneyDto
                        {
                            OrderCode = $"{ContractCodes.INVEST}{input.Id}",
                            AmountMoney = input.TotalValue ?? 0,
                            OwnerAccount = bankAccountItem.BankAccName,
                            PrefixAccount = prefixAcc.PrefixMsb,
                            Note = input.PaymentNote,
                            MId = prefixAcc.MId,
                            TId = prefixAcc.TId,
                        });
                        if (findMoney != null)
                        {
                            resultBankItem.BankAccNo = findMoney.AccountNumber;
                            resultBankItem.QrCode = findMoney.QrCode;
                        }
                    }
                    catch
                    {
                    }
                }
                result.Add(resultBankItem);
            }
            return result;
        }

        /// <summary>
        /// Lấy thông tin thanh toán theo hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public async Task<List<AppTradingBankAccountDto>> GetTradingBankAccountOfDistribution(int orderId, int distributionId)
        {
            var result = new List<AppTradingBankAccountDto>();
   
            var orderFind = _orderRepository.FindById(orderId);
            if (orderFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }
            result = await TradingBankAccountOfDistribution(new AppOrderDto
            {
                Id = orderId,
                DistributionId = distributionId,
                TotalValue = orderFind.TotalValue,
                PaymentNote = PaymentNotes.THANH_TOAN + orderFind.ContractCode,
                ContractCode = orderFind.ContractCode
            }, distributionId);
            return result;
        }
    }
}
