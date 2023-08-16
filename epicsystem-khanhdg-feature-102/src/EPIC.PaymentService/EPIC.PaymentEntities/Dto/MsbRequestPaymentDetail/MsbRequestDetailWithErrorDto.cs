using EPIC.MSB.Dto.PayMoney;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail
{
    public class MsbRequestDetailWithErrorDto
    {
        public long Id { get; set; }
        public int DataType { get; set; }
        public long ReferId { get; set; }
        public string OwnerAccount { get; set; }
        public string BankAccount { get; set; }
        public decimal AmountMoney { get; set; }
        public string Bin { get; set; }
        public string BankName { get; set; }
        public int BankId { get; set; }
        public string BankCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
