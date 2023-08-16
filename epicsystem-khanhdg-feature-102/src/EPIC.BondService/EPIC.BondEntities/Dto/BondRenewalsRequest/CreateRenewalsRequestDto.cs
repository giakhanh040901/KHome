using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.RenewalsRequest
{
    public class CreateRenewalsRequestDto
    {
        public long OrderId { get; set; }
        public int SettlementMethod { get; set; }
        public int RenewarsPolicyDetailId { get; set; }
        public string RequestNote { get; set; }
        public string Summary { get; set; }
    }
}
