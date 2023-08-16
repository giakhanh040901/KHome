using System;

namespace EPIC.CoreEntities.Dto.SaleInvestor
{
    public class SaleInvestorInfoDto
    {
        public int InvestorId { get; set; }
        public string Phone { get; set; }
        public string AvatarImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
    }
}
