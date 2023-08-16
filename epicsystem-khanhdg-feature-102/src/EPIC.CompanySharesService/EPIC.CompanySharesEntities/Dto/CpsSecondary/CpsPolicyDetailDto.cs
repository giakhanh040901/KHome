using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsSecondary
{
    public class CpsPolicyDetailDto
    {
        public int PolicyDetailId { get; set; }
        public int PolicyId { get; set; }
        public int? STT { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string InterestPeriodType { get; set; }
        public int? InterestPeriodQuantity { get; set; }

        public string PeriodType { get; set; }
        public int? PeriodQuantity { get; set; }
        public string Status { get; set; }
        public decimal? Profit { get; set; }
        public int? InterestDays { get; set; }
        public int? InterestType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string IsShowApp { get; set; }
    }
}
