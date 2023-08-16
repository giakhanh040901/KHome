using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.BlockadeLiberation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IBlockadeLiberationServices
    {
        PagingResult<BlockadeLiberationDto> FindAll(FilterBlockadeLiberationDto input);
        BlockadeLiberationDto FindById(int? id);
        BlockadeLiberation Add(CreateBlockadeLiberationDto input);
        int Update(UpdateBlockadeLiberationDto entity, int id);
    }
}
