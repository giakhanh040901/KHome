using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.Permissions
{
    public class RolePermissionDto
    {
        public decimal Id { get; set; }
        public decimal RoleId { get; set; }
        public string PermissionKey { get; set; }
        public decimal PermissionType { get; set; }
    }
}
