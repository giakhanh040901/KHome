using EPIC.Utils;
using EPIC.Utils.Validation;
using EPIC.Utils.Validation.Investor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class UpdateIdentificationDto
    {
		public int Id { get; set; }
		public string IdType { get; set; }
		public string IdNo { get; set; }
		public string Fullname { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Nationality { get; set; }
		public string PersonalIdentification { get; set; }
		public string IdIssuer { get; set; }
		public DateTime? IdDate { get; set; }
		public DateTime? IdExpiredDate { get; set; }
		public string PlaceOfOrigin { get; set; }

		[RequiredPlaceOfOriginWithIdType(AllowEmptyStrings = false, ErrorMessage = "Địa chỉ thường trú không được bỏ trống")]
		public string PlaceOfResidence { get; set; }
		public string IdFrontImageUrl { get; set; }
		public string IdBackImageUrl { get; set; }
		public string IdExtraImageUrl { get; set; }
		public string FaceImageUrl { get; set; }
		public string FaceVideoUrl { get; set; }
		public string Status { get; set; }

        [StringRange(AllowableValues = new string[] { Genders.MALE, Genders.FEMALE })]
		public string Sex { get; set; }

		public string IsVerifiedIdentification { get; set; }
		public string IsVerifiedFace { get; set; }
		public int InvestorGroupId { get; set; }
		public int InvestorId { get; set; }
	}
}
