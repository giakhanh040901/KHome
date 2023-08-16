using System;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    /// <summary>
    /// Thông tin sale của nhà đầu tư
    /// </summary>
    public class SaleInfoByInvestorDto
    {
        public int Id { get; set; }

        public int InvestorId { get; set; }

        public int? SaleId { get; set; }

        public string ReferralCode { get; set; }

        public string IsDefault { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string AvatarImageUrl { get; set; }

        public string Fullname { get; set; }

        public int? InvestorIdOfSale { get; set; }

        public int? BusinessCustomerIdOfSale { get; set; }

        public string Phone { get; set; }
    }
}
