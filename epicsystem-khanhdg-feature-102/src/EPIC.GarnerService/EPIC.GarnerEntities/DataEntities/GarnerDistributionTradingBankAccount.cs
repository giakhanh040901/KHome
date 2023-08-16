using EPIC.Entities;
using EPIC.Entities.DataEntities;
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
    [Table("GAN_DISTRIBUTION_TRADING_BANK_ACCOUNT", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Tai khoan thu huong ban theo ky han")]
    public class GarnerDistributionTradingBankAccount : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{nameof(GarnerDistributionTradingBankAccount).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Id")]
        public int Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(DistributionId))]
        [Comment("Id San pham phan phoi")]
        public int DistributionId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(BusinessCustomerBankAccId))]
        [Comment("Id ngan hang thu huong")]
        public int BusinessCustomerBankAccId { get; set; }

        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Trang thai kich hoat")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(Type))]
        [Comment("Loai ngan hang: 1 thu, 2 chi")]
        public int Type { get; set; }

        [ColumnSnackCase(nameof(IsAuto), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string IsAuto { get; set; }
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
