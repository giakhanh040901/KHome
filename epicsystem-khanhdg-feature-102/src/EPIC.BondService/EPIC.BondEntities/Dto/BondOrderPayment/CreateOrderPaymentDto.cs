using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.OrderPayment
{
    public class CreateOrderPaymentDto
    {
        public int OrderId { get; set; }
        public DateTime? TranDate { get; set; }
        public int TranType { get; set; }
        public int TranClassify { get; set; }
        public int PaymentType { get; set; }
        public decimal PaymentAmount { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
    }
}
