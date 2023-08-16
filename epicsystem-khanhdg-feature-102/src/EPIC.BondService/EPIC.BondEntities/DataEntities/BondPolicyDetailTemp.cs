using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondPolicyDetailTemp : IFullAudited
    {
		[Column(Name = "ID")]
		public int Id { get; set; }
		[Column(Name = "POLICY_TEMP_ID")]
		public int PolicyTempId { get; set; }
		[Column(Name = "STT")]
		public int? STT { get; set; }
		[Column(Name = "SHORT_NAME")]
		public string ShortName { get; set; }
		[Column(Name = "NAME")]
		public string Name { get; set; }

        [Column(Name = "INTEREST_TYPE")]
        public int InterestType { get; set; }

        [Column(Name = "INTEREST_PERIOD_TYPE")]
		public string InterestPeriodType { get; set; }
		[Column(Name = "INTEREST_PERIOD_QUANTITY")]
		public int? InterestPeriodQuantity { get; set; }

		[Column(Name = "PERIOD_TYPE")]
		public string PeriodType { get; set; }
		[Column(Name = "PERIOD_QUANTITY")]
		public int PeriodQuantity { get; set; }

		[Column(Name = "STATUS")]
		public string Status { get; set; }
		[Column(Name = "PROFIT")]
		public decimal Profit { get; set; }

		[Column(Name = "INTEREST_DAYS")]
		public int? InterestDays { get; set; }

		[Column(Name = "CREATED_DATE")]
		public DateTime? CreatedDate { get; set; }
		[Column(Name = "CREATED_BY")]
		public string CreatedBy { get; set; }
		[Column(Name = "MODIFIED_BY")]
		public string ModifiedBy { get; set; }
		[Column(Name = "MODIFIED_DATE")]
		public DateTime? ModifiedDate { get; set; }
		[Column(Name = "DELETED")]
		public string Deleted { get; set; }
	}
}
