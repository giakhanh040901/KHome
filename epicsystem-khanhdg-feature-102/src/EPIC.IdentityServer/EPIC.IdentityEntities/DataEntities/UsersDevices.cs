using EPIC.Entities;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.IdentityEntities.DataEntities
{
    [Table("USERS_DEVICES", Schema = DbSchemas.EPIC)]
    public class UsersDevices : ICreatedBy, ISoftDelted
    {
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(UserId))]
        public int UserId { get; set; }

        [ColumnSnackCase(nameof(DeviceId))]
        public string DeviceId { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public bool? Status { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
