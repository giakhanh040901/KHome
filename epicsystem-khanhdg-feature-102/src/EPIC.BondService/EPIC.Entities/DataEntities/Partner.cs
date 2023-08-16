using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_CORE_PARTNER", Schema = DbSchemas.EPIC)]
    public class Partner : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}PARTNER";

        [Key]
        [ColumnSnackCase(nameof(PartnerId))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PartnerId { get; set; }

        [ColumnSnackCase(nameof(Code))]
        [MaxLength(128)]
        public string Code { get; set; }

        [ColumnSnackCase(nameof(Name))]
        [MaxLength(256)]
        public string Name { get; set; }

        [ColumnSnackCase(nameof(ShortName))]
        [MaxLength(128)]
        public string ShortName { get; set; }

        [ColumnSnackCase(nameof(Address))]
        [MaxLength(512)]
        public string Address { get; set; }

        [ColumnSnackCase(nameof(Phone))]
        [MaxLength(50)]
        public string Phone { get; set; }

        [ColumnSnackCase(nameof(Mobile))]
        [MaxLength(50)]
        public string Mobile { get; set; }

        [ColumnSnackCase(nameof(Email))]
        [MaxLength(128)]
        public string Email { get; set; }

        [ColumnSnackCase(nameof(TaxCode))]
        [MaxLength(50)]
        public string TaxCode { get; set; }

        [ColumnSnackCase(nameof(LicenseDate), TypeName = "DATE")]
        public DateTime? LicenseDate { get; set; }

        [ColumnSnackCase(nameof(LicenseIssuer))]
        [MaxLength(512)]
        public string LicenseIssuer { get; set; }

        [ColumnSnackCase(nameof(Capital))]
        public double? Capital { get; set; }

        [ColumnSnackCase(nameof(RepName))]
        [MaxLength(128)]
        public string RepName { get; set; }

        [ColumnSnackCase(nameof(RepPosition))]
        [MaxLength(128)]
        public string RepPosition { get; set; }

        [ColumnSnackCase(nameof(Status))]
        [MaxLength(1)]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(TradingAddress))]
        [MaxLength(512)]
        public string TradingAddress { get; set; }

        [ColumnSnackCase(nameof(Nation))]
        [MaxLength(256)]
        public string Nation { get; set; }

        [ColumnSnackCase(nameof(DecisionNo))]
        [MaxLength(128)]
        public string DecisionNo { get; set; }

        [ColumnSnackCase(nameof(DecisionDate), TypeName = "DATE")]
        public DateTime? DecisionDate { get; set; }

        [ColumnSnackCase(nameof(NumberModified))]
        public int? NumberModified { get; set; }

        [ColumnSnackCase(nameof(DateModified), TypeName = "DATE")]
        public DateTime? DateModified { get; set; }

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

        [ColumnSnackCase(nameof(Deleted))]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
