using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EmailInvest
{
    /// <summary>
    /// Nội dung chi trả thành công
    /// </summary>
    public class InvestOrderInterestPaymentSuccessDto
    {
        public string CustomerName { get; set; }
        public string TotalValue { get; set; }
        public string InitTotalValue { get; set; }
        public string ContractCode { get; set; }
        public string Tenor { get; set; }
        public string PaymentFullDate { get; set; }
        public string InvCode { get; set; }
        public string InvName { get; set; }
        public string PolicyName { get; set; }
        public string Profit { get; set; }
        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public string PayDate { get; set; }

        /// <summary>
        /// Kỳ lợi nhuận
        /// </summary>
        public string PeriodIndex { get; set; }
        public string TradingProviderName { get; set; }

        /// <summary>
        /// Số tiền chi
        /// </summary>
        public string AmountMoneyPay { get; set; }

        #region Nếu chi trả là cuối kỳ, tất toán đúng hạn
        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public string InvestDate { get; set; }

        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public string DueDate { get; set; }
        #endregion
    }
}
