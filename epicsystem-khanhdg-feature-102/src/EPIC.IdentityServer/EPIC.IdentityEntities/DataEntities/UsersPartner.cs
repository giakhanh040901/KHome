using EPIC.Entities;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;

namespace EPIC.IdentityEntities.DataEntities
{
    [Table("USERS_PARTNER", Schema = DbSchemas.EPIC)]
    public class UsersPartner : IFullAudited
    {
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(UserId))]
        public int UserId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

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
        public string Deleted { get; set; } = YesNo.NO;
        #endregion
    }
}
