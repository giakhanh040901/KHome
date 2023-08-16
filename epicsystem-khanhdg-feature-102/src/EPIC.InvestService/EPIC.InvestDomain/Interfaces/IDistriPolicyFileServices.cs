using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistriPolicyFile;
using EPIC.InvestEntities.Dto.PolicyFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IDistriPolicyFileServices
    {
        DistriPolicyFile Add(CreateDistriPolicyFileDto input);
        DistriPolicyFile FindDistriPolicyFileById(int id);
        PagingResult<DistriPolicyFile> FindAllDistriPolicyFile(int distributionId, int pageSize, int pageNumber, string keyword);
        int DistriPolicyFileUpdate(int id, UpdateDistriPolicyFileDto entity);
        int DeleteDistriPolicyFile(int id);
    }
}
