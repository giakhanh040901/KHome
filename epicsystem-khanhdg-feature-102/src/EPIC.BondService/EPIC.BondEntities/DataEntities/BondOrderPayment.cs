using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondOrderPayment : IFullAudited
    {
		[Column(Name = "ID")]
		public long Id { get; set; }

		[Column(Name = "TRADING_PROVIDER_ID")]
		public int TradingProviderId { get; set; }

		[Column(Name = "ORDER_ID")]
		public int OrderId { get; set; }

		[Column(Name = "TRAN_DATE")]
		public DateTime? TranDate { get; set; }

		[Column(Name = "TRAN_TYPE")]
		public int TranType { get; set; }

		[Column(Name = "TRAN_CLASSIFY")]
		public int TranClassify { get; set; }

		[Column(Name = "PAYMENT_TYPE")]
		public int PaymentType { get; set; }

		[Column(Name = "PAYMENT_AMOUNT")]
		public decimal PaymentAmount { get; set; }

		[Column(Name = "DESCRIPTION")]
		public string Description { get; set; }

		[Column(Name = "STATUS")]
		public int Status { get; set; }

		[Column(Name = "APPROVE_BY")]
		public string ApproveBy { get; set; }

		[Column(Name = "APPROVE_DATE")]
		public DateTime? ApproveDate { get; set; }

		[Column(Name = "CREATED_BY")]
		public string CreatedBy { get; set; }

		[Column(Name = "CREATED_DATE")]
		public DateTime? CreatedDate { get; set; }

		[Column(Name = "MODIFIED_BY")]
		public string ModifiedBy { get; set; }

		[Column(Name = "MODIFIED_DATE")]
		public DateTime? ModifiedDate { get; set; }

		[Column(Name = "DELETED")]
		public string Deleted { get; set; }
	}
}
