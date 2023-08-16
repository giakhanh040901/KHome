using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.Order
{
    /// <summary>
    /// Thông tin tài sản thụ hưởng của đại lý sơ cấp
    /// </summary>
    public class AppPaymentInfoDto
    {
        public int? BusinessCustomerBankId { get; set; }
        public string BankAccName { get; set; }
        public string BankAccNo { get; set; }
        public string BankName { get; set; }
        public string PaymentNote { get; set; }
    }
}
