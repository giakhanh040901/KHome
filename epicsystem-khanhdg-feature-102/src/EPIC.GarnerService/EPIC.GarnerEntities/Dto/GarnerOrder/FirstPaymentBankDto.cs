using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class FirstPaymentBankDto
    {
        public int? BusinessCustomerBankId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public string BankAccName { get; set; }
        public string BankAccNo { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string Logo { get; set; }
        public int BankId { get; set; }
        public string FullBankName { get; set; }
        public int? Status { get; set; }
        public string IsDefault { get; set; }
    }
}
