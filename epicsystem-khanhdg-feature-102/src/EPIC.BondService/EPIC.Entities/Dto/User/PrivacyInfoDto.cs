using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.User
{
    public class PrivacyInfoDto
    {
        public DateTime? LastLogin { get; set; }
        public string LastDevice { get; set; }

        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string AvatarImageUrl { get; set; }
    }
}
