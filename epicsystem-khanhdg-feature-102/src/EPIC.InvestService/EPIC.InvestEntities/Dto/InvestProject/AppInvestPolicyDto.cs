using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestProject
{
    public class AppInvestPolicyDto
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int? Type { get; set; }
		//public string InvestorType { get; set; }
		public decimal? Classify { get; set; }
		public decimal? MinMoney { get; set; }
		/// <summary>
		/// Số tiền đầu tư tối đa theo chính sách
		/// </summary>
		public decimal? PolicyMaxMoney { get; set; }
		public decimal? MaxMoney { get; set; }
        //public int? InterestType { get; set; }
        public string Description { get; set; }
		public int? PolicyDisplayOrder { get; set; }

		/// <summary>
		/// Số tiền tối thiểu để yêu cầu nhận hợp đồng
		/// </summary>
        public decimal MinTakeContract { get; set; }
    }
}
