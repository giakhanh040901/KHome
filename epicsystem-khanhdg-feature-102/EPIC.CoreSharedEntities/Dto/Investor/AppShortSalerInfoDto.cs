using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class AppShortSalerInfoDto
    {
        public string FullName { get; set; }
        public string AvatarImageUrl { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsSale { get; set; }

        public List<AppDeparmentDto> ListDepartment { get; set; }
    }
}
