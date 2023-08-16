using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.User
{
    public class ViewUserFcmToken
    {
        public decimal UserId { get; set; }
        public List<string> FcmToken { get; set; }
    }
}
