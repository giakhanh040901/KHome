using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.User
{
    public class MyInfoDto
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Ảnh đại diện của User
        /// </summary>
        public string UserAvatarImageUrl { get; set; }
        public string Phone{ get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string IsTempPassword { get; set; }
        public string Status { get; set; }
        public string IsDeleted { get; set; }
    }
}
