using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class RequestApproveDto
    {
        public int InvestorId { get; set; }
        public int InvestorGroupId { get; set; }
        public int Action { get; set; }
        public string Notice { get; set; }
        public int? UserApproveId { get; set; }
        public string Summary { get; set; }
        public string ApproveRequestFileUrl { get; set; }
    }
}
