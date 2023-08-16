using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain
{
    public interface IOwnerServices
    {
        PagingResult<ViewOwnerDto> FindAll(int pageSize, int pageNumber, string keyword, int? status);
        ViewOwnerDto FindById(int id);
        Owner Add(CreateOwnerDto input);
        int Delete(int id);
        int Update(int issuerId, UpdateOwnerDto input);
    }
}
