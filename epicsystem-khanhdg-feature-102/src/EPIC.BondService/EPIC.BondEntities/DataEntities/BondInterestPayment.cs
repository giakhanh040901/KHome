using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondInterestPayment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int PeriodIndex { get; set; }
        public string CifCode { get; set; }
        public decimal AmountMoney { get; set; }
        public int PolicyDetailId { get; set; }
        public int TradingProviderId { get; set; }
        public DateTime? PayDate { get; set; }
        public string IsLastPeriod { get; set; }
        public int Status { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
    }
}
