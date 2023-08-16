using EPIC.IdentityEntities.Dto.Permissions;
using System.Collections.Generic;

namespace EPIC.IdentityEntities.Dto.Roles
{
    public class RolePermissionInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PartnerId { get; set; }
        public int? TradingProviderId { get; set; }
        public int RoleType { get; set; }
        public int PermissionInWeb { get; set; }
        public List<RolePermissionDto> PermissionKeys { get; set; }
    }
}
