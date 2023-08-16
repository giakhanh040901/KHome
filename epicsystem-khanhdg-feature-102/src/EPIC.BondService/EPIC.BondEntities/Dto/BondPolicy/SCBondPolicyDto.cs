using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondPolicy
{
    public class SCBondPolicyDto
    {
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
        public string IsShowApp { get; set; }
    }
}
