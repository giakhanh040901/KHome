using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondSecondary
    {
		[Column(Name = "ID")]
		public int Id { get; set; }

		[Column(Name = "BOND_ID")]
		public int BondId { get; set; }

		[Column(Name = "TRADING_PROVIDER_ID")]
		public int TradingProviderId { get; set; }

		[Column(Name = "PRIMARY_ID")]
		public int PrimaryId { get; set; }

		[Column(Name = "BUSINESS_CUSTOMER_BANK_ACC_ID")]
		public int BusinessCustomerBankAccId { get; set; }

		[Column(Name = "QUANTITY")]
		public long Quantity { get; set; }

		[Column(Name = "STATUS")]
		public int Status { get; set; }

		[Column(Name = "OPEN_SELL_DATE")]
		public DateTime? OpenSellDate { get; set; }

		[Column(Name = "CLOSE_SELL_DATE")]
		public DateTime? CloseSellDate { get; set; }

		[Required]
        [Column(Name = "IS_CLOSE")]		
		public string IsClose { get; set; }

		[Required]
		[Column(Name = "IS_SHOW_APP")]
		public string IsShowApp { get; set; }

        [Required]
        [Column(Name = "IS_CHECK")]
        public string IsCheck { get; set; }

        [Column(Name = "CONTENT_TYPE")]
        public string ContentType { get; set; }

        [Column(Name = "OVERVIEW_CONTENT")]
        public string OverviewContent { get; set; }

        [Column(Name = "OVERVIEW_IMAGE_URL")]
        public string OverviewImageUrl { get; set; }

        [Column(Name = "CREATED_BY")]
		public string CreatedBy { get; set; }

		[Column(Name = "CREATED_DATE")]
		public DateTime? CreatedDate { get; set; }

		[Column(Name = "MODIFIED_BY")]
		public string ModifiedBy { get; set; }

		[Column(Name = "MODIFIED_DATE")]
		public DateTime? ModifiedDate { get; set; }

		[Column(Name = "DELETED")]
		public string Deleted { get; set; }
	}

	/// <summary>
	/// KET QUA TRA RA TU PROC GET ALL
	/// </summary>
	public class ProductBondSecondaryView
	{
		// SECONDARY
		[Column(Name = "ID")]
		public int Id { get; set; }
		[Column(Name = "PRIMARY_ID")]
		public int PrimaryId { get; set; }
		[Column(Name = "QUANTITY")]
		public long Quantity { get; set; }
		[Column(Name = "HOLD_DATE")]
		public DateTime HoldDate { get; set; }
		[Column(Name = "STATUS")]
		public string Status { get; set; }
		[Column(Name = "CREATED_BY")]
		public string CreatedBy { get; set; }
		[Column(Name = "CREATED_DATE")]
		public DateTime CreatedDate { get; set; }
		[Column(Name = "MODIFIED_BY")]
		public string ModifiedBy { get; set; }

		[Column(Name = "IS_CHECK")]
		public string IsCheck { get; set; }

		[Column(Name = "MODIFIED_DATE")]
		public DateTime ModifiedDate { get; set; }
		// POLICY
		[Column(Name = "POLICY_ID")]
		public int PolicyId { get; set; }
		[Column(Name = "TRADING_PROVIDER_ID")]
		public int TradingProviderId { get; set; }
		[Column(Name = "POLICY_CODE")]
		public string PolicyCode { get; set; }
		[Column(Name = "POLICY_NAME")]
		public string PolicyName { get; set; }
		[Column(Name = "POLICY_TYPE")]
		public int PolicyType { get; set; }
		[Column(Name = "POLICY_TYPE_INVESTOR")]
		public string PolicyTypeInvestor { get; set; }
		[Column(Name = "POLICY_NUMBER_CLOSE_PER")]
		public int PolicyNumberClosePer { get; set; }
		[Column(Name = "POLICY_INCOME_TAX")]
		public decimal PolicyIncomeTax { get; set; }
		[Column(Name = "POLICY_MIN_MONEY")]
		public decimal PolicyMinMoney { get; set; }
		[Column(Name = "POLICY_STATUS")]
		public string PolicyStatus { get; set; }
		// POLICY DETAIL
		[Column(Name = "POLICY_DETAIL_ID")]
		public int PolicyDetailId { get; set; }
		[Column(Name = "DETAIL_CODE")]
		public string DetailCode { get; set; }
		[Column(Name = "DETAIL_NAME")]
		public string DetailName { get; set; }
		[Column(Name = "DETAIL_TYPE")]
		public decimal DetailType { get; set; }
		[Column(Name = "DETAIL_INTEREST_PERIOD_TYPE")]
		public string DetailInterestPeriodType { get; set; }
		[Column(Name = "DETAIL_INTEREST_PERIOD")]
		public decimal DetailInterestPeriod { get; set; }
		[Column(Name = "DETAIL_TYPE_INVESTOR")]
		public string DetailTypeInvestor { get; set; }
		[Column(Name = "DETAIL_NUMBER_CLOSE_PER")]
		public decimal DetailNumberClosePer { get; set; }
		[Column(Name = "DETAIL_INCOME_TAX")]
		public decimal DetailIncomeTax { get; set; }
		[Column(Name = "DETAIL_MIN_MONEY")]
		public decimal DetailMinMoney { get; set; }
		[Column(Name = "DETAIL_STATUS")]
		public string DetailStatus { get; set; }
		// PRIMARY
		[Column(Name = "PRIMARY_NAME")]
		public string PrimaryName { get; set; }
	}
}
