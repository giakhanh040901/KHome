using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Các file sơ đồ thiết kế của căn hộ
    /// </summary>
    [Table("RST_PRODUCT_ITEM_DESIGN_DIAGRAM_FILE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(ProductItemId), IsUnique = false, Name = "IX_RST_PRODUCT_ITEM_DESIGN_DIAGRAM_FILE")]
    public class RstProductItemDesignDiagramFile
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProductItemDesignDiagramFile).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        /// <summary>
        /// Id căn hộ
        /// </summary>
        [ColumnSnackCase(nameof(ProductItemId))]
        public int ProductItemId { get; set; }
        /// <summary>
        /// Tên file
        /// </summary>
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }
        /// <summary>
        /// Đường dẫn file
        /// </summary>
        [ColumnSnackCase(nameof(FileUrl))]
        [MaxLength(1024)]
        public string FileUrl { get; set; }
    }
}
