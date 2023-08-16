using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Policy
{
    /// <summary>
    /// Thông tin kỳ hạn không chứa Id
    /// </summary>
    public class SCInvestPolicyDetailDto
    {
        public int? STT { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string PeriodType { get; set; }
        public int? PeriodQuantity { get; set; }
        public decimal? Profit { get; set; }
        public int? InterestDays { get; set; }
        public int? InterestType { get; set; }
        public int? InterestPeriodQuantity { get; set; }
        public string InterestPeriodType { get; set; }
    }
}
