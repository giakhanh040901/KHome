using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDashboard
{
    public class RstDashboardDto
    {

        /// <summary>
        /// Tổng quan DashBoard
        /// </summary>
        public DashboardOverViewDto DashboardOverView { get; set; }
        /// <summary>
        /// Biểu đồ bán hàng theo thời gian
        /// </summary>
        public List<SellChartOverTimeDto> SellChartOverTime { get; set; }
        /// <summary>
        /// Biểu đồ tỷ lệ bán hàng
        /// </summary>
        public List<ChartRatioSellDto> ChartRatioSell { get; set; }
        /// <summary>
        /// Biểu đồ theo từng loại hình và mức bán
        /// </summary>
        public List<NumberOfApartmentsChartDto> NumberOfApartmentsChart { get; set; }
        /// <summary>
        /// Dự án nổi bật
        /// </summary>
        public List<DashboardOutstandingProjectDto> DashboardOutstandingProject { get; set; }
        /// <summary>
        /// Hoạt động gần đây
        /// </summary>
        public List<RstDashboardActionsDto> RstDashboardActions { get; set; }
    }
}
