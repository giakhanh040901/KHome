using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataAccess.Base.Entities
{
    [Table("DEFERROR", Schema = DbSchemas.EPIC)]
    public class DefError
    {
        [Key]
        [Required]
        [ColumnSnackCase(nameof(ErrCode), TypeName = "NUMBER")]
        public int ErrCode { get; set; }

        [Required]
        [ColumnSnackCase(nameof(ErrName))]
        public string ErrName { get; set; }

        [ColumnSnackCase(nameof(ErrMessage))]
        public string ErrMessage { get; set; }
    }
}
