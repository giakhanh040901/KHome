using EPIC.InvestEntities.Dto.InvestApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.RenewalsRequest
{
    public class FilterInvRenewalsRequestDto : InvestApproveGetDto
    {
        public int? SettlementMethod { get; set; }
    }
}
