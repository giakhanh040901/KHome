using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    public class ViewUserDto
    {
        public decimal UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string IsFirstTime { get; set; }
        public string IsTempPassword { get; set; }
        public string IsTempPin { get; set; }
        public string IsHavePin { get; set; }
        public string IsHaveBank { get; set; }
        public string IsEkyc { get; set; }
        public string IsVerifiedFace { get; set; }
        public string IsVerifiedEmail { get; set; }
        public string IsCheck { get; set; }
        public DateTime? LockedDate { get; set; }
    }
}
