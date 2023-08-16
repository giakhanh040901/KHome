using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InterestPayment
{
    public class InvestInterestPaymentDto
    {
        public int Id { get; set; }
        public long? OrderId { get; set; }
        public int? PeriodIndex { get; set; }
        public string CifCode { get; set; }
        public decimal? AmountMoney { get; set; }
        public decimal? Profit { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalValueInvestment { get; set; }
        public int? PolicyDetailId { get; set; }
        public int? TradingProviderId { get; set; }
        public string IsLastPeriod { get; set; }
        public int? Status { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveBy { get; set; }
    }
}
