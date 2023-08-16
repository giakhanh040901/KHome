using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.Dto.Pvcb
{
    public class CreateBankOrderPaymentDto
    {
        public int OrderId { get; set; }
        public decimal? Amount { get; set; }
        public string Account { get; set; }
        public DateTime? TranDate { get; set; }
        public string Description { get; set; }
    }
}
