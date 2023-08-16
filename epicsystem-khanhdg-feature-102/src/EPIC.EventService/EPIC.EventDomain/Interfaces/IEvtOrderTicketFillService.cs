using EPIC.Entities.Dto.ContractData;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    /// <summary>
    /// Xử lý fill thông tin lên vé
    /// </summary>
    public interface IEvtOrderTicketFillService
    {
        /// <summary>
        /// Fill ticket cho order chạy background
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task FillOrderTicket(int orderId);
        /// <summary>
        /// Fill cho từng vé
        /// </summary>
        /// <param name="orderTicketDetailId"></param>
        /// <returns></returns>
        Task FillTicket(int orderTicketDetailId);
        /// <summary>
        /// Fill mẫu vé sự kiện
        /// </summary>
        /// <param name="ticketTemplateId"></param>
        /// <returns></returns>
        Task<ExportResultDto> FillTemplateTicketPdf(int ticketTemplateId);
        ExportResultDto FillTemplateTicketWord(int ticketTemplateId);
    }
}
