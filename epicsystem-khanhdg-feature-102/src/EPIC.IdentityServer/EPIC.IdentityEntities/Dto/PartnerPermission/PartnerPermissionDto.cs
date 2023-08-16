using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.PartnerPermission
{
    public class PartnerPermissionDto
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string PermissionKey { get; set; }
        public int PermissionType { get; set; }
        public int PermissionInWeb { get; set; }
    }
}
