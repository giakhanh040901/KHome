using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_TRADING_PROVIDER", Schema = DbSchemas.EPIC)]
    public class TradingProvider : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_TRADING_PROVIDERS";

        [Key]
        [ColumnSnackCase(nameof(TradingProviderId))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int BusinessCustomerId { get; set; }
        public BusinessCustomer BusinessCustomer { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Deleted))]
        [MaxLength(1)]
        public string Deleted { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(AliasName))]
        [MaxLength(100)]
        public string AliasName { get; set; }

        [ColumnSnackCase(nameof(Secret))]
        [MaxLength(255)]
        public string Secret { get; set; }

        [ColumnSnackCase(nameof(Server))]
        [MaxLength(255)]
        public string Server { get; set; }

        [ColumnSnackCase(nameof(Key))]
        [MaxLength(255)]
        public string Key { get; set; }

        [ColumnSnackCase(nameof(StampImageUrl))]
        [MaxLength(512)]
        public string StampImageUrl { get; set; }

        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(IsIpPayment))]
        public string IsIpPayment { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsDefaultGarner), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Chung: Chon mac dinh la Garner (Y,N) mac dinh la N")]
        public string IsDefaultGarner { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsDefaultCps), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Chung: Chon mac dinh la Company Share (Y,N) mac dinh la N")]
        public string IsDefaultCps { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsDefaultBond), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Chung: Chon mac dinh la Bond (Y,N) mac dinh la N")]
        public string IsDefaultBond { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsDefaultInvest), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Chung: Chon mac dinh la Invest (Y,N) mac dinh la N")]
        public string IsDefaultInvest { get; set; }
    }
}
