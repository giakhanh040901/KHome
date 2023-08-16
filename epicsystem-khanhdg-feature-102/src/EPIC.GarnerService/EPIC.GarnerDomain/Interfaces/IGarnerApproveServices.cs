using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerApproveServices
    {
        PagingResult<ViewGarnerApproveDto> FindAll(FilterGarnerApproveDto input);
    }
}
