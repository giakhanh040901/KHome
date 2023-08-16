using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.Permissions
{
    public class CreateMaxWebPermissionToTradingDto
    {
        public List<string> PermissionWebKeys { get; set; }
        public int TradingProviderId { get; set; }
    }
}
