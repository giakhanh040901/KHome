using EPIC.EntitiesBase.Interfaces.Audit;
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
    [Table("EP_INVESTOR", Schema = DbSchemas.EPIC)]
    public class Investor : IFullAudited, IIpAddressAudit
    {
        public static string SEQ { get; } = $"SEQ_INVESTOR";
        public static string SEQ_GROUP { get; } = $"SEQ_INVESTOR_GROUP";
        [Key]
        [ColumnSnackCase(nameof(InvestorId))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvestorId { get; set; }

        [ColumnSnackCase(nameof(InvestorGroupId))]
        public int? InvestorGroupId { get; set; }

        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        [ColumnSnackCase(nameof(Address))]
        public string Address { get; set; }

        [ColumnSnackCase(nameof(ContactAddress))]
        public string ContactAddress { get; set; }

        [ColumnSnackCase(nameof(Nationality))]
        public string Nationality { get; set; }

        [ColumnSnackCase(nameof(Phone))]
        public string Phone { get; set; }

        [ColumnSnackCase(nameof(Fax))]
        public string Fax { get; set; }

        [ColumnSnackCase(nameof(Mobile))]
        public string Mobile { get; set; }

		//[Column("CIF_CODE")]
		[NotMapped]
        public string CifCode { get; set; }

        [NotMapped]
        public string Fullname { get; set; }

        [ColumnSnackCase(nameof(Email))]
        public string Email { get; set; }

        [ColumnSnackCase(nameof(TaxCode))]
        public string TaxCode { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(EkycStatus))]
        public string EkycStatus { get; set; }

        [ColumnSnackCase(nameof(Isonline))]
        public string Isonline { get; set; }


        [ColumnSnackCase(nameof(FaceImageUrl))]
        public string FaceImageUrl { get; set; }

        [ColumnSnackCase(nameof(AccountStatus))]
        public string AccountStatus { get; set; }

        [ColumnSnackCase(nameof(EkycOcrCount))]
        public int? EkycOcrCount { get; set; }

        [ColumnSnackCase(nameof(IsProf))]
        public string IsProf { get; set; }

        [ColumnSnackCase(nameof(ProfFileUrl))]
        public string ProfFileUrl { get; set; }

        [ColumnSnackCase(nameof(ProfDueDate))]
        public DateTime? ProfDueDate { get; set; }

        [ColumnSnackCase(nameof(ProfStartDate))]
        public DateTime? ProfStartDate { get; set; }

        [ColumnSnackCase(nameof(RegisterType))]
        public string RegisterType { get; set; }

        [ColumnSnackCase(nameof(RegisterSource))]
        public string RegisterSource { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }

        [ColumnSnackCase(nameof(IsCheck))]
        public string IsCheck { get; set; }

        [ColumnSnackCase(nameof(AvatarImageUrl))]
        public string AvatarImageUrl { get; set; }

        [ColumnSnackCase(nameof(ReferralCodeSelf))]
        public string ReferralCodeSelf { get; set; }
		
        [ColumnSnackCase(nameof(ReferralCode))]
        public string ReferralCode { get; set; }

        [ColumnSnackCase(nameof(ReferralDate))]
        public DateTime? ReferralDate { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(Source))]
        public int? Source { get; set; }

        [ColumnSnackCase(nameof(RepresentativePhone))]
        public string RepresentativePhone { get; set; }

        [ColumnSnackCase(nameof(RepresentativeEmail))]
        public string RepresentativeEmail { get; set; }

        [ColumnSnackCase(nameof(Step))]
        public int? Step { get; set; }

		[ColumnSnackCase(nameof(FinalStepDate))]
		public DateTime? FinalStepDate { get; set; }

		/// <summary>
		/// Ngày bắt đầu vào step 1
		/// </summary>
		[ColumnSnackCase(nameof(FirstStepDate))]
		public DateTime? FirstStepDate { get; set; }
		
		/// <summary>
		/// Ngày hoàn thành step 2 : Đã nhập OTP phone/email
		/// </summary>
        [ColumnSnackCase(nameof(SecondStepDate))]
        public DateTime? SecondStepDate { get; set; }

		/// <summary>
		/// Ngày hoàn thành step 3 : Đã eKYC giấy tờ + mặt
		/// </summary>
        [ColumnSnackCase(nameof(ThirdStepDate))]
        public DateTime? ThirdStepDate { get; set; }

		/// <summary>
		/// Ảnh mặt quay trái
		/// </summary>
        [ColumnSnackCase(nameof(FaceImageUrl1))]
        public string FaceImageUrl1 { get; set; }

		/// <summary>
		/// Ảnh mặt quay phải
		/// </summary>
        [ColumnSnackCase(nameof(FaceImageUrl2))]
        public string FaceImageUrl2 { get; set; }

		/// <summary>
		/// Ảnh mặt nháy mắt
		/// </summary>
        [ColumnSnackCase(nameof(FaceImageUrl3))]
        public string FaceImageUrl3 { get; set; }

		/// <summary>
		/// Ảnh mặt cười
		/// </summary>
        [ColumnSnackCase(nameof(FaceImageUrl4))]
        public string FaceImageUrl4 { get; set; }

        /// <summary>
        /// Độ chính xác nhận diện toàn mặt
        /// </summary>
        [ColumnSnackCase(nameof(FaceImageSimilarity))]
        public double? FaceImageSimilarity { get; set; }

        /// <summary>
        /// Độ chính xác Ảnh mặt quay trái
        /// </summary>
        [ColumnSnackCase(nameof(FaceImageSimilarity1))]
        public double? FaceImageSimilarity1 { get; set; }

        /// <summary>
        /// Độ chính xác Ảnh mặt quay phải
        /// </summary>
        [ColumnSnackCase(nameof(FaceImageSimilarity2))]
        public double? FaceImageSimilarity2 { get; set; }

        /// <summary>
        /// Độ chính xác Ảnh mặt nháy mắt
        /// </summary>
        [ColumnSnackCase(nameof(FaceImageSimilarity3))]
        public double? FaceImageSimilarity3 { get; set; }

        /// <summary>
        /// Độ chính xác Ảnh mặt cười
        /// </summary>
        [ColumnSnackCase(nameof(FaceImageSimilarity4))]
        public double? FaceImageSimilarity4 { get; set; }

		/// <summary>
		/// Loyalty: Tổng điểm tích lũy
		/// </summary>
        //[ColumnSnackCase(nameof(LoyTotalPoint))]
        [Column("LOY_TOTAL_POINT")]
        public int? LoyTotalPoint { get; set; }

		/// <summary>
		/// Loyalty: Điểm hiện tại
		/// </summary>
        //[ColumnSnackCase(nameof(LoyCurrentPoint))]
        [Column("LOY_CURRENT_POINT")]
        public int? LoyCurrentPoint { get; set; }
		/// <summary>
		/// Địa chỉ Ip đăng ký
		/// </summary>
        [Column("IP_ADDRESS")]
        public string IpAddress { get; set; }
        public List<InvestorIdentification> InvestorIdentifications { get; } = new();
		public List<InvestorIdTemp> InvestorIdTemps { get; } = new();
		public List<InvestorBankAccount> InvestorBankAccounts { get; } = new();
		public List<InvestorBankAccountTemp> InvestorBankAccountTemps { get; } = new();
		public List<InvestorContactAddress> InvestorContactAddresses { get; } = new();
		public List<InvestorContactAddressTemp> InvestorContactAddressTemps { get; } = new();
    }
}
