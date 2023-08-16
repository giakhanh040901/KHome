using EPIC.Utils.Attributes;
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
    [Table("EP_INVESTOR_TEMP", Schema = DbSchemas.EPIC)]
    public class InvestorTemp : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_INVESTOR_TEMP";
        [Key]
		[Column("INVESTOR_ID")]
		public int InvestorId { get; set; }

		[Column("INVESTOR_GROUP_ID")]
		public int? InvestorGroupId { get; set; }

		[Column("NAME")]
		public string Name { get; set; }

		[Column("ADDRESS")]
		public string Address { get; set; }

		[Column("CONTACT_ADDRESS")]
		public string ContactAddress { get; set; }

		[Column("NATIONALITY")]
		public string Nationality { get; set; }

		[Column("PHONE")]
		public string Phone { get; set; }

		[Column("FAX")]
		public string Fax { get; set; }

		[Column("MOBILE")]
		public string Mobile { get; set; }

		[Column("EMAIL")]
		public string Email { get; set; }

		[Column("TAX_CODE")]
		public string TaxCode { get; set; }

		[Column("STATUS")]
		public string Status { get; set; }

		[Column("PIN_CODE")]
		public string PinCode { get; set; }

		[Column("ISONLINE")]
		public string Isonline { get; set; }

		[Column("FACE_IMAGE_URL")]
		public string FaceImageUrl { get; set; }

		[Column("ACCOUNT_STATUS")]
		public string AccountStatus { get; set; }

		[Column("IS_PROF")]
		public string IsProf { get; set; }

		[Column("PROF_FILE_URL")]
		public string ProfFileUrl { get; set; }

		[Column("PROF_DUE_DATE")]
		public DateTime? ProfDueDate { get; set; }

		[Column("PROF_START_DATE")]
		public DateTime? ProfStartDate { get; set; }

		[Column("REGISTER_TYPE")]
		public string RegisterType { get; set; }
		[Column("REGISTER_SOURCE")]
		public string RegisterSource { get; set; }

		[Column("CREATED_BY")]
		public string CreatedBy { get; set; }

		[Column("CREATED_DATE")]
		public DateTime? CreatedDate { get; set; }

		[Column("MODIFIED_BY")]
		public string ModifiedBy { get; set; }

		[Column("MODIFIED_DATE")]
		public DateTime? ModifiedDate { get; set; }

		[Column("DELETED")]
		public string Deleted { get; set; }

        [Column("SOURCE")]
        public int? Source { get; set; }

        [Column("REPRESENTATIVE_PHONE")]
        public string RepresentativePhone { get; set; }

        [Column("REPRESENTATIVE_EMAIL")]
        public string RepresentativeEmail { get; set; }

        [Column("REFERRAL_CODE_SELF")]
        public string ReferralCodeSelf { get; set; }

		[ColumnSnackCase(nameof(TradingProviderId))]
		public int? TradingProviderId { get; set; }
	}
}
