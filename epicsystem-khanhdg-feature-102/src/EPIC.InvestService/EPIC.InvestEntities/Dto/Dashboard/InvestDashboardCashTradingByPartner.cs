using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Dashboard
{
    /// <summary>
    /// Báo cáo doanh số và số dư của đại lý
    /// </summary>
    public class InvestDashboardCashTradingByPartner
    {
        /// <summary>
        /// Số dư
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Tiền thừa
        /// </summary>
        public decimal Remain { get; set; }

        /// <summary>
        /// Id đại lý 
        /// </summary>
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Tên đại lý bán 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên viết tắt nếu có
        /// </summary>
        public string ShortName { get; set; }
    }
}
