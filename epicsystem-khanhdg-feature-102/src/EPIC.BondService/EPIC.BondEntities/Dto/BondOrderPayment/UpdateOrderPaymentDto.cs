using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.OrderPayment
{
    public class UpdateOrderPaymentDto : CreateOrderPaymentDto
    {
        public long OrderPaymentId { get; set; }
    }
}
