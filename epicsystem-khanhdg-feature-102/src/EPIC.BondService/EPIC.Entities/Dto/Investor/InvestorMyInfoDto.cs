using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    public class InvestorMyInfoDto
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string ShortName { get; set; }
        public string Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string AvatarImageUrl { get; set; }
        public string EkycInfoIsConfirmed { get; set; }
        public string ReferralCodeSelf { get; set; }
        public int ProfStatus { get; set; }
        public string IsVerifiedEmail { get; set; }
        public string IsScannedReferralCode { get; set; }
        public string IsVerified { get; set; }
        public string IsDirectioner { get; set; }
        public string AutoDirectional { get; set; }
    }
}
