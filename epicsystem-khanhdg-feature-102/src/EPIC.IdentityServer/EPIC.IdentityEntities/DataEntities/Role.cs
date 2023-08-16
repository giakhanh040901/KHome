using EPIC.Entities;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.IdentityEntities.DataEntities
{
    [Table("P_ROLE", Schema = DbSchemas.EPIC)]
    public class Role : IFullAudited
    {
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int? PartnerId { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Loại Role
        /// <see cref="RoleTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(RoleType))]
        public int RoleType { get; set; }

        /// <summary>
        /// Trạng thái A/D
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }

        /// <summary>
        /// Quyền thuộc Web nào
        /// <see cref="PermissionInWebs"/>
        /// </summary>
        [ColumnSnackCase(nameof(PermissionInWeb))]
        public int? PermissionInWeb { get; set; }

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
