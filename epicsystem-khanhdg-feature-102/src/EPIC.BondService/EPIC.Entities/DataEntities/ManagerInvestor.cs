using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    public class ManagerInvestor : IFullAudited
    {
		// INVESTOR
		[Column(Name = "INVESTOR_ID")]
		public int InvestorId { get; set; }

		[Column(Name = "INVESTOR_ACCOUNT_NO")]
		public string InvestorAccountNo { get; set; }
		[Column(Name = "INVESTOR_ACCOUNT_TYPE")]
		public int InvestorAccountType { get; set; }
		[Column(Name = "INVESTOR_NAME")]
		public string InvestorName { get; set; }
		[Column(Name = "INVESTOR_SEX")]
		public string InvestorSex { get; set; }
		[Column(Name = "INVESTOR_BIRTH_DATE")]
		public DateTime InvestorBirthDate { get; set; }
		[Column(Name = "INVESTOR_ADDRESS")]
		public string InvestorAddress { get; set; }
		[Column(Name = "INVESTOR_NAT")]
		public string InvestorNationality { get; set; }
		[Column(Name = "INVESTOR_PHONE")]
		public string InvestorPhone { get; set; }
		[Column(Name = "INVESTOR_FAX")]
		public string InvestorFax { get; set; }
		[Column(Name = "INVESTOR_MOBILE")]
		public string InvestorMobile { get; set; }
		[Column(Name = "INVESTOR_EMAIL")]
		public string InvestorEmail { get; set; }
		[Column(Name = "INVESTOR_STATUS")]
		public string InvestorStatus { get; set; }
		[Column(Name = "CREATED_BY")]
		public string CreatedBy { get; set; }
		[Column(Name = "CREATED_DATE")]
		public DateTime? CreatedDate { get; set; }
		[Column(Name = "MODIFIED_BY")]
		public string ModifiedBy { get; set; }
		[Column(Name = "MODIFIED_DATE")]
		public DateTime? ModifiedDate { get; set; }
		[Column(Name = "DELETED")]
		public string Deleted { get; set; }
		// IDENTIFICATION
		[Column(Name = "IDEN_ID")]
		public int IdentificationId { get; set; }
		[Column(Name = "IDEN_ID_TYPE")]
		public string IdentificationIdType { get; set; }
		[Column(Name = "IDEN_ID_NO")]
		public string IdentificationIdNo { get; set; }
		[Column(Name = "IDEN_FULLNAME")]
		public string IdentificationFullname { get; set; }
		[Column(Name = "IDEN_DOB")]
		public DateTime IdentificationDateOfBirth { get; set; }
		[Column(Name = "IDEN_NAT")]
		public string IdentificationNationality { get; set; }
		[Column(Name = "IDEN_PER_IDENTIFICATION")]
		public string IdentificationPerionsalIdentification { get; set; }
		[Column(Name = "IDEN_ID_ISSUER")]
		public string IdentificationIdIssuer { get; set; }
		[Column(Name = "IDEN_ID_DATE")]
		public string IdentificationIdDate { get; set; }
		[Column(Name = "IDEN_ID_EXPIRED_DATE")]
		public DateTime IdentificationIdExpiredDate { get; set; }
		[Column(Name = "IDEN_PLACE_ORIGIN")]
		public string IdentificationPlaceOfOrigin { get; set; }
		[Column(Name = "IDEN_PLACE_RESIDENCE")]
		public string IdentificationPlaceOfResidence { get; set; }
		[Column(Name = "IDEN_ID_FRONT_IMAGE")]
		public string IdentificationIdFrontImageUrl { get; set; }
		[Column(Name = "IDEN_ID_BACK_IMAGE")]
		public string IdentificationIdBackImageUrl { get; set; }
		[Column(Name = "IDEN_ID_EXTRA_IMAGE")]
		public string IdentificationIdExtraImageUrl { get; set; }
		[Column(Name = "IDEN_FACE_IMAGE")]
		public string IdentificationFaceImageUrl { get; set; }
		[Column(Name = "IDEN_FACE_VIDEO_URL")]
		public string IdentificationFaceVideoUrl { get; set; }
		[Column(Name = "IDEN_STATUS_APPROVED")]
		public string IdentificationStatus { get; set; }
		[Column(Name = "IDEN_IS_DEFAULT")]
		public string IdentificationIsDefault { get; set; }
		[Column(Name = "IDEN_IS_VERIFIED_FACE")]public string IsVerifiedIdentification { get; set; }
		[Column(Name = "IS_VERIFIED_IDENTIFICATION")]
		public string IsVerifiedFace { get; set; }
		[Column(Name = "IDEN_STATUS_APPROVED")]
		public string IdentificationStatusApproved { get; set; }
		
	}

}
