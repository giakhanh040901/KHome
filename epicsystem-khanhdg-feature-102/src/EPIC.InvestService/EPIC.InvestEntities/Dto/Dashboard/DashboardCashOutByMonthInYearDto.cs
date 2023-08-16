using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Dashboard
{
    /// <summary>
    /// Số tiền thực chi theo tháng trong năm hiện tại
    /// </summary>
    public class DashboardCashOutByMonthInYearDto
    {
        /// <summary>
        /// Tháng
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// Tiền thực chi theo tháng
        /// </summary>
        public decimal Amount { get; set; }
    }
}
