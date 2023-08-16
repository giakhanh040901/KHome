using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.Policy
{
	public class AppCpsPolicyDto
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int? Type { get; set; }
		//public string InvestorType { get; set; }
		public decimal? Classify { get; set; }
		public decimal? MinMoney { get; set; }
		public decimal? MaxMoney { get; set; }
	}
}
