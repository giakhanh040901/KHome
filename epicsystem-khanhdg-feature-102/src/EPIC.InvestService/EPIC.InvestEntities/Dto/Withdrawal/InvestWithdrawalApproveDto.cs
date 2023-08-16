using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Withdrawal
{
    /// <summary>
    /// Dữ liệu khi duyệt rút vốn
    /// </summary>
    public class InvestWithdrawalApproveDto
    {
        /// <summary>
        /// Id yêu cầu rút
        /// </summary>
        public long Id { get; set; }

        public int TradingProviderId { get; set; }

        /// <summary>
        /// Lợi nhuận thực nhận
        /// </summary>
        public decimal ActuallyAmount { get; set; }

        /// <summary>
        /// Lợi tức rút
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// Lợi tức khấu trừ
        /// </summary>
        public decimal DeductibleProfit { get; set; }

        /// <summary>
        /// Thuế lợi nhuận
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Lợi tức thực nhận
        /// </summary>
        public decimal ActuallyProfit { get; set; }

        /// <summary>
        /// Phí rút
        /// </summary>
        public decimal WithdrawalFee { get; set; }

        /// <summary>
        /// Ip người duyệt
        /// </summary>
        public string ApproveIp { get; set; }

        /// <summary>
        /// Nội dung duyệt
        /// </summary>
        public int? ApproveNote { get; set; }
        /// <summary>
        /// Người duyệt
        /// </summary>
        public string Username { get; set; }
    }
}
