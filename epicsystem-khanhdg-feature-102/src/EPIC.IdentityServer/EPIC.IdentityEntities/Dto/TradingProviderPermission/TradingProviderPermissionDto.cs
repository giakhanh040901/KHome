using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.TradingProviderPermission
{
    public class TradingProviderPermissionDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string PermissionKey { get; set; }
        public int PermissionType { get; set; }
        public int PermissionInWeb { get; set; }
    }
}
