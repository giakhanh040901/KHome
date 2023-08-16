using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.ContractTemplate;
using EPIC.DataAccess.Models;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface IContractTemplateServices
    {
        ContractTemplate FindById(int id);
        public PagingResult<ContractTemplate> FindAll(int pageSize, int pageNumber, string keyword, int distributionId, string type);
        ContractTemplate Add(CreateContractTemplateDto input);
        int Update(UpdateContractTemplateDto input);
        int Delete(int id);
        int ChangeStatus(int id);
        PagingResult<ViewContractTemplateByOrder> FindAllByOrder(int pageSize, int pageNumber, string keyword, int orderId, int? tradingProvider);
        PagingResult<ContractTemplateAppDto> FindAllForApp(int policyDetailId);
        PagingResult<ViewContractForApp> FindAllFileSignatureForApp(int orderId);
        PagingResult<ViewContractTemplateByOrder> FindAllByOrderCheckDisplayType(int pageSize, int pageNumber, string keyword, int orderId, int? tradingProvider);
    }
}
