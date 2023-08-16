using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDashboard
{
    public class GarnerDashboardDto
    {
        /// <summary>
        /// Dòng tiền vào
        /// </summary>
        public GarnerDashboardOverviewDto CashInFlow { get; set; }

        /// <summary>
        /// Dòng tiền ra
        /// </summary>
        public GarnerDashboardOverviewDto CashOutFlow { get; set; }

        /// <summary>
        /// Số dư
        /// </summary>
        public GarnerDashboardOverviewDto CashRemain { get; set; }

        /// <summary>
        /// Dòng tiền theo thời gian
        /// </summary>
        public List<GarnerDashboardTimeFlowDto> TimeFlow { get; set; }

        /// <summary>
        /// Doanh số theo kỳ hạn sản phẩm
        /// </summary>
        public List<GarnerDashboardPolicyDto> ListPolicy { get; set; }
        /// <summary>
        /// Doanh số và số dư theo đại lý
        /// </summary>
        public List<GarnerDashboardCashInByTrading> CashInByTrading { get; set; }
        /// <summary>
        /// Doanh số và số dư theo partner
        /// </summary>
        public List<GarnerDashboardCashInByPartner> CashInByPartner { get; set; }
        /// <summary>
        /// Báo cáo thực chi
        /// </summary>
        public List<GarnerDashboardCashOutByMonth> CashOutByMonths { get; set; }
        /// <summary>
        /// Hoạt động gần đây của khách hàng
        /// </summary>
        public List<GarnderDashboardActionsDto> Actions { get; set; }
    }
}
