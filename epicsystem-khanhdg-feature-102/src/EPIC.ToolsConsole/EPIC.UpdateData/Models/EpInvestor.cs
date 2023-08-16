using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvestor
    {
        public decimal InvestorId { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string ShortName { get; set; }
        public string Bori { get; set; }
        public string Dorf { get; set; }
        public string Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Occupation { get; set; }
        public string Address { get; set; }
        public string ContactAddress { get; set; }
        public string Nationality { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public DateTime? IdExpiredDate { get; set; }
        public string IdIssuer { get; set; }
        public string IdPlaceOfResidence { get; set; }
        public string FoundationNo { get; set; }
        public DateTime? FoundationDate { get; set; }
        public string FoundationIssuer { get; set; }
        public string LicenseNo { get; set; }
        public DateTime? LicenseDate { get; set; }
        public string LicenseIssuer { get; set; }
        public string RepName { get; set; }
        public string RepIdNo { get; set; }
        public DateTime? RepIdDate { get; set; }
        public string RepIdIssuer { get; set; }
        public string RepPosition { get; set; }
        public decimal? Priority { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? SaleId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string PinCode { get; set; }
        public string Isonline { get; set; }
        public string FaceImageUrl { get; set; }
        public string LivenessVideoUrl { get; set; }
        public string SignatureImageUrl { get; set; }
        public string AccountStatus { get; set; }
        public string EkycStatus { get; set; }
        public byte? EkycOcrCount { get; set; }
        public string IsEcontractSign { get; set; }
        public string EContractUrl { get; set; }
        public DateTime? EcSignDate { get; set; }
        public string IsProf { get; set; }
        public string ProfFileUrl { get; set; }
        public DateTime? ProfDueDate { get; set; }
        public string RegisterType { get; set; }
        public string RegisterSource { get; set; }
        public string IsOpnaccSigned { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public decimal? InvestorGroupId { get; set; }
        public string IsCheck { get; set; }
        public string ReferralCodeSelf { get; set; }
        public string ReferralCode { get; set; }
        public DateTime? ProfStartDate { get; set; }
        public string AvatarImageUrl { get; set; }
        public string VerifyEmailCode { get; set; }
        public DateTime? VerifyEmailCodeCreatedDate { get; set; }
        public DateTime? ReferralDate { get; set; }
        public decimal? SecurityCompany { get; set; }
        public string StockTradingAccount { get; set; }
        public decimal? Source { get; set; }
        public string RepresentativePhone { get; set; }
        public string RepresentativeEmail { get; set; }
        public decimal? Step { get; set; }
        public DateTime? FinalStepDate { get; set; }
    }
}
