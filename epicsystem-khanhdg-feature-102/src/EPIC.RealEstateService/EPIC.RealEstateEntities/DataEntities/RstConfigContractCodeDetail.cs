using DocumentFormat.OpenXml.Wordprocessing;
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

namespace EPIC.RealEstateEntities.DataEntities
{
    [Table("RST_CONFIG_CONTRACT_CODE_DETAIL", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(ConfigContractCodeId), IsUnique = false, Name = "IX_RST_CONFIG_CONTRACT_CODE_DETAIL")]
    public class RstConfigContractCodeDetail
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(RstConfigContractCodeDetail)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ConfigContractCodeId))]
        public int ConfigContractCodeId { get; set; }

        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }  

        [Required]
        [MaxLength(50)]
        [ColumnSnackCase(nameof(Key), TypeName = "VARCHAR2")]
        public string Key { get; set; }

        [MaxLength(20)]
        [ColumnSnackCase(nameof(Value), TypeName = "VARCHAR2")]
        public string Value { get; set; }
    }
}
