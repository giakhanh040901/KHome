using AutoMapper;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.RocketChat;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerInterestPayment;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.Dto.GarnerShared;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedEntities.Dto;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerFormulaServices : IGarnerFormulaServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly GarnerCalendarEFRepository _garnerCalendarEFRepository;
        private readonly GarnerInterestPaymentEFRepository _garnerInterestPaymentEFRepository;
        private readonly GarnerWithdrawalEFRepository _garnerWithdrawalEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerOrderPaymentEFRepository _garnerOrderPaymentEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerPolicyDetailEFRepository _garnerPolicyDetailEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly GarnerWithdrawalDetailEFRepository _garnerWithdrawalDetailEFRepository;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;

        public GarnerFormulaServices(
            EpicSchemaDbContext dbContext,
            IMapper mapper,
            ILogger<GarnerFormulaServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContextAccessor;
            _garnerCalendarEFRepository = new GarnerCalendarEFRepository(dbContext, logger);
            _garnerInterestPaymentEFRepository = new GarnerInterestPaymentEFRepository(dbContext, logger);
            _garnerWithdrawalEFRepository = new GarnerWithdrawalEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerOrderPaymentEFRepository = new GarnerOrderPaymentEFRepository(dbContext, logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, logger);
            _garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _garnerWithdrawalDetailEFRepository = new GarnerWithdrawalDetailEFRepository(dbContext, logger);
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, logger);
        }

        /// <summary>
        /// dòng tiền của chính sách
        /// </summary>
        public List<AppGarnerPolicyDetailDto> GetCashFlow(int policyId, decimal totalValue, DateTime now)
        {
            now = now.Date;
            var policy = _garnerPolicyEFRepository.FindById(policyId).ThrowIfNull<GarnerPolicy>(_dbContext, ErrorCode.GarnerPolicyNotFound);

            // % thuế lợi nhuận
            decimal taxRate = policy.IncomeTax / 100;

            var listPolicyDetail = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(policyId);
            listPolicyDetail = listPolicyDetail.Where(p => p.IsShowApp == YesNo.YES && p.Status == Status.ACTIVE).OrderBy(r => r.SortOrder).ToList();
            var result = new List<AppGarnerPolicyDetailDto>();
            foreach (var detailItem in listPolicyDetail)
            {
                var resultItem = new AppGarnerPolicyDetailDto();

                var dueDate = _garnerPolicyDetailEFRepository.NextInterestDate(now, detailItem.PeriodQuantity, detailItem.PeriodType);

                //số ngày đầu tư
                int numInvestDays = (dueDate - now).Days;
                if (numInvestDays < 0)
                {
                    _logger.LogError($"Ngày đầu tư đang lớn hơn ngày hiện tại: dueDate = {dueDate}, now = {now}");
                    continue;
                }
                // % lợi tức theo kỳ hạn
                decimal profitRate = detailItem.Profit / 100;

                //Tính lợi nhuận theo kỳ hạn
                var calculateProfit = _garnerInterestPaymentEFRepository.CalculateProfit(policy.CalculateType, totalValue, numInvestDays, profitRate, taxRate, 0);

                resultItem = _mapper.Map<AppGarnerPolicyDetailDto>(detailItem);
                resultItem.PeriodTypeName = DictionaryNames.PeriodUnitNameFind(detailItem.PeriodType);
                resultItem.DueDate = dueDate;
                resultItem.ActuallyProfit = Math.Round(calculateProfit.ActualProfit);
                resultItem.InterestTypeName = InterestTypes.InterestTypeNames(policy.InterestType, policy.RepeatFixedDate);
                result.Add(resultItem);
            }
            return result;
        }

        public CalculateGarnerWithdrawalDto CalculateWithdrawal(long withdrawalId)
        {
            _logger.LogInformation($"{nameof(GarnerFormulaServices)} -> {nameof(CalculateWithdrawal)}: withdrawalId = {withdrawalId}");
            var orderWithdraw = _garnerWithdrawalEFRepository.GetOrderWithdrawal(withdrawalId);
            return CalculateWithdrawal(orderWithdraw);
        }
        public CalculateGarnerWithdrawalDto CalculateWithdrawal(List<GarnerOrderWithdrawalDto> orderWithdrawal, DateTime? now = null)
        {
            _logger.LogInformation($"{nameof(GarnerFormulaServices)} -> {nameof(CalculateWithdrawal)}: orderWithdrawal = {JsonSerializer.Serialize(orderWithdrawal?.Select(o => new { o.Order?.Id, o.AmountMoney }))}");
            CalculateGarnerWithdrawalDto garnerWithdrawal = new()
            {
                GarnerWithdrawalDetails = new()
            };
            DateTime startDate = (now ?? DateTime.Now).Date;
            //xử lý tính tiền rút : lặp tính cho từng lệnh
            foreach (var order in orderWithdrawal)
            {
                GarnerWithdrawalDetailDto withdrawalDetail = CalculateWithdrawalDetail(order, startDate);
                withdrawalDetail.OrderId = order.Order.Id;
                garnerWithdrawal.GarnerWithdrawalDetails.Add(withdrawalDetail);
            }
            //tính tổng
            garnerWithdrawal.Profit = Math.Round(garnerWithdrawal.GarnerWithdrawalDetails.Sum(d => d.Profit));
            garnerWithdrawal.DeductibleProfit = Math.Round(garnerWithdrawal.GarnerWithdrawalDetails.Sum(d => d.DeductibleProfit));
            garnerWithdrawal.ActuallyProfit = Math.Round(garnerWithdrawal.GarnerWithdrawalDetails.Sum(d => d.ActuallyProfit));
            garnerWithdrawal.WithdrawalFee = Math.Round(garnerWithdrawal.GarnerWithdrawalDetails.Sum(d => d.WithdrawalFee));
            garnerWithdrawal.AmountReceived = Math.Round(garnerWithdrawal.GarnerWithdrawalDetails.Sum(d => d.AmountReceived));
            garnerWithdrawal.Tax = Math.Round(garnerWithdrawal.GarnerWithdrawalDetails.Sum(d => d.Tax));
            return garnerWithdrawal;
        }

        public GarnerWithdrawalDetailDto CalculateWithdrawalDetail(long orderId, long withdrawalId)
        {
            _logger.LogInformation($"{nameof(GarnerFormulaServices)} -> {nameof(CalculateWithdrawalDetail)}: orderId = {orderId}, withdrawalId = {withdrawalId}");
            var garnerOrderWithdrawal = _garnerWithdrawalEFRepository.GetOrderWithdrawalDetail(withdrawalId, orderId)
                .ThrowIfNull<GarnerOrderWithdrawalDto>(_dbContext, ErrorCode.GarnerOrderWithdrawalNotFound);
            return CalculateWithdrawalDetail(garnerOrderWithdrawal);
        }

        public GarnerWithdrawalDetailDto CalculateWithdrawalDetail(GarnerOrderWithdrawalDto orderWithdrawal, DateTime? now = null)
        {
            _logger.LogInformation($"{nameof(GarnerFormulaServices)} -> {nameof(CalculateWithdrawalDetail)}: orderWithdrawal = {JsonSerializer.Serialize(orderWithdrawal)}");
            DateTime startDate = (now ?? DateTime.Now).Date;
            if (orderWithdrawal.Order.InvestDate == null)
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerOrderInvestDateIsNull);
            }
            DateTime investDate = orderWithdrawal.Order.InvestDate.Value.Date;

            var policy = _garnerPolicyEFRepository.FindById((int)orderWithdrawal.Order.PolicyId)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);

            //tra % lợi tức
            var policyDetail = _garnerPolicyDetailEFRepository.FindPolicyDetailByDate((int)orderWithdrawal.Order.PolicyId, investDate, startDate);
            if (policyDetail == null)
            {
                policyDetail.Profit = 0;
            }    

            //% lợi nhuận
            decimal profitRate = policyDetail.Profit / 100;

            //% thuế lợi nhuận
            decimal taxRate = policy.IncomeTax / 100;

            //số ngày đầu tư
            int numInvestDays = (startDate - investDate).Days;
            if (numInvestDays < 0)
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerOrderActiveDateGreaterThanNow);
            }

            //lợi tức rút vốn: số tiền rút * số ngày tính từ đầu đến ngày rút * lợi tức tra từ bảng) / 365
            decimal profit = _garnerInterestPaymentEFRepository.FomulaInterest(orderWithdrawal.AmountMoney, numInvestDays, profitRate);

            /**
             * lợi tức khấu trừ: Trước ngày rút có bao nhiêu lợi tức tương ứng với số tiền rút mà đã được trả
             * lấy ngày đã chi trả lợi tức gần nhất tìm ra % lợi tức * số tiền rút * (từ ngày đầu tư đến ngày gần nhất đó) /365
             */
            decimal deductibleProfit = 0;
            DateTime? lastPay = _garnerInterestPaymentEFRepository.FindLastDatePaymentInterest(orderWithdrawal.Order.Id);
            if (lastPay != null)
            {
                var policyDetailLastPay = _garnerPolicyDetailEFRepository.FindPolicyDetailByDate(orderWithdrawal.Order.PolicyId, investDate, lastPay.Value)
                    .ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyDetailByDateNotFound);

                decimal profitRateLastPay = policyDetailLastPay.Profit / 100;
                int numInvestDaysLastPay = (lastPay.Value - orderWithdrawal.Order.InvestDate.Value.Date).Days;
                if (numInvestDaysLastPay < 0)
                {
                    _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerOrderActiveDateGreaterThanLastPay);
                }

                deductibleProfit = _garnerInterestPaymentEFRepository.FomulaInterest(orderWithdrawal.AmountMoney, numInvestDaysLastPay, profitRateLastPay);
            }

            /**
             * thuế: từ ngày trả lợi tức gần nhất đến ngày rút
             * (số tiền rút * số ngày tính từ ngày trả lợi tức gần nhất đến ngày rút * lợi tức tra từ bảng)/365 * thuế xuất
             */
            int numInvestDaysFromLastToEnd = (startDate - (lastPay ?? investDate)).Days; //nếu chưa trả ngày nào thì tính từ đầu đến giờ
            if (numInvestDaysFromLastToEnd < 0)
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerOrderActiveDateGreaterThanNow);
            }

            decimal tax = 0;
            var cifCodeFind = _cifCodeEFRepository.FindByCifCode(orderWithdrawal.Order.CifCode)
                .ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            if (cifCodeFind.InvestorId == null) //khách doanh nghiệp thì taxRate về 0
            {
                taxRate = 0;
            }

            decimal actuallyProfit = 0;
            if (policy.CalculateType == CalculateTypes.GROSS)
            {
                //lợi tức thực nhận: Lợi tức rút vốn - lợi tức khấu trừ - thuế
                tax = _garnerInterestPaymentEFRepository.FomulaInterest(orderWithdrawal.AmountMoney, numInvestDaysFromLastToEnd, profitRate) * taxRate;
                actuallyProfit = profit - deductibleProfit - tax;
            }
            else if (policy.CalculateType == CalculateTypes.NET)
            {
                actuallyProfit = profit - deductibleProfit;
                //tính lại thuế
                tax = actuallyProfit / (1 - taxRate) * taxRate;
            }

            //phí rút
            decimal withdrawalFee = 0;
            //trường hợp theo kỳ hạn mới tính
            //1: theo số tiền
            // % * số tiền rút

            //2: theo năm
            // (% * số tiền rút * (ngày đáo hạn - ngày rút)) / 365

            //số tiền thực nhận
            decimal amountReceived = orderWithdrawal.AmountMoney + actuallyProfit - withdrawalFee;

            GarnerWithdrawalDetailDto result = new()
            {
                ProfitRate = profitRate,
                Profit = Math.Round(profit),
                DeductibleProfit = Math.Round(deductibleProfit),
                Tax = Math.Round(tax),
                ActuallyProfit = Math.Round(actuallyProfit),
                WithdrawalFee = Math.Round(withdrawalFee),
                AmountReceived = Math.Round(amountReceived),
                AmountMoney = Math.Round(orderWithdrawal.AmountMoney)
            };
            return result;
        }

        /// <summary>
        /// Tính lợi nhuận hiện tại của hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public CalculateProfitDto CaculateProfitNow(long orderId)
        {
            var order = _garnerOrderEFRepository.FindById(orderId);
            if (order == null)
            {
                _logger.LogError($"{nameof(CaculateProfitNow)} Không tìm thấy thông tin hợp đồng: orderId = {orderId} ");
                return new CalculateProfitDto();
            }  
            
            if (order.Status != OrderStatus.DANG_DAU_TU)
            {
                _logger.LogError($"{nameof(CaculateProfitNow)} Hợp đồng không trong trạng thái hoạt động: orderId = {orderId} ");
                return new CalculateProfitDto();
            }   
            
            if (order.InvestDate == null)
            {
                _logger.LogError($"{nameof(CaculateProfitNow)} Không tìm thấy ngày hợp đồng invest: orderId = {orderId} ");
                return new CalculateProfitDto();
            }    

            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId);
            if (policy == null)
            {
                _logger.LogError($"{nameof(CaculateProfitNow)} Không tìm thấy thông tin chính sách: policyId = {order.PolicyId} ");
                return new CalculateProfitDto();
            }

            DateTime now = DateTime.Now.Date;

            var result = new CalculateProfitDto();
            // % thuế lợi nhuận
            decimal taxRate = policy.IncomeTax / 100;
            int numInvestDays = (now - order.InvestDate.Value.Date).Days;

            var policyDetail = _garnerPolicyDetailEFRepository.FindPolicyDetailByDate(order.PolicyId, order.InvestDate.Value.Date, now);
            if (policyDetail != null)
            {
                //% lợi nhuận
                decimal profitRate = policyDetail.Profit / 100;

                //số tiền đã trả theo lệnh
                decimal profitPaid = _garnerInterestPaymentEFRepository.ProfitPaid(order.Id);

                var profitResult = _garnerInterestPaymentEFRepository
                    .CalculateProfit(policy.CalculateType, order.TotalValue, numInvestDays, profitRate, taxRate, profitPaid);

                result.Profit = Math.Round(profitResult.Profit);
                result.ActualProfit = Math.Round(profitResult.ActualProfit);
                result.Tax = Math.Round(profitResult.Tax);
                result.InvestmentDays = numInvestDays;
            }
            return result;
        }

        /// <summary>
        ///  Lấy danh sách lịch sử dòng tiền của Nhà đầu tư
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<TradingRecentlyDto> GarnerTradingRecently(int investorId)
        {
            _logger.LogInformation($"{nameof(GarnerFormulaServices)} -> {nameof(GarnerTradingRecently)}: investorId = {investorId}");
            var result = new List<TradingRecentlyDto>();
            var cifCode = _cifCodeEFRepository.FindByInvestor(investorId).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            result = (from orderPayment in _dbContext.GarnerOrderPayments
                                  join order in _dbContext.GarnerOrders on orderPayment.OrderId equals order.Id
                                  where orderPayment.Deleted == YesNo.NO && order.Deleted == YesNo.NO
                                  && order.CifCode == cifCode.CifCode && orderPayment.TranClassify != TranClassifies.RUT_VON
                                  select new TradingRecentlyDto
                                  {
                                        ContractCode = order.ContractCode,
                                        PaymentAmnount = orderPayment.PaymentAmount,
                                        Status= orderPayment.Status,
                                        TranClassify= orderPayment.TranClassify,
                                        TranDate = orderPayment.TranDate,
                                        TranType = orderPayment.TranType,
                                        GeneralProductType = GeneralProductTypes.GARNER
                                  }).ToList();

            var tradingRecentlyWithdrawal = _dbContext.GarnerWithdrawals.Where(w => w.CifCode == cifCode.CifCode && w.Deleted == YesNo.NO);
            foreach (var item in tradingRecentlyWithdrawal)
            {
                // Đưa về các trạng thái chung: 1: Khởi tạo/Yêu cầu/Chờ phản hồi, 2: Đã duyệt(Đi tiền/Không đi tiền), 3 Hủy duyệt
                if (item.Status == WithdrawalStatus.CHO_PHAN_HOI)
                {
                    item.Status = WithdrawalStatus.YEU_CAU;
                }
                else if (item.Status == WithdrawalStatus.DUYET_DI_TIEN || item.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
                {
                    item.Status = WithdrawalStatus.DUYET_DI_TIEN;
                }
                var policy = _garnerPolicyEFRepository.FindById(item.PolicyId);
                if (policy != null)
                {
                    result.Add(new TradingRecentlyDto
                    {
                        PolicyName = policy.Name,
                        PaymentAmnount = item.AmountMoney,
                        Status = item.Status,
                        TranClassify = TranClassifies.RUT_VON,
                        TranDate = item.CreatedDate,
                        TranType = TranTypes.CHI,
                        GeneralProductType = GeneralProductTypes.GARNER
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách ngân hàng đặt lệnh theo cifCode
        /// </summary>
        /// <param name="cifCode"></param>
        /// <returns></returns>
        public List<BankAccountInfoDto> FindListBankOfCifCode(string cifCode)
        {
            _logger.LogInformation($"{nameof(GarnerFormulaServices)} -> {nameof(GarnerTradingRecently)}: cifCode = {cifCode}");
            List<BankAccountInfoDto> result = new();
            var cifCodeFind = _cifCodeEFRepository.FindByCifCode(cifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            var ordersByCifCode = _garnerOrderEFRepository.Entity.Where(o => o.CifCode == cifCode && o.Status == OrderStatus.DANG_DAU_TU && o.Deleted == YesNo.NO).ToList();
            foreach (var order in ordersByCifCode)
            {
                if (cifCodeFind.InvestorId != null)
                {
                    var investorBankAcc = _investorBankAccountEFRepository
                                 .GetBankById(order.InvestorBankAccId ?? 0, cifCodeFind.InvestorId);
                    if (investorBankAcc != null)
                    {
                        result.Add(investorBankAcc);
                    }    
                }  
                else if (cifCodeFind.BusinessCustomerId != null)
                {
                    var businessBankAcc = _businessCustomerBankEFRepository.GetBankById(order.BusinessCustomerBankAccId ?? 0, cifCodeFind.BusinessCustomerId);
                    if (businessBankAcc != null)
                    {
                        result.Add(businessBankAcc);
                    }
                }
            }
            result = result.GroupBy(b => b.Id).Select(y => y.First()).ToList();
            return result;
        }

        /// <summary>
        /// Xem trước dòng tiền lợi nhuận chi trả
        /// </summary>
        public List<GarnerOrderCashFlowExpectedDto> GetCashFlowOrder(long orderId)
        {
            _logger.LogInformation($"{nameof(GetCashFlowOrder)}: orderId = {orderId}");
            List<GarnerOrderCashFlowExpectedDto> result = new();
            DateTime investDateCalculator = new();
            var orderQuery = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);

            var policyFind = _garnerPolicyEFRepository.FindById(orderQuery.PolicyId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            var investDateMin = _garnerOrderEFRepository.EntityNoTracking
               .Where(o => o.Deleted == YesNo.NO && o.CifCode == orderQuery.CifCode && o.Status == OrderStatus.DANG_DAU_TU && o.PolicyId == policyFind.Id && o.InvestDate != null).Min(o => o.InvestDate);

            investDateCalculator = investDateMin != null ? investDateMin.Value : orderQuery.BuyDate;

            // Danh sách ngày chi trả
            List<DateTime> listInterestDates = _garnerPolicyDetailEFRepository.GetListPayDateByPolicy(policyFind, investDateCalculator, investDateCalculator.AddYears(1));

            int periodIndex = 0;
            // Lặp theo các ngày chi trả
            foreach (var item in listInterestDates)
            {
                //Kiểm tra ngày nghỉ lễ
                var interestDateItem = _garnerCalendarEFRepository.NextWorkDay(item, policyFind.TradingProviderId);

                DateTime investDateOrder = orderQuery.InvestDate == null ? orderQuery.BuyDate : orderQuery.InvestDate.Value;

                // % thuế lợi nhuận
                decimal taxRate = policyFind.IncomeTax / 100;

                // Tra bậc 
                var policyDetail = _garnerPolicyDetailEFRepository.FindPolicyDetailByDate(orderQuery.PolicyId, investDateOrder, interestDateItem);

                //% lợi nhuận
                decimal profitRate = policyDetail.Profit / 100;

                //số tiền đã trả theo lệnh 
                decimal profitPaid = _garnerInterestPaymentEFRepository.ProfitPaid(orderQuery.Id, interestDateItem);

                //số ngày đầu tư (Ngày tính lợi nhuận - ngày đầu tư của lệnh)
                int numInvestDays = (interestDateItem - investDateOrder).Days;
                if (numInvestDays < 0)
                {
                    continue;
                }
                // Số tiền đã trã được lập và số tiền chưa được lập tại thời điểm trước đó
                profitPaid = profitPaid + result.Where(p => p.PayDate < interestDateItem).Sum(r => r.AmountReceived);

                // Tính lợi nhuận
                var calculateProfit = _garnerInterestPaymentEFRepository.CalculateProfit(policyFind.CalculateType, orderQuery.TotalValue, numInvestDays, profitRate, taxRate, profitPaid);
                periodIndex = periodIndex + 1;
                result.Add(new GarnerOrderCashFlowExpectedDto
                {
                    PeriodIndexName = (policyFind.GarnerType == GarnerPolicyTypes.KHONG_CHON_KY_HAN && policyFind.InterestType == InterestTypes.CUOI_KY) ? "Tiền nhận cuối kỳ" : $"Lợi tức kỳ {periodIndex}",
                    TotalValue = orderQuery.TotalValue,
                    InitTotalValue = orderQuery.InitTotalValue,
                    ProfitRate = profitRate,
                    NumberOfDays = numInvestDays,
                    ExistingAmount = orderQuery.TotalValue,
                    Profit = Math.Round(calculateProfit.Profit),
                    Tax = Math.Round(calculateProfit.Tax),
                    AmountReceived = Math.Round(calculateProfit.ActualProfit),
                    InvestDays = numInvestDays,
                    PayDate = interestDateItem,
                });
            }
            return result;
        }
    }
}
