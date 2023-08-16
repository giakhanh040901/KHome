using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.PolicyFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondPolicyFileService
    {
        BondPolicyFile Add(CreatePolicyFileDto input);
        BondPolicyFile FindPolicyFileById(int id);
        PagingResult<BondPolicyFile> FindAllPolicyFile(int bondSecondaryId, int pageSize, int pageNumber, string keyword);
        int PolicyFileUpdate(int id, UpdatePolicyFileDto entity);
        int DeletePolicyFile(int id);
    }
}
