using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppSaleManagerDto
    {
        public string ReferralCode { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string TradingProviderName { get; set; }
        public string Area { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAddress { get; set; }
        public string AvatarImageUrl { get; set; }
    }
}
