using EPIC.Utils.ConstantVariables.Identity;
using System.Collections.Generic;

namespace EPIC.IdentityEntities.Dto.Roles
{
    public class CreateRolePermissionDto
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Loại Role
        /// <see cref="RoleTypes"/>
        /// </summary>
        public int? RoleType { get; set; }

        public int PermissionInWeb { get; set; }
        public List<string> PermissionKeys { get; set; }
        public List<string> PermissionKeysRemove { get; set; }
    }
}
