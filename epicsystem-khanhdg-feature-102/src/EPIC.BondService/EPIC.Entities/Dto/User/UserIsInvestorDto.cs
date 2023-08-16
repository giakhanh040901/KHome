using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.User
{
    public class UserIsInvestorDto
    {
        public int UserId { get; set; } 
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Sex { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int InvestorId { get; set; }
        public string CifCode { get; set; }
        public string IdNo { get; set; }
        public int? Source { get; set; }
        public int? CheckSaleStatus { get; set; }
        public string ReferralCodeSelf { get; set; }
    }

    public class UserIsInvestorForAppDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int InvestorId { get; set; }
        public string CifCode { get; set; }
        public List<string> FcmTokens { get; set; }
    }
}
