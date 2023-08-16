using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class LogError
    {
        public string Target { get; set; }
        public DateTime? LogDate { get; set; }
        public string LogTime { get; set; }
        public string Message { get; set; }
        public string Errno { get; set; }
        public Guid? Msgid { get; set; }
        public string Deleted { get; set; }
        public string Lstupdusrcde { get; set; }
        public DateTime? Lstupddtetme { get; set; }
        public decimal Autoid { get; set; }
    }
}
