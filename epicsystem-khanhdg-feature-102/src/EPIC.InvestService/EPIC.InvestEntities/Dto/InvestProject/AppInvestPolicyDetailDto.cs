using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestProject
{
	public class AppInvestPolicyDetailDto
	{
		public int Id { get; set; }
		public int PolicyId { get; set; }
		public int? STT { get; set; }
		public string ShortName { get; set; }
		public string Name { get; set; }
		public string PeriodType { get; set; }
		public int? PeriodQuantity { get; set; }
		public int? InterestType { get; set; }
		public decimal? Profit { get; set; }
		public int? InterestDays { get; set; }
		public int? InterestPeriodQuantity { get; set; }
		public string InterestPeriodType { get; set; }

		/// <summary>
		/// Tính toán lợi tức
		/// </summary>
		public decimal? CalProfit { get; set; }
	}
}
