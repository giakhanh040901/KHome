using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.Permissions
{
    public class CreateMaxWebPermissionDto
    {
        public List<string> PermissionWebKeys { get; set; }
        public int PartnerId { get; set; }
    }
}
