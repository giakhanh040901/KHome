using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpMsbNotification
    {
        public long Id { get; set; }
        public string TranSeq { get; set; }
        public string VaCode { get; set; }
        public string VaNumber { get; set; }
        public string FromAccountName { get; set; }
        public string FromAccountNumber { get; set; }
        public string ToAccountName { get; set; }
        public string ToAccountNumber { get; set; }
        public string TranAmount { get; set; }
        public string TranRemark { get; set; }
        public string TranDate { get; set; }
        public string Signature { get; set; }
        public int Status { get; set; }
        public string Exception { get; set; }
        public string Ip { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
