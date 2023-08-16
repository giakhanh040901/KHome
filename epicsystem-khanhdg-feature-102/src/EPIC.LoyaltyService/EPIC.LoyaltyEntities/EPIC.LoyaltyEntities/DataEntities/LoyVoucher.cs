using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace EPIC.LoyaltyEntities.DataEntities
{
    [Table("LOY_VOUCHER", Schema = DbSchemas.EPIC_LOYALTY)]
    //[Index(nameof(Deleted), nameof(PartnerId), nameof(Status), IsUnique = false, Name = "IX_RST_OWNER")]
    public class LoyVoucher : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyVoucher).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Mã lô voucher
        /// </summary>
        [ColumnSnackCase(nameof(Code), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// Tên voucher
        /// </summary>
        [ColumnSnackCase(nameof(Name))]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Tên hiển thị trên ứng dụng
        /// </summary>
        [ColumnSnackCase(nameof(DisplayName), TypeName = "VARCHAR2")]
        [MaxLength(256)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Loại hình (C: Cứng; DT: Điện tử)
        /// <see cref="LoyVoucherTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(VoucherType))]
        public string VoucherType { get; set; }

        /// <summary>
        /// Loại voucher (TD: Tiêu dùng; MS: Mua sắm; AT: Ẩm thực; DV: Dịch vụ)
        /// <see cref="LoyVoucherUseTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(UseType), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string UseType { get; set; }

        /// <summary>
        /// Đơn vị của giá trị voucher (P: phần trăm; VND: VND)
        /// <see cref="LoyVoucherValueUnits"/>
        /// </summary>
        [ColumnSnackCase(nameof(Unit), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string Unit { get; set; }

        /// <summary>
        /// Giá trị voucher
        /// </summary>
        [ColumnSnackCase(nameof(Value))]
        public decimal? Value { get; set; }

        /// <summary>
        /// Điểm quy đổi
        /// </summary>
        [ColumnSnackCase(nameof(Point))]
        public int Point { get; set; }

        /// <summary>
        /// Điểm quy đổi
        /// </summary>
        [ColumnSnackCase(nameof(LinkVoucher), TypeName = "VARCHAR2")]
        [MaxLength(500)]
        public string LinkVoucher { get; set; }

        /// <summary>
        /// Ảnh đại diện voucher
        /// </summary>
        [ColumnSnackCase(nameof(Avatar), TypeName = "VARCHAR2")]
        [MaxLength(500)]
        public string Avatar { get; set; }

        /// <summary>
        /// Ảnh Banner voucher
        /// </summary>
        [ColumnSnackCase(nameof(BannerImageUrl), TypeName = "VARCHAR2")]
        [MaxLength(500)]
        public string BannerImageUrl { get; set; }

        /// <summary>
        /// Ngày áp dụng
        /// </summary>
        [ColumnSnackCase(nameof(StartDate), TypeName = "DATE")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        [ColumnSnackCase(nameof(EndDate), TypeName = "DATE")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        [ColumnSnackCase(nameof(ExpiredDate), TypeName = "DATE")]
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Ngày nhập lô voucher
        /// </summary>
        [ColumnSnackCase(nameof(BatchEntryDate), TypeName = "DATE")]
        public DateTime? BatchEntryDate { get; set; }

        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        [ColumnSnackCase(nameof(DescriptionContentType), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string DescriptionContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        [ColumnSnackCase(nameof(DescriptionContent), TypeName = "CLOB")]
        public string DescriptionContent { get; set; }

        /// <summary>
        /// Số lượng phát hành
        /// </summary>
        [ColumnSnackCase(nameof(PublishNum))]
        public int? PublishNum { get; set; }

        /// <summary>
        /// Số lượt đổi tối đa mỗi khách
        /// </summary>
        [ColumnSnackCase(nameof(ExchangeRoundNum))]
        public int? ExchangeRoundNum { get; set; }

        /// <summary>
        /// Đại lý sơ cấp
        /// </summary>
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Đối tác
        /// </summary>
        [ColumnSnackCase(nameof(PartnerId))]
        public int? PartnerId { get; set; }

        /// <summary>
        /// Hiển thị trên app
        /// </summary>
        [ColumnSnackCase(nameof(IsShowApp))]
        public string IsShowApp { get; set; }

        /// <summary>
        /// Đặt làm nổi bật
        /// </summary>
        [ColumnSnackCase(nameof(IsHot))]
        public string IsHot { get; set; }

        /// <summary>
        /// Sử dụng mã cho vòng quay may mắn
        /// </summary>
        [ColumnSnackCase(nameof(IsUseInLuckyDraw))]
        public string IsUseInLuckyDraw { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }

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
