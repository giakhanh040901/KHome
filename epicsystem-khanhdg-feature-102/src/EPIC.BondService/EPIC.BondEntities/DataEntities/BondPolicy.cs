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
    public class BondPolicy : IFullAudited
    {
		[Column(Name = "ID")]
		public int Id { get; set; }
		[Column(Name = "TRADING_PROVIDER_ID")]
		public int TradingProviderId { get; set; }
		[Column(Name = "SECONDARY_ID")]
		public int SecondaryId { get; set; }

		[Required]
		[Column(Name = "CODE")]
		public string Code { get; set; }

        [Required]
        [Column(Name = "NAME")]
		public string Name { get; set; }

		[Column(Name = "TYPE")]
		public int Type { get; set; }

		[Column(Name = "INVESTOR_TYPE")]
		public string InvestorType { get; set; }
		[Column(Name = "INCOME_TAX")]
		public decimal IncomeTax { get; set; }
		[Column(Name = "TRANSFER_TAX")]
		public decimal TransferTax { get; set; }
		[Column(Name = "CLASSIFY")]
		public int Classify { get; set; }
		[Column(Name = "MIN_MONEY")]
		public decimal? MinMoney { get; set; }
		[Column(Name = "IS_TRANSFER")]
		public string IsTransfer { get; set; }
        public string IsShowApp { get; set; }

        [Column(Name = "STATUS")]
		public string Status { get; set; }
		[Column(Name = "START_DAY")]
		public DateTime? StartDate { get; set; }
		[Column(Name = "END_DAY")]
		public DateTime? EndDate { get; set; }
		[Column(Name = "DESCRIPTION")]
		public string Description { get; set; }
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
