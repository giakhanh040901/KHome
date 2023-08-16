using EPIC.Utils.Attributes;
using System.ComponentModel.DataAnnotations;
using System;
using EPIC.Entities;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using EPIC.Utils.DataUtils;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Chính sách ưu đãi từ CĐT và đại lý dựa theo loại hình đặt lệnh là online hay offline
    /// </summary>
    [Table("RST_ORDER_SELLING_POLICY", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(OrderId), nameof(ProductItemProjectPolicyId), nameof(SellingPolicyId), IsUnique = false, Name = "IX_RST_ORDER_SELLING_POLICY")]
    public class RstOrderSellingPolicy : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstOrderSellingPolicy).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(OrderId))]
        public int OrderId { get; set; }
        /// <summary>
        /// Id chính sách ưu đãi chủ đầu tư chọn cho sản phẩm
        /// </summary>
        [ColumnSnackCase(nameof(ProductItemProjectPolicyId))]
        public int? ProductItemProjectPolicyId { get; set; }
        /// <summary>
        /// Id chính sách ưu đãi của đại lý
        /// </summary>
        [ColumnSnackCase(nameof(SellingPolicyId))]
        public int? SellingPolicyId { get; set; }

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
