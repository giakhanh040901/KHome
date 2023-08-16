using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail
{
    public class CreateMsbRequestDetailDto
    {
        public long RequestId { get; set; }
        public int DataType { get; set; }
        public int BankId { get; set; }
        public int BankAccountId { get; set; }
        public decimal AmountMoney { get; set; }
        public string Exception { get; set; }
        public int TradingBankAccId { get; set; }
    }
}
