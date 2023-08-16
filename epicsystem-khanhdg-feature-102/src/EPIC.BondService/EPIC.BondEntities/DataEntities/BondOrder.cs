using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
	[Table("EP_BOND_ORDER", Schema = DbSchemas.EPIC)]
	public class BondOrder : IFullAudited
    {
		public static string SEQ { get; } = $"SEQ_BOND_ORDER";
		[Key]
		[ColumnSnackCase(nameof(Id))]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		[ColumnSnackCase(nameof(TradingProviderId))]
		public int TradingProviderId { get; set; }

		[ColumnSnackCase(nameof(CifCode))]
		public string CifCode { get; set; }

		[ColumnSnackCase(nameof(DepartmentId))]
		public int? DepartmentId { get; set; }

		[ColumnSnackCase(nameof(DistributionContractId))]
		public int DistributionContractId { get; set; }

		[ColumnSnackCase(nameof(BondId))]
		public int BondId { get; set; }

		[ColumnSnackCase(nameof(SecondaryId))]
		public int SecondaryId { get; set; }

		[ColumnSnackCase(nameof(PolicyId))]
		public int PolicyId { get; set; }

		[ColumnSnackCase(nameof(PolicyDetailId))]
		public int PolicyDetailId { get; set; }

		[ColumnSnackCase(nameof(TotalValue))]
		public decimal TotalValue { get; set; }

		[ColumnSnackCase(nameof(BuyDate), TypeName = "DATE")]
		public DateTime? BuyDate { get; set; }

		[ColumnSnackCase(nameof(IsInterest))]
		public string IsInterest { get; set; }
		[ColumnSnackCase(nameof(SaleReferralCode))]
		public string SaleReferralCode { get; set; }

		[ColumnSnackCase(nameof(Source))]
		public int Source { get; set; }

		[ColumnSnackCase(nameof(ContractCode))]
		public string ContractCode { get; set; }

		[ColumnSnackCase(nameof(BusinessCustomerBankAccId))]
		public int? BusinessCustomerBankAccId { get; set; }

		[ColumnSnackCase(nameof(InvestorBankAccId))]
		public int? InvestorBankAccId { get; set; }

		[ColumnSnackCase(nameof(PaymentFullDate), TypeName = "DATE")]
		public DateTime? PaymentFullDate { get; set; }

		[ColumnSnackCase(nameof(Price))]
		public decimal? Price { get; set; }

		[ColumnSnackCase(nameof(Quantity))]
		public long? Quantity { get; set; }

		[ColumnSnackCase(nameof(Status))]
		public int Status { get; set; }

		[ColumnSnackCase(nameof(IpAddressCreated))]
		public string IpAddressCreated { get; set; }

		[ColumnSnackCase(nameof(InvestorIdenId))]
		public int? InvestorIdenId { get; set; }
		[ColumnSnackCase(nameof(ContractAddressId))]
		public int? ContractAddressId { get; set; }
		[ColumnSnackCase(nameof(DepartmentName))]
		public string DepartmentName { get; set; }
		[ColumnSnackCase(nameof(DeliveryCode))]
		public string DeliveryCode { get; set; }
		[ColumnSnackCase(nameof(DeliveryStatus))]
		public int? DeliveryStatus { get; set; }

		/// <summary>
		/// Ngày đầu tư
		/// </summary>
		[ColumnSnackCase(nameof(InvestDate), TypeName = "DATE")]
		public DateTime? InvestDate { get; set; }

		/// <summary>
		/// Phương thức tất toán cuối kỳ
		/// </summary>
		[ColumnSnackCase(nameof(SettlementMethod))]
		public int? SettlementMethod { get; set; }

		/// <summary>
		/// Loại kỳ hạn sau khi tái tục
		/// </summary>
		[ColumnSnackCase(nameof(RenewalsPolicyDetailId))]
		public int? RenewalsPolicyDetailId { get; set; }

		/// <summary>
		/// Id sale đặt lệnh hộ
		/// </summary>
		[ColumnSnackCase(nameof(SaleOrderId))]
		public int? SaleOrderId { get; set; }

		/// <summary>
		/// Phòng ban bán hộ
		/// </summary>
		[ColumnSnackCase(nameof(DepartmentIdSub))]
		public int? DepartmentIdSub { get; set; }

		/// <summary>
		/// Mã sale bán hộ
		/// </summary>
		[ColumnSnackCase(nameof(SaleReferralCodeSub))]
		public string SaleReferralCodeSub { get; set; }

		#region audit
		[ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
		public DateTime? CreatedDate { get; set; }

		[ColumnSnackCase(nameof(CreatedBy))]
		[MaxLength(50)]
		public string CreatedBy { get; set; }

		[ColumnSnackCase(nameof(ModifiedDate), TypeName = "DATE")]
		public DateTime? ModifiedDate { get; set; }

		[ColumnSnackCase(nameof(ModifiedBy))]
		[MaxLength(50)]
		public string ModifiedBy { get; set; }

		[ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
		[Required]
		[MaxLength(1)]
		public string Deleted { get; set; }
		#endregion
	}
}
