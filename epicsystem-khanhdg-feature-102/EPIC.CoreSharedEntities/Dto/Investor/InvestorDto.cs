using EPIC.Entities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class InvestorDto
    {
		public int InvestorId { get; set; }
		public string AccountNo { get; set; }
		public int AccountType { get; set; }
		public string Name { get; set; }
		public string NameEn { get; set; }
		public string ShortName { get; set; }
		public string Bori { get; set; }
		public string Dorf { get; set; }
		public string Sex { get; set; }
		public DateTime BirthDate { get; set; }
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
		public string IdNo { get; set; }
		public DateTime IdDate { get; set; }
		public string IdIssuer { get; set; }
		public string FoundationNo { get; set; }
		public DateTime FoundationDate { get; set; }
		public string FoundationIssuer { get; set; }
		public string LicenseNo { get; set; }
		public DateTime LicenseDate { get; set; }
		public string LicenseIssuer { get; set; }
		public string RepName { get; set; }
		public string RepIdNo { get; set; }
		public DateTime RepIdDate { get; set; }
		public string RepIdIssuer { get; set; }
		public string RepPosition { get; set; }
		public int Priority { get; set; }
		public string Brid { get; set; }
		public int BrokerId { get; set; }
		public string Description { get; set; }
		public string Status { get; set; }
		public string Isonline { get; set; }
		public string IdFrontImageUrl { get; set; }
		public string IdBackImageUrl { get; set; }
		public string FaceImageUrl { get; set; }
		public string LivenessVideoUrl { get; set; }
		public string SignatureImageUrl { get; set; }
		public string AccountStatus { get; set; }
		public string IsEcontractSign { get; set; }
		public string EContractUrl { get; set; }
		public DateTime EcSignDate { get; set; }
		public string IsProf { get; set; }
		public DateTime ProfDueDate { get; set; }
		public string RegSource { get; set; }
		public string EditedInfo { get; set; }
		public string IsOpnaccSigned { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime ModifiedDate { get; set; }
		public string CifCode { get; set; }
		public string AvatarImageUrl { get; set; }
		public string ReferralCodeSelf { get; set; }
		public int ProfStatus { get; set; }
        public List<InvestorBankAccount> ListBank { get; set; }
		public InvestorIdentificationDto InvestorIdentification { get; set; }
	}
}
