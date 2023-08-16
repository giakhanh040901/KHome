using EPIC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class Secondary : IFullAudited
    {
        public int Id { get; set; }
        public int? CpsId { get; set; }
        public int? TradingProviderId { get; set; }
        public decimal? Quantity { get; set; }
        public int? Status { get; set; }
        public DateTime? OpenCellDate { get; set; }
        public DateTime? CloseCellDate { get; set; }
        public string IsClose { get; set; }
        public string IsShowApp { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string IsCheck { get; set; }
        public string OverviewImageUrl { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
    }
}
