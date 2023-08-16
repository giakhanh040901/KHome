using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstHistoryServices
    {
        PagingResult<RstHistoryUpdateDto> FindAllHistoryTable(FilterRstHistoryUpdateDto input, int[] uploadTable);
    }
}
