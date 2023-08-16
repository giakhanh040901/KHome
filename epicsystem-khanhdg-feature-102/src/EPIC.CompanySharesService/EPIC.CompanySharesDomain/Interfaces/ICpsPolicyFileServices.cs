using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.PolicyFile;
using EPIC.DataAccess.Models;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface ICpsPolicyFileServices
    {
        CpsPolicyFile Add(CreateCpsPolicyFileDto input);
        CpsPolicyFile FindPolicyFileById(int id);
        PagingResult<CpsPolicyFile> FindAllPolicyFile(CpsPolicyFileFilterDto input);
        int PolicyFileUpdate(int id, UpdateCpsPolicyFileDto entity);
        int DeletePolicyFile(int id);
    }
}
