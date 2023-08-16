using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.UsersPartner
{
    public class CreateUsersPartnerDto
    {
        public int PartnerId { get; set; }
        public List<int> UserIds { get; set; }
    }
}
