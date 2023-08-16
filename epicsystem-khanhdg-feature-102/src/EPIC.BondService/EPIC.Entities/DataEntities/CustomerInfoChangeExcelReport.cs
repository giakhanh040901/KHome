using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    public class CustomerInfoChangeExcelReport
    {
        public int ApproveId { get; set; }
        public string CifCode { get; set; }
        public string FullName { get; set; }
        public string IdNo { get; set; }
        public int? InvestorId { get; set; }
        public int? ActionType { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime? RequestDate { get; set; }
        public int? Status { get; set; }
        public int? UserApproveId { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveNote { get; set; }
        public string IsCheck { get; set; }
    }
}
