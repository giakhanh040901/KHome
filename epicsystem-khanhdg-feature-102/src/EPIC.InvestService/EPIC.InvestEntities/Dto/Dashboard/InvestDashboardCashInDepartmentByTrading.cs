using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Dashboard
{
    /// <summary>
    /// Báo cáo doanh số và số dư khu vực
    /// </summary>
    public class InvestDashboardCashInDepartmentByTrading
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
        /// Id phòng ban của đại lý
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Id đại lý bán hộ
        /// </summary>
        public int TradingProviderIdSub { get; set; }

        /// <summary>
        /// Tên phòng ban hoặc đại lý bán hộ
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên viết tắt nếu có
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Loại doanh số từ: 1: Phòng bán, 2: Đại lý bán hộ
        /// </summary>
        public int Type { get; set; }
    }
}
