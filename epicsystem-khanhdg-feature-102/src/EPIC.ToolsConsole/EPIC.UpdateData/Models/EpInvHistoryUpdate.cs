using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvHistoryUpdate
    {
        public decimal Id { get; set; }
        public decimal RealTableId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string FieldName { get; set; }
        public decimal UpdateTable { get; set; }
        public decimal Action { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Summary { get; set; }
    }
}
