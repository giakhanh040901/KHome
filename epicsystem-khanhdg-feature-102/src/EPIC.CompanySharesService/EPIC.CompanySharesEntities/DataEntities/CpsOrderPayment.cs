using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class CpsOrderPayment : IFullAudited
    {
        public long Id { get; set; }
        public int? TradingProviderId { get; set; }
        public long? OrderId { get; set; }
        public DateTime? TranDate { get; set; }
        public int? TranType { get; set; }
        public int? TranClassify { get; set; }
        public int? PaymentType { get; set; }
        public decimal? PaymentAmnount { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
    }
}
