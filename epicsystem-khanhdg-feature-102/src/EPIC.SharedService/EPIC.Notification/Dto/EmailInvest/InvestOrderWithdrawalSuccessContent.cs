using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EmailInvest
{
    /// <summary>
    /// Rút vốn thành công / Tất toán trước hạn (Rút toàn bộ)
    /// </summary>
    public class InvestOrderWithdrawalSuccessContent
    {
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Giá trị đầu tư trong order
        /// </summary>
        public string TotalValue { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }
        public string Tenor { get; set; }
        public string PaymentFullDate { get; set; }
        public string InvCode { get; set; }
        public string InvName { get; set; }
        public string PolicyName { get; set; }
        public string Profit { get; set; }
        public string TradingProviderName { get; set; }

        /// <summary>
        /// Ngày rút
        /// </summary>
        public string WithdrawDate { get; set; }

        /// <summary>
        /// Số tiền rút
        /// </summary>
        public string WithdrawAmount { get; set; }

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public string ActuallyAmount { get; set; }

        /// <summary>
        /// Gía trị còn lại
        /// </summary>
        public string AmountRemain { get; set; }

        /// <summary>
        /// Ngày tất toán
        /// </summary>
        public string SettlementDate { get; set; }

    }
}
