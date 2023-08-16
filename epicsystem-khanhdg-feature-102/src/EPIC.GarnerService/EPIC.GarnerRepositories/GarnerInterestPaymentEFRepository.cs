using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerInterestPayment;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.Dto.GarnerShared;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerInterestPaymentEFRepository : BaseEFRepository<GarnerInterestPayment>
    {
        public GarnerInterestPaymentEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerInterestPayment.SEQ}")
        {
        }

        /// <summary>
        /// Lợi tức đã trả cho lệnh, tìm trong bảng BondInterestPayment của order id truyền vào cộng tổng lại
        /// </summary>
        /// <param name="orderId">Id lệnh</param>
        /// <returns></returns>
        public decimal ProfitPaid(long orderId, DateTime? interestDate = null)
        {
            var paymentInterestQuery = from paymentDetails in _epicSchemaDbContext.GarnerInterestPaymentDetails
                                   join interestPayments in _dbSet on paymentDetails.InterestPaymentId equals interestPayments.Id
                                   where paymentDetails.OrderId == orderId && interestPayments.Deleted == YesNo.NO && (interestDate == null || interestPayments.PayDate < interestDate)
                                   select paymentDetails;
            return paymentInterestQuery.Sum(p => p.AmountReceived);
        }

        /// <summary>
        /// Tính lợi tức theo công thức: (Số tiền * số ngày * % lợi tức)/365
        /// </summary>
        public decimal FomulaInterest(decimal totalValue, int numInvestDays, decimal profitRate)
        {
            return (totalValue * numInvestDays * profitRate) / 365;
        }

        /// <summary>
        /// Tính toán lợi nhuận theo loại hình tính lợi tức
        /// </summary>
        /// <param name="calculateType">1: NET, 2: GROSS</param>
        /// <param name="totalValue">Tổng giá trị đầu tư</param>
        /// <param name="numInvestDays">Số ngày đầu tư</param>
        /// <param name="profitRate">% lợi nhuận</param>
        /// <param name="taxRate">% thuế lợi nhuận</param>
        /// <param name="profitPaid">số tiền đã trả theo lệnh</param>
        /// <returns></returns>
        public CalculateProfitDto CalculateProfit(int calculateType, decimal totalValue, int numInvestDays, decimal profitRate, decimal taxRate, decimal profitPaid)
        {
            var result = new CalculateProfitDto();
            if (calculateType == CalculateTypes.GROSS)
            {
                //lợi tức = (Số tiền * số ngày * % lợi tức (tra bậc))/365 - tổng tiền đã trả trước đấy
                result.Profit = FomulaInterest(totalValue, numInvestDays, profitRate) - profitPaid;
                if (result.Profit > 0) //nếu mà lợi nhuận lớn hơn 0
                {
                    result.Tax = result.Profit * taxRate;
                    result.ActualProfit = result.Profit - result.Tax;
                }
            }
            else if (calculateType == CalculateTypes.NET)
            {
                result.ActualProfit = FomulaInterest(totalValue, numInvestDays, profitRate) - profitPaid;
                if (result.ActualProfit > 0) // nếu số tiền thực nhận lớn hơn 0
                {
                    result.Profit = result.ActualProfit / (1 - profitRate);
                    result.Tax = result.Profit - result.ActualProfit;
                }
            }
            return result;
        }

        /// <summary>
        /// Ngày chi trả lợi tức gần nhất của lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DateTime? FindLastDatePaymentInterest(long orderId)
        {
            var paymentInterestQuery = from interestPayment in _dbSet
                                   join details in _epicSchemaDbContext.GarnerInterestPaymentDetails on interestPayment.Id equals details.InterestPaymentId
                                   where details.OrderId== orderId && interestPayment.Deleted== YesNo.NO
                                   select interestPayment;
            if (paymentInterestQuery.Any())
            {
                return paymentInterestQuery.Max(p => p.PayDate);
            }
            return null;
        }

        /// <summary>
        /// Thêm chi trả lợi tức (BondInterestPayment)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerInterestPayment Add(GarnerInterestPayment input)
        {
            _logger.LogInformation($"{nameof(GarnerInterestPaymentEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)} ");
            input.Id = (long)NextKey();
            input.CreatedDate = DateTime.Now;
            input.Status = InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Lấy danh sách đã lập chi trả 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public PagingResult<GarnerInterestPayment> FindAll(FilterGarnerInterestPaymentDto input)
        {
            _logger.LogInformation($"{nameof(GarnerInterestPaymentEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            PagingResult<GarnerInterestPayment> result = new();

            var cifQuery = from cif in _epicSchemaDbContext.CifCodes
                           join investor in _epicSchemaDbContext.Investors on cif.InvestorId equals investor.InvestorId into investors
                           from investor in investors.DefaultIfEmpty()
                           join businessCustomer in _epicSchemaDbContext.BusinessCustomers on cif.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                           from businessCustomer in businessCustomers.DefaultIfEmpty()
                           select new
                           {
                               cif.CifCode,
                               Phone = (investor != null && investor.Phone != null) ? investor.Phone : (businessCustomer != null ? businessCustomer.Phone : null),
                           };

            var interestPaymentQuery = from interestPayment in _dbSet.OrderByDescending(p => p.Id)
                                       join policy in _epicSchemaDbContext.GarnerPolicies on interestPayment.PolicyId equals policy.Id
                                       join distribution in _epicSchemaDbContext.GarnerDistributions on policy.DistributionId equals distribution.Id
                                       where  interestPayment.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO
                                       && (input.Status == null || (input.Status == InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA && (new List<int> { InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA, InterestPaymentStatus.CHO_PHAN_HOI}).Contains(interestPayment.Status))
                                       || (input.Status == InterestPaymentStatus.DA_DUYET_CHI_TIEN && (new List<int> { InterestPaymentStatus.DA_DUYET_CHI_TIEN, InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN }).Contains(interestPayment.Status)))
                                       && (input.InterestPaymentStatus == null || interestPayment.Status == input.InterestPaymentStatus)
                                       && (input.Phone == null || cifQuery.Where(c => c.Phone == input.Phone).Any())
                                       && (input.CifCode == null || cifQuery.Where(c => c.CifCode == input.CifCode).Any())
                                       && (input.DistributionId == null || distribution.Id == input.DistributionId)
                                       && (input.PolicyId == null || interestPayment.PolicyId == input.PolicyId)
                                       && (input.PayDate == null || interestPayment.PayDate <= input.PayDate.Value.Date)
                                       select interestPayment;
            // Lọc đúng ngày chi trả
            if (input.IsExactDate == YesNo.YES)
            {
                interestPaymentQuery = interestPaymentQuery.Where(r => (input.PayDate == null ? r.PayDate == DateTime.Now : r.PayDate == input.PayDate))
                    .OrderByDescending(r => r.PayDate.Date);
            }
            // Lọc cả các ngày chưa chi trả trước đó
            else if (input.IsExactDate == YesNo.NO)
            {
                interestPaymentQuery = interestPaymentQuery.Where(r => (input.PayDate == null ? r.PayDate <= DateTime.Now : r.PayDate <= input.PayDate))
                    .OrderByDescending(r => r.PayDate.Date);
            }

            if (input.PageSize != -1)
            {
                interestPaymentQuery = interestPaymentQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.TotalItems = interestPaymentQuery.Count();
            result.Items = interestPaymentQuery.ToList();
            return result;
        }

        public GarnerInterestPayment FindById(long interestPaymentId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerInterestPaymentEFRepository)}->{nameof(FindById)}: interestPaymentId = {interestPaymentId}, tradingProviderId = {tradingProviderId}");

            var result = _dbSet.FirstOrDefault(e => e.Id == interestPaymentId && (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Tính lợi tức chi trả theo thời gian (DASHBOARD)
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public decimal CalculateInterestPayment(List<int> tradingProviderIds, DateTime? now = null)
        {
            _logger.LogInformation($"{nameof(CalculateInterestPayment)}: tradingProviderIds={JsonSerializer.Serialize(tradingProviderIds)}; now={now}");

            var result = _epicSchemaDbContext.GarnerInterestPayments.AsNoTracking()
                        .Where(x => (tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId))
                                    && x.Deleted == YesNo.NO
                                    && new int[] {InterestPaymentStatus.DA_DUYET_CHI_TIEN, InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN}.Contains(x.Status)
                                     && (now == null || (x.ApproveDate != null && x.ApproveDate.Value.Date == now.Value.Date))
                        ).Sum(x => x.AmountMoney);

            return result;
        }
    }
}
