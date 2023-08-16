using EPIC.BondEntities.Dto.BondDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondDashboardService
    {
        /// <summary>
        /// Lấy thông số bond dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ViewBondDashboard GetBondDashboard(GetBondDashboardDto dto);
    }
}
