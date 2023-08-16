using EPIC.Utils.ConstantVariables.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EPIC.Entities.DataEntities
{
    [Table("WARDS", Schema = DbSchemas.EPIC)]
    public class Ward
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

        [Column("DISTRICT_CODE")]
        public string DistrictCode { get; set; }

        [Column("ADMINISTRATIVE_UNIT_ID")]
        public int AdministrativeUnitId { get; set; }
    }
}
