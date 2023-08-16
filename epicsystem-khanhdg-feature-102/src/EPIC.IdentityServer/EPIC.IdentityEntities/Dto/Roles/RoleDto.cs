using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.Roles
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PartnerId { get; set; }
        public int? TradingProviderId { get; set; }
        public int RoleType { get; set; }
        public int PermissionInWeb { get; set; }
        public int TotalUse { get; set; }
        public string Status { get; set; }
    }
}
    