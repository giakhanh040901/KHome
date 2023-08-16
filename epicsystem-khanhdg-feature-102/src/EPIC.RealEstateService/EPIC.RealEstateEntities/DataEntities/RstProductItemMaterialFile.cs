using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// các file của vật liệu căn hộ
    /// </summary>
    [Table("RST_PRODUCT_ITEM_MATERIAL_FILE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(ProductItemId), IsUnique = false, Name = "IX_RST_PRODUCT_ITEM_MATERIAL_FILE")]
    public class RstProductItemMaterialFile
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProductItemMaterialFile).ToSnakeUpperCase()}";
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
