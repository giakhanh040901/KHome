using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.ContractData;
using EPIC.EntitiesBase.Dto;
using EPIC.EventEntites.Dto.EvtDeliveryTicketTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    /// <summary>
    /// Giao nhận vé
    /// </summary>
    public interface IEvtDeliveryTicketTemplateService
    {
        /// <summary>
        /// Thêm mẫu giao nhận
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EvtDeliveryTicketTemplateDto Add(CreateDeliveryTicketTemplateDto input);
        /// <summary>
        /// Cập nhật mẫu giao nhận
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EvtDeliveryTicketTemplateDto Update(UpdateDeliveryTicketTemplateDto input);
        /// <summary>
        /// Xóa mẫu giao nhận
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// ĐỔi trạng thái mẫu giao nhận
        /// </summary>
        /// <param name="id"></param>
        void ChangeStatus(int id);
        /// <summary>
        /// TÌm theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EvtDeliveryTicketTemplateDto FindById(int id);
        /// <summary>
        /// Danh sách mẫu giao nhận vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EvtDeliveryTicketTemplateDto> FindAll(FilterDeliveryTicketTemplateDto input);
        /// <summary>
        /// Test fill mẫu hợp đồng giao nhận
        /// </summary>
        /// <param name="deliveryTicketTemplateId"></param>
        /// <returns></returns>
        Task<ExportResultDto> FillDeliveryTemplateTicketPdf(int deliveryTicketTemplateId);
        /// <summary>
        /// Test fill mẫu hợp đồng giao nhận
        /// </summary>
        /// <param name="deliveryTicketTemplateId"></param>
        /// <returns></returns>
        ExportResultDto FillDeliveryTemplateTicketWord(int deliveryTicketTemplateId);
        /// <summary>
        /// fill mẫu hợp đồng giao nhận
        /// </summary>
        /// <param name="deliveryTicketTemplateId"></param>
        /// <returns></returns>
        Task<ExportResultDto> FillDeliveryTicket(int orderId);
    }
}
