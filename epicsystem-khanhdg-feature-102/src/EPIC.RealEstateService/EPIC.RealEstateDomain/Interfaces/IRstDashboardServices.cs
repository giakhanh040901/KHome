using EPIC.RealEstateEntities.Dto.RstDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstDashboardServices
    {
        RstDashboardDto Dasboard(GetRstDashboardDto dto);
        List<RstListProjectDashboardDto> GetListProject();
    }
}
