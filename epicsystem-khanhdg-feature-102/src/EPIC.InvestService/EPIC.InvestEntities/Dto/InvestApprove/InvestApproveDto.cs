using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestApprove
{
    public class InvestApproveDto
    {
        public int Id { get; set; }
        public string ApproveNote { get; set; }
        public int? UserApproveId { get; set; }
    }
}
