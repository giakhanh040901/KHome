using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InterestPayment
{
    public class InvestInterestPaymentApproveRenewalDto
    {
        public int Id { get; set; }
        public long? OrderId { get; set; }

        /// <summary>
        /// Id hợp đồng mới sau tái tục
        /// </summary>
        public long? NewOrderId { get; set; }
        public int? PeriodIndex { get; set; }
        public string CifCode { get; set; }
        public decimal? AmountMoney { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalValueInvestment { get; set; }

        /// <summary>
        /// Kỳ hạn mới sau tái tục
        /// </summary>
        public int? PolicyDetailId { get; set; }
        public int? TradingProviderId { get; set; }
        public string IsLastPeriod { get; set; }
        public int? Status { get; set; }
        public DateTime? PayDate { get; set; }
    }
}
