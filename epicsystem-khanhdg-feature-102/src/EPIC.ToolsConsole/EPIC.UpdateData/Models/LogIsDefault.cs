using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class LogIsDefault
    {
        public decimal Id { get; set; }
        public string TableName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public decimal? IdRecord { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Notice { get; set; }
    }
}
