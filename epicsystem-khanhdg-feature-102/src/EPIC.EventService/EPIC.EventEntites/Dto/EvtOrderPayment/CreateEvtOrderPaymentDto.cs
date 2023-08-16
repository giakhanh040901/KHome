using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrderPayment
{
    public class CreateEvtOrderPaymentDto
    {
        public int OrderId { get; set; }
        /// <summary>
        /// Tài khoản ngân hàng của đại lý
        /// </summary>
        public int TradingBankAccountId { get; set; }
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
        
        private string _description;
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
    }
}
