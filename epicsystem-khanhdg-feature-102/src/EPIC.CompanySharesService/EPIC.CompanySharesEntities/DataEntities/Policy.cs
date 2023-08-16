using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class Policy : IFullAudited
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int SecondaryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public string InvestorType { get; set; }
        public decimal? IncomeTax { get; set; }
        public decimal? TransferTax { get; set; }
        public int? Classify { get; set; }
        public decimal? MinMoney { get; set; }
        public string IsTransfer { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string IsShowApp { get; set; }
    }
}
