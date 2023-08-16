using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.Dto.RstApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstApproveServices
    {
        PagingResult<RstDataApproveDto> FindAll(FilterRstApproveDto input);
    }
}
