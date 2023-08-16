using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Dashboard
{
    public class DashboardDto
    {
        /// <summary>
        /// Tổng quan DashBoard
        /// </summary>
        public DashboardOverviewDto DashboardOverview { get; set; }
        /// <summary>
        /// Biểu đồ khách hàng theo giới tính 
        /// </summary>
        public List<GenderCustomerChartDto> GenderCustomerCharts { get; set; }
        /// <summary>
        /// Biểu đồ khách hàng theo thế hệ
        /// </summary>
        public List<GenerationCustomerChartDto> GenerationCustomerCharts { get; set; }
        /// <summary>
        /// Biểu đồ phễu
        /// </summary>
        public FunnelChartDto FunnelCharts { get; set; }
    }
}
