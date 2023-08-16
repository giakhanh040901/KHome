using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondPolicy
{
    public class SCBondPolicyDetailDto
    {
        public int? STT { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string PeriodType { get; set; }
        public int? PeriodQuantity { get; set; }
        public int? InterestType { get; set; }
        public string InterestPeriodType { get; set; }
        public int? InterestPeriodQuantity { get; set; }
        public string Status { get; set; }
        public decimal? Profit { get; set; }
        public int? InterestDays { get; set; }
        public string IsShowApp { get; set; }
    }
}
