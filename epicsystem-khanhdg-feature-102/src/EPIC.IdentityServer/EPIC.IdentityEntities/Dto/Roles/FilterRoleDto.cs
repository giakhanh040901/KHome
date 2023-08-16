using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EPIC.IdentityEntities.Dto.Roles
{
    public class FilterRoleDto : PagingRequestBaseDto
    {
        public int PermissionInWeb { get; set; }
        /// <summary>
        /// A,D
        /// </summary>
        [FromQuery(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Loại role
        /// </summary>
        [FromQuery(Name = "roleType")]
        public int? RoleType { get; set; }
    }
}
