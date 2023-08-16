using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpMsbRequestPaymentDetail
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public int DataType { get; set; }
        public int ReferId { get; set; }
        public int Status { get; set; }
        public int BankId { get; set; }
        public string Exception { get; set; }
        public string Bin { get; set; }
        public string Note { get; set; }
        public string OwnerAccount { get; set; }
        public int TradingBankAccId { get; set; }
        public decimal AmountMoney { get; set; }
    }
}
