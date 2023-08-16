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
    /// <summary>
    /// Tỉnh / Thành phố
    /// </summary>
    [Table("PROVINCES", Schema = DbSchemas.EPIC)]
    public class Province
	{
        [Key]
        [Column("CODE")]
		public string Code { get; set; }

		[Column("NAME")]
		public string Name { get; set; }

		[Column("NAME_EN")]
		public string NameEn { get; set; }

		[Column("FULL_NAME")]
		public string FullName { get; set; }

		[Column("FULL_NAME_EN")]
		public string FullNameEn { get; set; }

		[Column("CODE_NAME")]
		public string CodeName { get; set; }

		[Column("ADMINISTRATIVE_UNIT_ID")]
		public int AdministrativeUnitId { get; set; }

		[Column("ADMINISTRATIVE_REGION_ID")]
		public int AdministrativeRegionId { get; set; }
	}
}
