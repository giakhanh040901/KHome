using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ContractTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IContractTemplateServices
    {
        void Add(CreateContractTemplateDto input);
        void Delete(int id);
        List<ContractTemplateDto> FindAll(int policyId);
        ContractTemplateDto FindById(int id);
        InvestContractTemplate Update(UpdateContractTemplateDto input);
        PagingResult<ContractTemplateDto> FindAllContractTemplate(ContractTemplateTempFilterDto input);
        //int ChangeStatus(int id);
        PagingResult<ContractTemplateAppDto> FindAllForApp(int policyDetailId);
        PagingResult<ViewContractForApp> FindAllFileSignatureForApp(int orderId);
        PagingResult<ViewContractTemplateByOrder> FindAllByOrderCheckDisplayType(int pageSize, int pageNumber, string keyword, int orderId);
        PagingResult<ViewContractTemplateByOrder> FindAllByOrder(int pageSize, int pageNumber, string keyword, long orderId, int? tradingProvider, int? contractType = null);

        /// <summary>
        /// Đổi trạng thái mẫu hợp đồng
        /// </summary>
        void ChangeStatus(int id);

        /// <summary>
        /// danh sách active
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        List<ContractTemplateDto> FindAllContractTemplateActive(int distributionId);
        PagingResult<ViewContractTemplateByOrder> FindAllByOrder(int pageSize, int pageNumber, string keyword, InvOrder order, int? tradingProvider, int? contractType = null, string displayType = null);
    }
}
