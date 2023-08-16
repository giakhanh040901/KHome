using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondRenewalsRequest
    {
        public int Id { get; set; }
        public long OrderId { get; set; }
        public int SettlementMethod { get; set; }
        public int RenewarsPolicyDetailId { get; set; }
    }
}
