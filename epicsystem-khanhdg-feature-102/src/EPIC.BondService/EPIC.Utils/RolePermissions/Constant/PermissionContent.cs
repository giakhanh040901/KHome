using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.RolePermissions.Constant
{
    public class PermissionContent
    {
        /// <summary>
        /// Loại permission là web, menu, button,...
        /// </summary>
        public int PermissionType { get; set; }
        /// <summary>
        /// Permission nằm trong web nào: core, bond,...
        /// </summary>
        public int PermissionInWeb { get; set; }
        /// <summary>
        /// Mô tả 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Key cha
        /// </summary>
        public string ParentKey { get; set; }
    }
}
