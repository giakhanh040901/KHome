﻿using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_CONFIG_CONTRACT_CODE", Schema = DbSchemas.EPIC)]
    public class InvestConfigContractCode : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(InvestConfigContractCode)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [Required]
        [MaxLength(128)]
        [ColumnSnackCase(nameof(Name))]
        [Comment("Tên config")]
        public string Name { get; set; }

        [NotMapped]
        [Obsolete("bỏ")]
        public string CustomerType { get; set; }

        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

        [MaxLength(512)]
        [ColumnSnackCase(nameof(Description))]
        [Comment("Mô tả")]
        public string Description { get; set; }

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
