using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.Issuer;
using EPIC.DataAccess.Models;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface ICpsIssuerService
    {
        CpsIssuer Add(CreateCpsIssuerDto input);
        int Delete(int id);
        int Update(int issuerId, UpdateCpsIssuerDto input);
        ViewCpsIssuerDto FindById(int id);
        PagingResult<ViewCpsIssuerDto> FindAll(CpsIssuerFilterDto input);
    }
}
