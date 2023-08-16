using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class ViewManagerInvestorBaseDto
    {
        public int Id { get; set; }
        public int InvestorId { get; set; }
		public int InvestorGroupId { get; set; }
		public string AccountNo { get; set; }
		public int AccountType { get; set; }
		public string Name { get; set; }
		public string NameEn { get; set; }
		public string ShortName { get; set; }
		public string Bori { get; set; }
		public string Dorf { get; set; }
		public string Sex { get; set; }
		public DateTime? BirthDate { get; set; }
		public string EduLevel { get; set; }
		public string Occupation { get; set; }
		public string Address { get; set; }
		public string ContactAddress { get; set; }
		public string Nationality { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public string TaxCode { get; set; }
		public int TradingProviderId { get; set; }
		public int? SaleId { get; set; }
		public string Description { get; set; }
		public string PinCode { get; set; }
		public string Isonline { get; set; }
        public string IsCheck { get; set; }
        public string IdFrontImageUrl { get; set; }
		public string IdBackImageUrl { get; set; }
		public string FaceImageUrl { get; set; }
		public string LivenessVideoUrl { get; set; }
		public string SignatureImageUrl { get; set; }
		public string AccountStatus { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
        public string IsUpdate { get; set; }
        public string CifCode { get; set; }
        public string IsConfirmed { get; set; }
		public string IsProf { get; set; }

		public string ProfFileUrl { get; set; }

		public DateTime? ProfDueDate { get; set; }

		public DateTime? ProfStartDate { get; set; }
        public string AvatarImageUrl { get; set; }
        public ViewIdentificationDto DefaultIdentification { get; set; }
		public InvestorBankAccount DefaultBank { get; set; }
		public InvestorContactAddress DefaultContactAddress { get; set; }
        public ViewInvestorStockDto DefaultStock { get; set; }
        public int ProfStatus { get; set; }
        public string ReferralCodeSelf { get; set; }
		public string ReferralCode { get; set; }
        public DateTime? ReferralDate { get; set; }
        public int? SecurityCompany { get; set; }
        public string StockTradingAccount { get; set; }
        public string RepresentativePhone { get; set; }
        public string RepresentativeEmail { get; set; }
        public ViewManagerInvestorTemporaryDto ReferralInvestor { get; set; }
        public BusinessCustomerDto ReferralBusinessCustomer { get; set; }
        public int? Source { get; set; }
		public int? ApproveId { get; set; }
        public string FaceImageUrl1 { get; set; }
        public string FaceImageUrl2 { get; set; }
        public string FaceImageUrl3 { get; set; }
        public string FaceImageUrl4 { get; set; }
        public double? FaceImageSimilarity { get; set; }
        public double? FaceImageSimilarity1 { get; set; }
        public double? FaceImageSimilarity2 { get; set; }
        public double? FaceImageSimilarity3 { get; set; }
        public double? FaceImageSimilarity4 { get; set; }
		/// <summary>
		/// Tổng điểm
		/// </summary>
        public int? LoyTotalPoint { get; set; }
		/// <summary>
		/// Điểm hiện tại
		/// </summary>
        public int? LoyCurrentPoint { get; set; }
		/// <summary>
		/// Điểm đã tiêu
		/// </summary>
        public int? LoyConsumePoint { get; set; }

		/// <summary>
		/// Id rank
		/// </summary>
        public int? RankId { get; set; }

		/// <summary>
		/// Tên rank
		/// </summary>
        public string RankName { get; set; }

    }
}
