using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_RATING", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Rating sản phẩm")]
    public class GarnerRating : ICreatedBy, IModifiedBy
    {
        public static string SEQ { get; } = $"SEQ_{nameof(GarnerRating).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        public long OrderId { get; set; }

        [ColumnSnackCase(nameof(Rate))]
        public int Rate { get; set; }

        [ColumnSnackCase(nameof(ProductExperience))]
        public int ProductExperience { get; set; }

        [MaxLength(1024)]
        [ColumnSnackCase(nameof(Feedback))]
        public string Feedback { get; set; }

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
        #endregion
    }
}
