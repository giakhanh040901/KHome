using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerContractTemplateServices
    {
        void Add(CreateGarnerContractTemplateDto input);
        void Delete(int id);
        List<GarnerContractTemplateDto> FindAll(int policyId);
        GarnerContractTemplateDto FindById(int id);
        GarnerContractTemplate Update(UpdateGarnerContractTemplateDto input);
        List<ViewContractTemplateByOrder> FindAllByOrder(long orderId, int? tradingProviderId = null);
        PagingResult<GarnerContractTemplateDto> FindAllContractTemplate(GarnerContractTemplateFilterDto input);

        /// <summary>
        /// Danh sách hợp đồng cho app
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        List<ViewContractForApp> FindAllFileSignature(long orderId);
        List<ViewContractTemplateByOrder> FindAllByOrderCheckDisplayType(long orderId);
        List<GarnerContractTemplateAppDto> FindAllForApp(int policyId, int contractType);

        /// <summary>
        /// Đổi trạng thái mẫu hợp đồng
        /// </summary>
        void ChangeStatus(int id);

        /// <summary>
        /// Lấy danh sách đang acive
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        List<GarnerContractTemplateDto> FindAllContractTemplateActive(int distributionId);
        //List<ViewContractTemplateByOrder> FindAllByOrderAdd(long orderId, int? tradingProviderId = null);
    }
}
