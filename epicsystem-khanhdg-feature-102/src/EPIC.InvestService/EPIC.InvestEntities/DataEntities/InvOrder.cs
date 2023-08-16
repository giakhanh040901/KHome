using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_ORDER", Schema = DbSchemas.EPIC)]
    public class InvOrder : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_INV_ORDER";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
        public TradingProvider TradingProvider { get; set; }

        [ColumnSnackCase(nameof(CifCode))]
        public string CifCode { get; set; }
        [JsonIgnore]
        public CifCodes CifCodes { get; set; }

        [ColumnSnackCase(nameof(DepartmentId))]
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        [ColumnSnackCase(nameof(DistributionId))]
        public int DistributionId { get; set; }
        public Distribution Distribution { get; set; }

        [ColumnSnackCase(nameof(PolicyId))]
        public int PolicyId { get; set; }
        public Policy Policy { get; set; }

        [ColumnSnackCase(nameof(PolicyDetailId))]
        public int PolicyDetailId { get; set; }
        public PolicyDetail PolicyDetail { get; set; }

        [ColumnSnackCase(nameof(TotalValue))]
        public decimal TotalValue { get; set; }
        /// <summary>
        /// Số tiền đầu tư ban đầu
        /// </summary>
        [ColumnSnackCase(nameof(InitTotalValue))]
        public decimal InitTotalValue { get; set; }

        [ColumnSnackCase(nameof(BuyDate), TypeName = "DATE")]
        public DateTime? BuyDate { get; set; }

        [ColumnSnackCase(nameof(IsInterest))]
        public string IsInterest { get; set; }

        [ColumnSnackCase(nameof(SaleReferralCode))]
        public string SaleReferralCode { get; set; }

        ///// <summary>
        ///// Sale giới thiệu
        ///// </summary>
        //[ColumnSnackCase(nameof(SaleReferralId))]
        //[NotMapped]
        //public int? SaleReferralId { get; set; }

        /// <summary>
        /// online, offline
        /// </summary>
        [ColumnSnackCase(nameof(Source))]
        public int Source { get; set; }

        [ColumnSnackCase(nameof(ContractCode))]
        public string ContractCode { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerBankAccId))]
        public int? BusinessCustomerBankAccId { get; set; }
        public BusinessCustomerBank BusinessCustomerBankAcc { get; set; }

        /// <summary>
        /// Đang lưu chung bank cả khách doanh nghiệp và khách cá nhân
        /// </summary>
        [ColumnSnackCase(nameof(InvestorBankAccId))]
        public int? InvestorBankAccId { get; set; }
        //public InvestorBankAccount InvestorBankAccount { get; set; }

        [ColumnSnackCase(nameof(PaymentFullDate), TypeName = "DATE")]
        public DateTime? PaymentFullDate { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        [ColumnSnackCase(nameof(IpAddressCreated))]
        public string IpAddressCreated { get; set; }

        [ColumnSnackCase(nameof(InvestorIdenId))]
        public int? InvestorIdenId { get; set; }
        [JsonIgnore]
        public InvestorIdentification InvestorIdentification { get; set; }

        [ColumnSnackCase(nameof(ContractAddressId))]
        public int? ContractAddressId { get; set; }
        [JsonIgnore]
        public InvestorContactAddress InvestorContactAddress { get; set; }

        [ColumnSnackCase(nameof(DeliveryCode))]
        public string DeliveryCode { get; set; }

        [ColumnSnackCase(nameof(DeliveryStatus))]
        public int? DeliveryStatus { get; set; }

        /// <summary>
        /// Ngày tất toán
        /// </summary>
        [ColumnSnackCase(nameof(SettlementDate))]
        public DateTime? SettlementDate { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        [ColumnSnackCase(nameof(InvestDate), TypeName = "DATE")]
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Ngày hợp đồng đáo hạn
        /// </summary>
        [ColumnSnackCase(nameof(DueDate), TypeName = "DATE")]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Ngày active
        /// </summary>
        [ColumnSnackCase(nameof(ActiveDate), TypeName = "DATE")]
        public DateTime? ActiveDate { get; set; }

        /// <summary>
        /// Người duyệt
        /// </summary>
        [ColumnSnackCase(nameof(ApproveBy))]
        public string ApproveBy { get; set; }

        /// <summary>
        /// Ngày duyệt
        /// </summary>
        [ColumnSnackCase(nameof(ApproveDate), TypeName = "DATE")]
        public DateTime? ApproveDate { get; set; }

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

        /// <summary>
        /// ngày giao nhận và người giao nhận hợp đồng
        /// </summary>
        [ColumnSnackCase(nameof(PendingDate))]
        public DateTime? PendingDate { get; set; }

        [ColumnSnackCase(nameof(DeliveryDate))]
        public DateTime? DeliveryDate { get; set; }

        [ColumnSnackCase(nameof(ReceivedDate))]
        public DateTime? ReceivedDate { get; set; }

        [ColumnSnackCase(nameof(FinishedDate))]
        public DateTime? FinishedDate { get; set; }

        [ColumnSnackCase(nameof(FinishedDateModifiedBy))]
        public string FinishedDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(PendingDateModifiedBy))]
        public string PendingDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(DeliveryDateModifiedBy))]
        public string DeliveryDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(ReceivedDateModifiedBy))]
        public string ReceivedDateModifiedBy { get; set; }

        /// <summary>
        /// 1: Quản trị viên đặt; 2: Khách đặt; 3: Sale đặt
        /// </summary>
        [NotMapped]
        public int? OrderSource { get; set; }

        [ColumnSnackCase(nameof(RenewalsReferId))]
        public long? RenewalsReferId { get; set; }

        [ColumnSnackCase(nameof(RenewalsReferIdOriginal))]
        public long? RenewalsReferIdOriginal { get; set; }

        /// <summary>
        /// Tái tục lần thứ ?
        /// </summary>
        [ColumnSnackCase(nameof(RenewalIndex))]
        public int? RenewalIndex { get; set; }

        /// <summary>
        /// Hình thức chi trả lợi tức, đáo hạn
        /// <see cref="InvestMethodInterests"/>
        /// </summary>
        [ColumnSnackCase(nameof(MethodInterest))]
        public int MethodInterest { get; set; }

        /// <summary>
        /// Id background job hangfire
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(BackgroundJobId), TypeName = "VARCHAR2")]
        public string BackgroundJobId { get; set; }

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
        [NotMapped]
        public string CustomerName { get; set; }
    }
}
