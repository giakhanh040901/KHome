using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.Permissions
{
    public class UpdateMaxPermissionInWeb
    {
        public int PermissionInWeb { get; set; }
        public List<string> PermissionKeys { get; set; }
        public List<string> PermissionKeysRemove { get; set; }
    }
}
