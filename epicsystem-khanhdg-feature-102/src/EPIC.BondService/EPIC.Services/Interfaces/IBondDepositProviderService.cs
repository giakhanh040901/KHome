using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.DepositProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondDepositProviderService
    {
        DepositProviderDto FindById(int id);
        PagingResult<DepositProviderDto> FindAll(int pageSize, int pageNumber, string keyword, string status);
        BondDepositProvider Add(CreateDepositProviderDto input);
        int Delete(int id);
    }
}
