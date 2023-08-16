using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondContractTemplateService
    {
        BondContractTemplate FindById(int id);
        PagingResult<BondContractTemplate> FindAll(int pageSize, int pageNumber, string keyword,int bondSecondaryId,int? classify, string type);
        PagingResult<BondContractTemplate> FindAllForApp(int policyDetailId);
        PagingResult<ViewContractTemplateByOrder> FindAllByOrder(int pageSize, int pageNumber, string keyword, int orderId, int? tradingproviderId);
        BondContractTemplate Add(CreateContractTemplateDto input);
        int Update(UpdateContractTemplateDto input);
        int Delete(int id);
        int ChangeStatus(int id);
        PagingResult<ViewOrderContractForApp> FindAllFileSignatureForApp(long orderId);
    }
}
