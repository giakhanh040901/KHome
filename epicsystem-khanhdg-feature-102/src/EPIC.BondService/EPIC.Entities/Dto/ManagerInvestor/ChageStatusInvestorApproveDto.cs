using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class ChageStatusInvestorApproveDto
    {
        public int Status { get; set; }
        public int ApproveId { get; set; }
        public int InvestorId { get; set; }
        public string Username { get; set; }
        public int TradingProviderId { get; set; }
    }
}
