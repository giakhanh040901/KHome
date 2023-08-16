using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_DISTRIBUTION", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Ban phan phoi (phan phoi san pham)")]
    public class GarnerDistribution : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerDistribution)).ToSnakeUpperCase()}";

        #region Id
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(ProductId))]
        [Comment("Id san pham")]
        public int ProductId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int TradingProviderId { get; set; }
        #endregion

        [ColumnSnackCase(nameof(OpenCellDate), TypeName = "DATE")]
        [Comment("Ngay mo ban")]
        public DateTime? OpenCellDate { get; set; }

        [ColumnSnackCase(nameof(CloseCellDate), TypeName = "DATE")]
        [Comment("Ngay ket thuc ban")]
        public DateTime? CloseCellDate { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Status))]
        [Comment("Trang thai (1: Khoi tao, 2: Trinh duyet, 3: Hoat dong, 4: Huy duyet)")]
        public int Status { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsClose), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Dong khong cho dat lenh")]
        public string IsClose { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsShowApp), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Co show app")]
        public string IsShowApp { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsCheck), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Da duyet")]
        public string IsCheck { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsDefault), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Co la mac dinh theo dai ly")]
        public string IsDefault { get; set; }

        [NotMapped]
        [Obsolete("Chuyển sang lưu nhiều bank")]
        public int BusinessCustomerBankAccId { get; set; }

        [ColumnSnackCase(nameof(ContentType), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        [Comment("Loai noi dung tong quan")]
        public string ContentType { get; set; }

        [ColumnSnackCase(nameof(OverviewContent), TypeName = "CLOB")]
        [Comment("Noi dung tong quan")]
        public string OverviewContent { get; set; }

        [ColumnSnackCase(nameof(OverviewImageUrl), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        [Comment("Noi dung hinh anh tong quan")]
        public string OverviewImageUrl { get; set; }

        /// <summary>
        /// Hình ảnh phân phối
        /// </summary>
        [ColumnSnackCase(nameof(Image), TypeName = "VARCHAR2")]
        [MaxLength(512)]
        public string Image { get; set; }
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
