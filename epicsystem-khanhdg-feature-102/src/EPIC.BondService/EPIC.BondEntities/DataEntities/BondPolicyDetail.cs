using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondPolicyDetail : IFullAudited
    {
		[Column(Name = "ID")]
		public int Id { get; set; }

		[Column(Name = "TRADING_PROVIDER_ID")]
		public int TradingProviderId { get; set; }

		[Column(Name = "POLICY_ID")]
		public int PolicyId { get; set; }

		[Column(Name = "SECONDARY_ID")]
		public int SecondaryId { get; set; }

		[Column(Name = "STT")]
		public int? STT { get; set; }

		[Required]
		[Column(Name = "SHORT_NAME")]
		public string ShortName { get; set; }

        [Required]
        [Column(Name = "NAME")]
		public string Name { get; set; }

        [Column(Name = "INTEREST_TYPE")]
        public int InterestType { get; set; }

        [Required]
		[Column(Name = "PERIOD_TYPE")]
		public string PeriodType { get; set; }

		[Column(Name = "PERIOD_QUANTITY")]
		public int PeriodQuantity { get; set; }

		[Column(Name = "INTEREST_PERIOD_TYPE")]
		public string InterestPeriodType { get; set; }

		[Column(Name = "INTEREST_PERIOD_QUANTITY")]
		public int? InterestPeriodQuantity { get; set; }

		[Column(Name = "STATUS")]
		public string Status { get; set; }

		[Column(Name = "PROFIT")]
		public decimal Profit { get; set; }

		[Column(Name = "INTEREST_DAYS")]
		public int? InterestDays { get; set; }

        [Required]
        public string IsShowApp { get; set; }

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
