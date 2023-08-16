using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Các thuộc tính trong dự án dựa theo DefineAttrGroup
    /// </summary>
    [Table("RST_PROJECT_ATTRIBUTE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(ProjectId), nameof(DefineAttributeId), IsUnique = true, Name = "IX_RST_PROJECT_ATTRIBUTE")]
    public class RstProjectAttribute
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectAttribute).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        [ColumnSnackCase(nameof(DefineAttributeId))]
        public int DefineAttributeId { get; set; }

        /// <summary>
        /// Trạng thái Y, N để ẩn hiện thuộc tính có show app
        /// </summary>
        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(IsShowApp), TypeName = "VARCHAR2")]
        public string IsShowApp { get; set; }
    }

    /// <summary>
    /// Giá trị của thuộc tính
    /// </summary>
    [Table("RST_PROJECT_VALUE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(AttributeId), nameof(DefineAttrValueId), IsUnique = false, Name = "IX_RST_PROJECT_VALUE")]
    public class RstProjectValue
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectValue).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id thuộc tính
        /// </summary>
        [ColumnSnackCase(nameof(AttributeId))]
        public int AttributeId { get; set; }

        [ColumnSnackCase(nameof(NumberValue), TypeName = "NUMBER(18, 6)")]
        public decimal? NumberValue { get; set; }

        [MaxLength(512)]
        [ColumnSnackCase(nameof(StringValue))]
        public string StringValue { get; set; }

        [MaxLength(512)]
        [ColumnSnackCase(nameof(UrlValue), TypeName = "VARCHAR2")]
        public string UrlValue { get; set; }

        /// <summary>
        /// Lưu cho thuộc tính chọn 1
        /// </summary>
        [ColumnSnackCase(nameof(DefineAttrValueId))]
        public int? DefineAttrValueId { get; set; }
    }

    /// <summary>
    /// Giá trị của thuộc tính đa trị
    /// </summary>
    [Table("RST_PROJECT_VALUE_MULTIPLE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(AttributeId), nameof(DefineAttrValueId), IsUnique = false, Name = "IX_RST_PROJECT_VALUE_MULTIPLE")]
    public class RstProjectValueMultiple
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectValueMultiple).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id thuộc tính
        /// </summary>
        [ColumnSnackCase(nameof(AttributeId))]
        public int AttributeId { get; set; }

        [MaxLength(512)]
        [ColumnSnackCase(nameof(StringValue))]
        public string StringValue { get; set; }

        [MaxLength(512)]
        [ColumnSnackCase(nameof(UrlValue), TypeName = "VARCHAR2")]
        public string UrlValue { get; set; }

        /// <summary>
        /// Lưu cho thuộc tính chọn nhiều
        /// </summary>
        [ColumnSnackCase(nameof(DefineAttrValueId))]
        public int? DefineAttrValueId { get; set; }

        /// <summary>
        /// Tiêu đề giá trị, dùng trong trường hợp tiêu đề hình ảnh
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(Title))]
        public string Title { get; set; }

        /// <summary>
        /// Trạng thái A, D
        /// </summary>
        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }
    }
}
