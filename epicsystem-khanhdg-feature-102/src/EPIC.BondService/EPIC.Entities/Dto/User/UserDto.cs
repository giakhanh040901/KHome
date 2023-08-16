using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Status { get; set; }
        public string IpAddress { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }

        public int? PartnerId { get; set; }
        public int? TradingProviderId { get; set; }
        public int? InvestorId { get; set; }

        public string IsFirstTime { get; set; }
        public string IsTempPassword { get; set; }
        public string IsTempPin { get; set; }
        public string IsVerifiedEmail { get; set; }
    }
}
