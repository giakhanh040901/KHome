
using EPIC.InvestEntities.Dto.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IDashboardServices
    {
        /// <summary>
        /// Lấy thông số invest dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ViewInvDashboard GetInvDashboard(GetInvDashboardDto dto);

        /// <summary>
        /// Sản phẩm của đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        List<InvestDashboardDistributionPickDto> GetPicklistDistributionByTrading(int? tradingProviderId);
    }
}
