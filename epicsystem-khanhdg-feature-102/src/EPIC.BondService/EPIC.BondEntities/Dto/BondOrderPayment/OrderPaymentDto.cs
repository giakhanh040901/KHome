using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.OrderPayment
{
    public class OrderPaymentDto
    {
        public long OrderPaymentId { get; set; }
        public long? OrderId { get; set; }
        public DateTime? TranDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public int? TranType { get; set; }
        public int? TranClassify { get; set; }
        public int? PaymentType { get; set; }
        public decimal? PaymentAmnount { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
    }
}
