using EPIC.DataAccess.Base;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestOrderPaymentEFRepository : BaseEFRepository<OrderPayment>
    {
        public InvestOrderPaymentEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{OrderPayment.SEQ}")
        {
        }

        public OrderPayment Add(OrderPayment input)
        {
            _logger.LogInformation($"{nameof(InvestOrderPaymentEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (long)NextKey();
            input.CreatedDate = DateTime.Now;
            input.Deleted = YesNo.NO;
            return _dbSet.Add(input).Entity;
        }

        public decimal SumPaymentAmount(long orderId)
        {
            return _dbSet.Where(o => o.OrderId == orderId && o.TranType == TranTypes.THU && o.TranClassify == TranClassifies.THANH_TOAN && o.Deleted == YesNo.NO && o.Status == OrderPaymentStatus.DA_THANH_TOAN)
                .Sum(o => o.PaymentAmnount) ?? 0;
        }

        /// <summary>
        /// Lấy tiền vào dashboard
        /// </summary>
        /// <param name="isCummulative"></param>
        /// <param name="tradingProviderIds"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public decimal DashboardCashInFlow(bool isCummulative, List<int> tradingProviderIds, DateTime? now = null)
        {
            decimal result = 0;
            if (isCummulative)
            {
                result = (from payment in _epicSchemaDbContext.InvestOrderPayments
                          join order in _epicSchemaDbContext.InvOrders on payment.OrderId equals order.Id
                          where payment.Deleted == YesNo.NO && payment.Status == OrderPaymentStatus.DA_THANH_TOAN && payment.TranType == TranTypes.THU
                          && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(payment.TradingProviderId))
                          && order.Deleted == YesNo.NO
                          select payment).Sum(p => p.PaymentAmnount ?? 0);
            }
            else
            {
                // Tổng số tiền được duyệt thanh toán trong ngày
                result = (from payment in _epicSchemaDbContext.InvestOrderPayments
                          join order in _epicSchemaDbContext.InvOrders on payment.OrderId equals order.Id
                          where payment.Deleted == YesNo.NO && payment.Status == OrderPaymentStatus.DA_THANH_TOAN && payment.TranType == TranTypes.THU
                          && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(payment.TradingProviderId))
                          && order.Deleted == YesNo.NO && (now == null || order.PaymentFullDate != null && order.PaymentFullDate.Value.Date == now.Value.Date)
                          select payment).Sum(p => p.PaymentAmnount ?? 0);

            }
            return result;
        }

        /// <summary>
        /// Dashboard tiền ra
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public decimal DashboardCashOutFlow(List<int> tradingProviderIds, DateTime? now = null)
        {
            decimal result = 0;
            // Lấy giá trị được rút hoặc chi được lập sau khi duyệt ở bảng Thanh toán
            result = (from payment in _epicSchemaDbContext.InvestOrderPayments
                      join order in _epicSchemaDbContext.InvOrders on payment.OrderId equals order.Id
                      where payment.Deleted == YesNo.NO && payment.Status == OrderPaymentStatus.DA_THANH_TOAN && payment.TranType == TranTypes.CHI
                      && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(payment.TradingProviderId))
                      && order.Deleted == YesNo.NO && (now == null || payment.ApproveDate != null && payment.ApproveDate.Value.Date == now.Value.Date)
                      select payment).Sum(p => p.PaymentAmnount ?? 0);

            return result;
        }

        public List<OrderPayment> FindAllByOrderId(long orderId, int? status = null, int? tranType = null)
        {
            _logger.LogInformation($"{nameof(InvestOrderPaymentEFRepository)}->{nameof(FindAllByOrderId)}: orderId = {orderId}, status = {status}");
            return _dbSet.Where(o => o.OrderId == orderId && (tranType == null || o.TranType == tranType) && o.Deleted == YesNo.NO && (status == null || o.Status == status)).ToList();
        }
    }
}
