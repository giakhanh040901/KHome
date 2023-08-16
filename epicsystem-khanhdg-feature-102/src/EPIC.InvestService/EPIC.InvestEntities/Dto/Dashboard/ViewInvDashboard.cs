using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Dashboard
{
    public class ViewInvDashboard
    {
        public DashboardTienVaoDto TienVao { get; set; }
        public DashboardTienRaDto TienRa { get; set; }
        public DashboardSoDuDto SoDu { get; set; }
        public List<DashboardDongTienTheoNgayDto> DongTienTheoNgay { get; set; }
        public List<DashboardDanhSachTheoKyHanSP> DanhSachTheoKyHanSP { get; set; }

        /// <summary>
        /// Báo cáo doanh số và số dư khu vực theo đại lý
        /// </summary>
        public List<InvestDashboardCashInDepartmentByTrading> DashboardCashInDepartment { get; set; }

        /// <summary>
        /// Báo cáo doanh số và số dư của đại lý
        /// </summary>
        public List<InvestDashboardCashTradingByPartner> DashboardCashTradingByPartner { get; set; }

        public List<DashboardCashOutByMonthInYearDto> CashOutByMonthInYear { get; set; }

        /// <summary>
        /// Hoạt động gần đây
        /// </summary>
        public List<InvestDashboardActionsDto> Actions { get; set; }
    }
}
