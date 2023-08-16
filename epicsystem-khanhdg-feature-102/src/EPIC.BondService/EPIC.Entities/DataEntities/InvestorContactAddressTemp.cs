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
    [Table("EP_INVESTOR_CONTACT_ADD_TEMP", Schema = DbSchemas.EPIC)]
    public class InvestorContactAddressTemp : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_INVESTOR_CONTACT_ADD_TEMP";
        [Key]
        [Column("CONTACT_ADDRESS_ID")]
		public int ContactAddressId { get; set; }

		[Column("INVESTOR_ID")]
		public int InvestorId { get; set; }

		[Column("CONTACT_ADDRESS")]
		public string ContactAddress { get; set; }

		[Column("DETAIL_ADDRESS")]
		public string DetailAddress { get; set; }

		[Column("PROVINCE_CODE")]
		public string ProvinceCode { get; set; }

		[Column("DISTRICT_CODE")]
		public string DistrictCode { get; set; }

		[Column("WARD_CODE")]
		public string WardCode { get; set; }

		[Column("IS_DEFAULT")]
		public string IsDefault { get; set; }

		[Column("CREATED_DATE")]
		public DateTime? CreatedDate { get; set; }

		[Column("CREATED_BY")]
		public string CreatedBy { get; set; }

		[Column("MODIFIED_DATE")]
		public DateTime? ModifiedDate { get; set; }

		[Column("MODIFIED_BY")]
		public string ModifiedBy { get; set; }

		[Column("DELETED")]
		public string Deleted { get; set; }

		[Column("INVESTOR_GROUP_ID")]
		public int? InvestorGroupId { get; set; }

        [Column("REFER_ID")]
        public int? ReferId { get; set; }
	}
}
