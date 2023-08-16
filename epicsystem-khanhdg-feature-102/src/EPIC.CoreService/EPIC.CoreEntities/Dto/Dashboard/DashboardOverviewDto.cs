using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Dashboard
{
    public class DashboardOverviewDto
    {
        /// <summary>
        /// Tổng số lượng khách hàng
        /// </summary>
        public int TotalInvestor { get; set; }
        /// <summary>
        /// Tổng số lượng khách hàng hôm nay
        /// </summary>
        public int TotalTodayInvestor { get; set; }
        /// <summary>
        /// Tổng số lượng khách hàng trong tháng
        /// </summary>
        public int TotalMonthInvestor { get; set; }
        /// <summary>
        /// Tổng số lượng khách hàng trong năm
        /// </summary>
        public int TotalYearInvestor { get; set; }
        /// <summary>
        /// Tổng số lượng khách hàng giao dịch
        /// </summary>
        public int TotalDealInvestor { get; set; }
        /// <summary>
        /// Tổng số lượng khách hàng hôm nay
        /// </summary>
        public int TotalTodayDealInvestor { get; set; }
        /// <summary>
        /// Tổng số lượng khách hàng trong tháng
        /// </summary>
        public int TotalMonthDealInvestor { get; set; }
        /// <summary>
        /// Tổng số lượng khách hàng trong năm
        /// </summary>
        public int TotalYearDealInvestor { get; set; }
    }
}
