using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using DocumentFormat.OpenXml.CustomXmlSchemaReferences;
using System.ComponentModel.DataAnnotations.Schema;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.DataUtils;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Sổ lệnh đồng sở hữu
    /// </summary>
    [Table("RST_ORDER_CO_OWNER", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(OrderId), nameof(InvestorIdenId), IsUnique = false, Name = "IX_RST_ORDER_CO_OWNER")]
    public class RstOrderCoOwner : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstOrderCoOwner).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        public int OrderId { get; set; }

        /// <summary>
        /// Giấy tờ chọn từ hệ thống
        /// </summary>
        [ColumnSnackCase(nameof(InvestorIdenId))]
        public int? InvestorIdenId { get; set; }

        [ColumnSnackCase(nameof(Fullname))]
        [MaxLength(100)]
        public string Fullname { get; set; }

        [ColumnSnackCase(nameof(Phone), TypeName = "VARCHAR2")]
        [MaxLength(100)]
        public string Phone { get; set; }

        /// <summary>
        /// Địa chỉ liên hệ
        /// </summary>
        [ColumnSnackCase(nameof(Address))]
        [MaxLength(256)]
        public string Address { get; set; }

        /// <summary>
        /// Loại giấy tờ: CCCD, CMND, PASSPORT
        /// <see cref="CardTypesInput"/>
        /// </summary>
        [ColumnSnackCase(nameof(IdType), TypeName = "VARCHAR2")]
        [MaxLength(10)]
        public string IdType { get; set; }

        /// <summary>
        /// Ảnh mặt trước
        /// </summary>
        [ColumnSnackCase(nameof(IdFrontImageUrl), TypeName = "VARCHAR2")]
        [MaxLength(512)]
        public string IdFrontImageUrl { get; set; }

        /// <summary>
        /// Ảnh mặt sau
        /// </summary>
        [ColumnSnackCase(nameof(IdBackImageUrl), TypeName = "VARCHAR2")]
        [MaxLength(512)]
        public string IdBackImageUrl { get; set; }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        [ColumnSnackCase(nameof(IdNo), TypeName = "VARCHAR2")]
        [MaxLength(256)]
        public string IdNo { get; set; }

        [ColumnSnackCase(nameof(DateOfBirth), TypeName = "DATE")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Nguyên quán
        /// </summary>
        [ColumnSnackCase(nameof(PlaceOfOrigin), TypeName = "VARCHAR2")]
        [MaxLength(512)]
        public string PlaceOfOrigin { get; set; }

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
