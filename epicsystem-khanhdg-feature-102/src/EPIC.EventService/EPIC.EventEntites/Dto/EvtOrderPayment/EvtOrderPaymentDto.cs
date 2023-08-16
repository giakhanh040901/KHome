using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrderPayment
{
    public class EvtOrderPaymentDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id đại lý
        /// </summary>
        public int TradingProviderId { get; set; }
        /// <summary>
        /// ID order
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Số giao dịch 
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// Tài khoản đại lý
        /// </summary>
        public int? TradingBankAccountId { get; set; }
        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime? TranDate { get; set; }
        /// <summary>
        /// Loại giao dịch
        /// </summary>
        public int TranClassify { get; set; }
        /// <summary>
        /// Phương thức thanh toán
        /// </summary>
        public int PaymentType { get; set; }
        /// <summary>
        /// Số tiền giao dịch
        /// </summary>
        public decimal PaymentAmount { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// trạng thái
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Ngày duyệt
        /// </summary>
        public DateTime? ApproveDate { get; set; }
        /// <summary>
        /// Người duyệt
        /// </summary>
        public string ApproveBy { get; set; }
        /// <summary>
        /// Ngày hủy
        /// </summary>
        public DateTime? CancelDate { get; set; }
        /// <summary>
        /// Người hủy
        /// </summary>
        public string CancelBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Deleted { get; set; }

        /// <summary>
        /// Thông tin ngân hàng của đại lý hoặc đối tác(chủ đầu tư)
        /// </summary>
        public BusinessCustomerBankDto BankAccount { get; set; }
    }
}
