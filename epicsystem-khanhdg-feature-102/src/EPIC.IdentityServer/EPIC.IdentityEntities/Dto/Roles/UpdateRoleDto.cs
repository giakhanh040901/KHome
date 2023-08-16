using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.Roles
{
    public class UpdateRoleDto : CreateRoleDto
    {
        public decimal Id { get; set; }
    }
}
