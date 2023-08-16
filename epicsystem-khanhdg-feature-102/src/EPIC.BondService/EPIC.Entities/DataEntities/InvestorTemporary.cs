using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    /// <summary>
    /// ENTITY CHO TEMPORAY THAT VA TEMP
    /// VI CA 2 KHAC NHAU O KIEU DU LIEU CUA STATUS
    /// </summary>
    //[NotMapped]
    public class InvestorTemporary : IFullAudited
	{
		public int InvestorId { get; set; }

		public string AccountNo { get; set; }
		public int AccountType { get; set; }
		public string Name { get; set; }
		public string Sex { get; set; }
		public DateTime? BirthDate { get; set; }

		public string Address { get; set; }

		public string ContactAddress { get; set; }

		public string Nationality { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }

		public string TaxCode { get; set; }

		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string Deleted { get; set; }
		public string InvestorStatus { get; set; }
		public string ApprovedBy { get; set; }
		public DateTime ApprovedDate { get; set; }
		public int InvestorGroupId { get; set; }
		public int TradingProviderId { get; set; }
		public string FaceImageUrl { get; set; }
		public string IsUpdate { get; set; }
		public string EkycInfoIsConfirmed { get; set; }
		public string IsProf { get; set; }

		public string ProfFileUrl { get; set; }

		public DateTime? ProfDueDate { get; set; }

		public DateTime? ProfStartDate { get; set; }
		public string AvatarImageUrl { get; set; }
        public string CifCode { get; set; }
		public string ReferralCodeSelf { get; set; }
		public string ReferralCode { get; set; }
		public int ProfStatus { get; set; }
        public string IsVerifiedEmail { get; set; }
        public string IsVerifiedIdentification { get; set; }
        public DateTime? ReferralDate { get; set; }
		public int? SecurityCompany { get; set; }
		public string StockTradingAccount { get; set; }
		public string RepresentativePhone { get; set; }
        public string RepresentativeEmail { get; set; }
        public int? Source { get; set; }
		public int? ApproveId { get; set; }
		public int? Step { get; set; }
        public string FaceImageUrl1 { get; set; }
        public string FaceImageUrl2 { get; set; }
        public string FaceImageUrl3 { get; set; }
        public string FaceImageUrl4 { get; set; }
        public double? FaceImageSimilarity { get; set; }
        public double? FaceImageSimilarity1 { get; set; }
        public double? FaceImageSimilarity2 { get; set; }
        public double? FaceImageSimilarity3 { get; set; }
        public double? FaceImageSimilarity4 { get; set; }
        public int? LoyTotalPoint { get; set; }
        public int? LoyCurrentPoint { get; set; }
    }
}
