using EPIC.GarnerEntities.Dto.GarnerDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerDashboardServices
    {
        /// <summary>
        /// Lấy thông tin dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        GarnerDashboardDto Dashboard(GarnerDashboardFindDto dto);
        /// <summary>
        /// Lấy list product để fe chọn trong màn dashboard
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<GarnerDashboardProductPickListDto> GetPicklistProductByTrading(int? tradingProviderId);
    }
}
