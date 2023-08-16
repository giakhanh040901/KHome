using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Sổ lệnh
    /// </summary>
    [Table("RST_ORDER", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(CifCode), nameof(ProductItemId), nameof(OpenSellDetailId), 
        nameof(SaleReferralCode), nameof(DistributionPolicyId), nameof(Status), IsUnique = false, Name = "IX_RST_ORDER")]
    public class RstOrder : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstOrder).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(CifCode), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string CifCode { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Id sản phẩm
        /// </summary>
        [ColumnSnackCase(nameof(ProductItemId))]
        public int ProductItemId { get; set; }

        /// <summary>
        /// Id sản phẩm trong mở bán
        /// </summary>
        [ColumnSnackCase(nameof(OpenSellDetailId))]
        public int OpenSellDetailId { get; set; }

        /// <summary>
        /// Nếu đặt trên app sẽ có id này
        /// </summary>
        [ColumnSnackCase(nameof(CartId))]
        public int? CartId { get; set; }

        /// <summary>
        /// EH 8 số 0 kèm id
        /// </summary>
        [ColumnSnackCase(nameof(ContractCode), TypeName = "VARCHAR2")]
        [MaxLength(100)]
        public string ContractCode { get; set; }

        /// <summary>
        /// Hình thức thanh toán mua nhà:1 Thanh toán thường, 2 Thanh toán Sớm 3: Trả góp ngân hàng
        /// <see cref="RstOrderPaymentypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(PaymentType))]
        public int? PaymentType { get; set; }

        [ColumnSnackCase(nameof(InvestorIdenId))]
        public int? InvestorIdenId { get; set; }

        /// <summary>
        /// Địa chỉ nhận hợp đồng bản cứng
        /// </summary>
        [ColumnSnackCase(nameof(ContractAddressId))]
        public int? ContractAddressId { get; set; }

        /// <summary>
        /// Chính sách tại thời điểm đặt lệnh lưu vết vào đây
        /// </summary>
        [ColumnSnackCase(nameof(DistributionPolicyId))]
        public int DistributionPolicyId { get; set; }

        /// <summary>
        /// Số tiền cọc phải nộp tính ra lưu vào trường này dựa theo chính sách phân phối
        /// </summary>
        [ColumnSnackCase(nameof(DepositMoney), TypeName = "NUMBER(18, 6)")]
        public decimal DepositMoney { get; set; }

        /// <summary>
        /// Đại lý phòng ban của sale
        /// </summary>
        [ColumnSnackCase(nameof(DepartmentId))]
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Mã giới thiệu của sale
        /// </summary>
        [ColumnSnackCase(nameof(SaleReferralCode), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string SaleReferralCode { get; set; }

        /// <summary>
        /// Đại lý phòng ban bán hộ
        /// </summary>
        [ColumnSnackCase(nameof(DepartmentIdSub))]
        public int? DepartmentIdSub { get; set; }

        /// <summary>
        /// Mã giới thiệu của Sale bán hộ
        /// </summary>
        [ColumnSnackCase(nameof(SaleReferralCodeSub), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string SaleReferralCodeSub { get; set; }

        /// <summary>
        /// Id sale đặt lệnh hộ
        /// </summary>
        [ColumnSnackCase(nameof(SaleOrderId))]
        public int? SaleOrderId { get; set; }

        /// <summary>
        /// Nguồn đặt lệnh: 1: ONLINE, 2: OFFLINE
        /// <see cref="SourceOrder"/>
        /// </summary>
        [ColumnSnackCase(nameof(Source))]
        public int Source { get; set; }

        /// <summary>
        /// Thời gian hết hạn chuyển tiền lưu ngày giờ, đặt cả app lẫn cms đều có giờ này
        /// </summary>
        [ColumnSnackCase(nameof(ExpTimeDeposit), TypeName = "DATE")]
        public DateTime? ExpTimeDeposit { get; set; }

        /// <summary>
        /// Id background job hangfire
        /// </summary>
        [MaxLength(50)]
        [ColumnSnackCase(nameof(DepositJobId), TypeName = "VARCHAR2")]
        public string DepositJobId { get; set; }

        /// <summary>
        /// Trạng thái lệnh (1: Khởi tạo, 2: Chờ thanh toán cọc, 3: Chờ ký hợp đồng, 4: Chờ duyệt hợp đồng, 5: Đã cọc, 6: Phong tỏa, 7: Đã xóa)<br/>
        /// Phong tỏa khi trạng thái căn hộ bị CĐT reset về khởi tạo
        /// <see cref="RstOrderStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Trạng thái max mà hợp đồng đã đến
        /// </summary>
        [ColumnSnackCase(nameof(StatusMax))]
        public int StatusMax { get; set; }

        [ColumnSnackCase(nameof(IpAddressCreated), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string IpAddressCreated { get; set; }

        [ColumnSnackCase(nameof(ApproveBy))]
        [MaxLength(50)]
        public string ApproveBy { get; set; }

        [ColumnSnackCase(nameof(ApproveDate), TypeName = "DATE")]
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Ngày hợp đồng cọc
        /// </summary>
        [ColumnSnackCase(nameof(DepositDate), TypeName = "DATE")]
        public DateTime? DepositDate { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng của đối tác
        /// </summary>
        [ColumnSnackCase(nameof(PartnerBankAccountId))]
        public int? PartnerBankAccountId { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng của đại lý
        /// </summary>
        [ColumnSnackCase(nameof(TradingBankAccountId))]
        public int? TradingBankAccountId { get; set; }

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
        [DefaultValue(YesNo.NO)]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
