using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Policy
{
    /// <summary>
    /// Thông tin chính sách của INVEST không chứa Id
    /// </summary>
    public class SCnvestPolicyDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public decimal? Classify { get; set; }
        public decimal? MinMoney { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
