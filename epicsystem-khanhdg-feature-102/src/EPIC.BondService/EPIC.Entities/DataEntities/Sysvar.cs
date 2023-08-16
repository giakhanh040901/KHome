using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("SYSVAR", Schema = DbSchemas.EPIC)]
    [Keyless]
    public class SysVar
    {
        [Column("GRNAME")]
        public string GrName { get; set; }

        [Column("VARNAME")]
        public string VarName { get; set; }

        [Column("VARVALUE")]
        public string VarValue { get; set; }

        [Column("VARDESC")]
        public string VarDesc { get; set; }
    }
}
