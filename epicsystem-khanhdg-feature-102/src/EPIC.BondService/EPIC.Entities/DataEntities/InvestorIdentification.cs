//using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
	/// <summary>
	/// Giấy tờ thật
	/// </summary>
    [Table("EP_INVESTOR_IDENTIFICATION", Schema = DbSchemas.EPIC)]
    public class InvestorIdentification : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_INVESTOR_IDENTIFICATION";

        [Key]
		[Column("ID")]
		public int Id { get; set; }
		[Column("INVESTOR_ID")]
		public int InvestorId { get; set; }
        [JsonIgnore]
        public Investor Investor { get; set; }
		[Column("ID_TYPE")]
		public string IdType { get; set; }
		[Column("ID_NO")]
		public string IdNo { get; set; }
		[Column("FULLNAME")]
		public string Fullname { get; set; }
		[Column("DATE_OF_BIRTH")]
		public DateTime? DateOfBirth { get; set; }
		[Column("NATIONALITY")]
		public string Nationality { get; set; }
		[Column("PERSONAL_IDENTIFICATION")]
		public string PersonalIdentification { get; set; }
		[Column("ID_ISSUER")]
		public string IdIssuer { get; set; }
		[Column("ID_DATE")]
		public DateTime? IdDate { get; set; }
		[Column("ID_EXPIRED_DATE")]
		public DateTime? IdExpiredDate { get; set; }
		[Column("PLACE_OF_ORIGIN")]
		public string PlaceOfOrigin { get; set; }
		[Column("PLACE_OF_RESIDENCE")]
		public string PlaceOfResidence { get; set; }
		[Column("ID_FRONT_IMAGE_URL")]
		public string IdFrontImageUrl { get; set; }
		[Column("ID_BACK_IMAGE_URL")]
		public string IdBackImageUrl { get; set; }
		[Column("ID_EXTRA_IMAGE_URL")]
		public string IdExtraImageUrl { get; set; }
		[Column("FACE_IMAGE_URL")]
		public string FaceImageUrl { get; set; }
		[Column("FACE_VIDEO_URL")]
		public string FaceVideoUrl { get; set; }
		[Column("STATUS")]
		public string Status { get; set; }
		[Column("IS_DEFAULT")]
		public string IsDefault { get; set; }

		[Column("IS_VERIFIED_FACE")]
		public string IsVerifiedFace { get; set; }

		[Column("IS_VERIFIED_IDENTIFICATION")]
		public string IsVerifiedIdentification { get; set; }

		[Column("STATUS_APPROVED")]
		public string StatusApproved { get; set; }

		[Column("INVESTOR_GROUP_ID")]
		public int InvestorGroupId { get; set; }

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

		[Column("SEX")]
        public string Sex { get; set; }

        [Column("EKYC_INCORRECT_FIELDS")]
        public string EkycIncorrectFields { get; set; }

		[Column("EKYC_INFO_IS_CONFIRMED")]
        public string EkycInfoIsConfirmed { get; set; }

		[Column("REFER_ID")]
        public int? ReferId { get; set; }
	}
}
