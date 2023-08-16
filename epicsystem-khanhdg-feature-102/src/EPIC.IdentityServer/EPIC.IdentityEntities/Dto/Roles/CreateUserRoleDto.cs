using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.Roles
{
    public class CreateUserRoleDto
    {
        public int UserId { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
