using DocumentFormat.OpenXml.CustomXmlSchemaReferences;
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

namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_PARTNER_MSB_PREFIX_ACCOUNT", Schema = DbSchemas.EPIC)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(PartnerBankAccountId), IsUnique = false, Name = "IX_PARTNER_MSB_PREFIX_ACCOUNT")]
    public class PartnerMsbPrefixAccount : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(PartnerMsbPrefixAccount)).ToSnakeUpperCase()}";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ColumnSnackCase(nameof(Id))]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        [ColumnSnackCase(nameof(PartnerBankAccountId))]
        public int PartnerBankAccountId { get; set; }

        [ColumnSnackCase(nameof(PrefixMsb), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string PrefixMsb { get; set; }

        [ColumnSnackCase(nameof(MId))]
        [MaxLength(50)]
        public string MId { get; set; }

        [ColumnSnackCase(nameof(TId))]
        [MaxLength(50)]
        public string TId { get; set; }

        [ColumnSnackCase(nameof(AccessCode))]
        [MaxLength(50)]
        public string AccessCode { get; set; }

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
