using EPIC.Utils.Attributes;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_PRODUCT_TYPE", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Loai hinh du an san pham tich luy")]
    public class GarnerProductType : ICreatedBy
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerProductType)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProductId))]
        [Comment("San pham")]
        public int ProductId { get; set; }

        [ColumnSnackCase(nameof(Type))]
        [Comment("Loai hinh du an (1:Nha rieng, 2: Can ho chung cu, 3:Nha pho, biet thu du an, 4: Dat nen du an, 5: Biet thu nghi duong, 6: Condotel, 7: Shophouse, 8: Officetel)")]
        public int Type { get; set; }

        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }
    }
}
