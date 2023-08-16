using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderPayment
{
    public class RstOrderPaymentDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int OrderId { get; set; }
        public DateTime? TranDate { get; set; }
        public int TranType { get; set; }
        public int TranClassify { get; set; }
        public int PaymentType { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? CancelDate { get; set; }
        public string CancelBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        /// <summary>
        /// Thông tin ngân hàng của đại lý 
        /// </summary>
        public BusinessCustomerBankDto BankAccount { get; set; }
    }
}
