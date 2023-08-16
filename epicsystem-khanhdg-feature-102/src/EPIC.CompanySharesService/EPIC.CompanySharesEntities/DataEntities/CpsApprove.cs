using EPIC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class CpsApprove
    {
        public int Id { get; set; }
        public int? UserRequestId { get; set; }
        public int? UserApproveId { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public string RequestNote { get; set; }
        public string ApproveNote { get; set; }
        public string CancelNote { get; set; }
        public int Status { get; set; }
        public int? DataType { get; set; }
        public int? DataStatus { get; set; }
        public string DataStatusStr { get; set; }
        public int? ActionType { get; set; }
        public int? ReferIdTemp { get; set; }
        public int? ReferId { get; set; }
        public string IsCheck { get; set; }
        public int? UserCheckId { get; set; }
        public string Summary { get; set; }
    }
}
