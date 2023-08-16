using EPIC.CoreEntities.Dto.Dashboard;
using EPIC.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IDashboardServices
    {
        PagingResult<CustomerListDto> CustomerList(FilterCoreDashboardDto dto);
        DashboardDto Dashboard(FilterCoreDashboardDto dto);
    }
}
