using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.RenewalsRequest
{
    public class ViewInvRenewalsRequestDto : ViewInvestApproveDto
    {
        public int? SettlementMethod { get; set; }
        public EPIC.InvestEntities.DataEntities.Policy Policy { get; set; }
        public PolicyDetail PolicyDetail { get; set; }
        public EPIC.InvestEntities.DataEntities.Project Project { get; set; }
    }
}
