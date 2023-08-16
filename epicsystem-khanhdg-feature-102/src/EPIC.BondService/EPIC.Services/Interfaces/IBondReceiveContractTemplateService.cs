using EPIC.BondEntities.DataEntities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ReceiveContractTemplate;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondReceiveContractTemplateService
    {
        BondReceiveContractTemplate FindById(int id);
        BondReceiveContractTemplate FindBySecondaryId(int id);
        //PagingResult<BondReceiveContractTemplate> FindAll(int pageSize, int pageNumber, string keyword,int bondSecondaryId,int? classify);
        //PagingResult<BondReceiveContractTemplate> FindAllForApp(int policyDetailId);
        //PagingResult<ViewContractTemplateByOrder> FindAllByOrder(int pageSize, int pageNumber, string keyword, int orderId, int? tradingproviderId);
        BondReceiveContractTemplate Add(CreateReceiveContractTemplateDto input);
        int Update(UpdateReceiveContractTemplateDto input);
        int Delete(int id);
        int ChangeStatus(int id);
        /*        PagingResult<ViewOrderContractForApp> FindAllFileSignatureForApp(long orderId);*/
    }
}
