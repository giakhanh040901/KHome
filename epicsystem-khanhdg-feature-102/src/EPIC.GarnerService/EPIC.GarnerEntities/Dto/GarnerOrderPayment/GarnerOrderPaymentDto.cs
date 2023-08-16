using EPIC.Entities.Dto.BusinessCustomer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrderPayment
{
    public class GarnerOrderPaymentDto
    {
        public long Id { get; set; }
        public int TradingProviderId { get; set; }
        public long OrderId { get; set; }
        public DateTime? TranDate { get; set; }
        public int TranType { get; set; }
        public int TranClassify { get; set; }
        public int PaymentType { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        /// <summary>
        /// Thông tin ngân hàng của đại lý
        /// </summary>
        public BusinessCustomerBankDto TradingBankAccount { get; set; }
    }
}
