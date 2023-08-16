using EPIC.Entities.Dto.Investor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class SaleInvestorDto
    {
        public int SaleId { get; set; }
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public string ReferralCode { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AvatarImageUrl { get; set; }
    }
}
