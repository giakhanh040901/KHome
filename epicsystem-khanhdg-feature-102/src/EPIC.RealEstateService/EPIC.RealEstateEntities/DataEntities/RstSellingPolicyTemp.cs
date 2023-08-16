using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Chính sách bán hàng
    /// </summary>
    [Table("RST_SELLING_POLICY_TEMP", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(Status), nameof(TradingProviderId), IsUnique = false, Name = "IX_RST_SELLING_POLICY_TEMP")]
    public class RstSellingPolicyTemp : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstSellingPolicyTemp).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }


        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Code))]
        public string Code { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        /// <summary>
        /// Loại hình áp dụng: 1 giá trị cố định, 2 giá trị căn hộ 
        /// </summary>  
        [ColumnSnackCase(nameof(SellingPolicyType))]
        public int SellingPolicyType { get; set; }

        /// <summary>
        /// Loại hình áp dụng: 1 online, 2 offline, 3 all
        /// </summary>
        [ColumnSnackCase(nameof(Source))]
        public int Source { get; set; }

        /// <summary>
        /// Giá trị quy ra tiền
        /// </summary>
        [ColumnSnackCase(nameof(ConversionValue), TypeName = "NUMBER(18, 6)")]
        public decimal ConversionValue { get; set; }

        /// <summary>
        /// Giá trị từ
        /// </summary>
        [ColumnSnackCase(nameof(FromValue), TypeName = "NUMBER(18, 6)")]
        public decimal? FromValue { get; set; }

        /// <summary>
        /// Đến giá trị
        /// </summary>
        [ColumnSnackCase(nameof(ToValue), TypeName = "NUMBER(18, 6)")]
        public decimal? ToValue { get; set; }

        /// <summary>
        /// Trạng thái A, D
        /// </summary>
        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

        /// <summary>
        /// Mô tả chính sách
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(Description))]
        public string Description { get; set; }

        /// <summary>
        /// Tên file
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(FileName))]
        public string FileName { get; set; }

        /// <summary>
        /// Đường dẫn file
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(FileUrl))]
        public string FileUrl { get; set; }

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
