using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Issuer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondIssuerService
    {
        PagingResult<ViewIssuerDto> FindAll(int pageSize, int pageNumber, bool isNoPaging, string keyword, string status);
        ViewIssuerDto FindById(int id);
        BondIssuer Add(CreateIssuerDto input);
        int Delete(int id);
        int Update(int issuerId, UpdateIssuerDto input);
    }
}
