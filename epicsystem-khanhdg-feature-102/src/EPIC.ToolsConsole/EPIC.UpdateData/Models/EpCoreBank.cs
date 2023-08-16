using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreBank
    {
        public decimal BankId { get; set; }
        public string Logo { get; set; }
        public string BankName { get; set; }
        public string FullBankName { get; set; }
        public string PvcbBankId { get; set; }
        public string BankCode { get; set; }
        public string Bin { get; set; }
    }
}
