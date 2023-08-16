using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestApprove
{
    public class InvestApproveGetDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Keyword { get; set; }
        public int? Status { get; set; }
        public int? UserApproveId { get; set; }
        public int? UserRequestId { get; set; }
        public int? DataType { get; set; }
        public int? ActionType { get; set; }
        public string Summary { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ApproveDate { get; set; }
    }
}
