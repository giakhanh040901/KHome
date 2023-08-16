using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.RealEstateNotification
{
    public class RstOrderPaymentSuccessContent
    {
        /// <summary>
        /// Nhà đầu tư
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Mã dự án 
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Mã căn hộ
        /// </summary>
        public string ProductItemCode { get; set; }
        /// <summary>
        /// Số tiền thanh toán
        /// </summary>
        public string PaymentAmount { get; set; }

        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public string TranDate { get; set; }

        /// <summary>
        /// Thông tin giao dịch
        /// </summary>
        public string TranNote { get; set; }

        /// <summary>
        /// Tên đại lý
        /// </summary>
        public string TradingProviderName { get; set; }

        /// <summary>
        /// Số tiền cọc phải nộp
        /// </summary>
        public string DepositMoney { get; set; }

        /// <summary>
        /// Tổng số tiền đã thanh toán cọc
        /// </summary>
        public string PaymentMoney { get; set; }
    }
}
