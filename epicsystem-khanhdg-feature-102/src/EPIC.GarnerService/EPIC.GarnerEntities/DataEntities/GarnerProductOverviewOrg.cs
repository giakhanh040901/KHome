using EPIC.Entities;
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

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_PRODUCT_OVERVIEW_ORG", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Tong quan To chuc cua San pham tich luy")]
    public class GarnerProductOverviewOrg : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerProductOverviewOrg)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int TradingProviderId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(DistributionId))]
        [Comment("Id Phan phoi san pham tich luy")]
        public int DistributionId { get; set; }

        [ColumnSnackCase(nameof(Name))]
        [MaxLength(1024)]
        [Comment("Ten to chuc")]
        public string Name { get; set; }

        [ColumnSnackCase(nameof(Code))]
        [MaxLength(512)]
        [Comment("Ma cua to chuc")]
        public string Code { get; set; }

        [ColumnSnackCase(nameof(Role))]
        [MaxLength(512)]
        [Comment("Vai tro cua to chuc")]
        public string Role { get; set; }

        [ColumnSnackCase(nameof(Icon))]
        [MaxLength(1024)]
        [Comment("Icon to chuc")]
        public string Icon { get; set; }

        [ColumnSnackCase(nameof(Url))]
        [MaxLength(1024)]
        [Comment("Duong dan")]
        public string Url { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Status))]
        [Comment("Trang thai")]
        public string Status { get; set; }

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
