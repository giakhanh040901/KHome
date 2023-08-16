using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.RenewalsRequest
{
    public class AppCreateRenewalsRequestDto
    {
        public long OrderId { get; set; }
        public int SettlementMethod { get; set; }
        public int RenewarsPolicyDetailId { get; set; }
    }
}
