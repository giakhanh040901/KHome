using EPIC.InvestEntities.Dto.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.RenewalsRequest
{
    public class InvRenewalsRequestDto
    {
        public int Id { get; set; }
        public long OrderId { get; set; }
        public int SettlementMethod { get; set; }
        public int? RenewalsPolicyDetailId { get; set; }
        public PolicyDetailDto PolicyDetail { get; set; }
        public PolicyDto Policy { get; set; }
    }
}
