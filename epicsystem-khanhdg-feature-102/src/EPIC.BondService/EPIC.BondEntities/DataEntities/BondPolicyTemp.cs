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
    public class BondPolicyTemp : IFullAudited
    {
		[Column(Name = "ID")]
		public int Id { get; set; }
		[Column(Name = "CODE")]
		public string Code { get; set; }
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
		public decimal MinMoney { get; set; }

		[Required]
		[Column(Name = "IS_TRANSFER")]
		public string IsTransfer { get; set; }

		[Required]
		[Column(Name = "STATUS")]
		public string Status { get; set; }

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

	/// <summary>
	/// KET QUA TRA RA TU PROC GET ALL
	/// </summary>
	public class ProductBondPolicyTempView
	{
		//PolicyTemp

		[Column(Name = "ID")]
		public int Id { get; set; }
		[Column(Name = "CODE")]
		public string Code { get; set; }
		[Column(Name = "NAME")]
		public string Name { get; set; }
		[Column(Name = "TYPE")]
		public int? Type { get; set; }
		[Column(Name = "INVESTOR_TYPE")]
		public string InvestorType { get; set; }
		[Column(Name = "INCOME_TAX")]
		public decimal IncomeTax { get; set; }
		[Column(Name = "TRANSFER_TAX")]
		public decimal TransferTax { get; set; }
		[Column(Name = "CLASSIFY")]
		public int Classify { get; set; }
		[Column(Name = "MIN_MONEY")]
		public decimal MinMoney { get; set; }
		[Column(Name = "IS_TRANSFER")]
		public string IsTransfer { get; set; }
		[Column(Name = "STATUS")]
		public string Status { get; set; }
		[Column(Name = "CREATED_DATE")]
		public DateTime CreatedDate { get; set; }
		[Column(Name = "CREATED_BY")]
		public string CreatedBy { get; set; }
		[Column(Name = "MODIFIED_BY")]
		public string ModifiedBy { get; set; }
		[Column(Name = "MODIFIED_DATE")]
		public DateTime ModifiedDate { get; set; }
		[Column(Name = "DELETED")]
		public string Deleted { get; set; }

		//PolicyDetailTemp

		[Column(Name = "BOND_POLICY_DETAIL_TEMP_ID")]
		public int PolicyDetailTempId { get; set; }
		[Column(Name = "DE_STT")]
		public int? DeStt { get; set; }
		[Column(Name = "DE_SHORT_NAME")]
		public string DeShortName { get; set; }
		[Column(Name = "DE_NAME")]
		public string DeName { get; set; }
		[Column(Name = "DE_INTEREST_PERIOD_TYPE")]
		public string DeInterestPeriodType { get; set; }
		[Column(Name = "DE_INTEREST_PERIOD_QUANTITY")]
		public int? DeInterestPeriodQuantity { get; set; }
		[Column(Name = "DE_PERIOD_TYPE")]
		public string DePeriodType { get; set; }
		[Column(Name = "DE_PERIOD_QUANTITY")]
		public int DePeriodQuantity { get; set; }
		[Column(Name = "DE_STATUS")]
		public string DeStatus { get; set; }
		[Column(Name = "DE_PROFIT")]
		public decimal DeProfit { get; set; }
		[Column(Name = "DE_INTEREST_DAYS")]
		public int? DeInterestDays { get; set; }
		[Column(Name = "DE_INTEREST_TYPE")]
		public int? DeInterestType { get; set; }
		[Column(Name = "DE_CREATED_DATE")]
		public DateTime DeCreatedDate { get; set; }
		[Column(Name = "DE_CREATED_BY")]
		public string DeCreatedBy { get; set; }
		[Column(Name = "DE_MODIFIED_BY")]
		public string DeModifiedBy { get; set; }
		[Column(Name = "DE_MODIFIED_DATE")]
		public DateTime DeModifiedDate { get; set; }
	}
}
