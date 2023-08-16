using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.OrderPayment
{
    public class OrderPaymentDto
    {
		public long Id { get; set; }
		public long? OrderId { get; set; }
		public string PaymentNo { get; set; }
		public DateTime? TranDate { get; set; }
		public int? TranType { get; set; }
		public int? TranClassify { get; set; }
		public int? PaymentType { get; set; }
		public decimal? PaymentAmnount { get; set; }
		public string Description { get; set; }
		public int? Status { get; set; }
	}
}
