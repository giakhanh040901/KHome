using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BlockadeLiberation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondBlockadeLiberationService
    {
        PagingResult<BlockadeLiberationDto> FindAll(int pageSize, int pageNumber, string keyword, int? status, int? type);
        BlockadeLiberationDto FindById(int? id);
        BondBlockadeLiberation Add(CreateBlockadeLiberationDto input);
        int Update(UpdateBlockadeLiberationDto entity, int id);
    }
}
