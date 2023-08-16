using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventMedia;
using EPIC.EventEntites.Dto.EvtOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtAppOrderService
    {
        IEnumerable<AppEventDetailDto> FindEventDetailsById(int id, bool isSaleView);
        Task<int> Add(AppCreateEvtOrderDto input, bool isSelfDoing, int? investorId);
        Task<IEnumerable<AppTradingBankAccountDto>> TradingBankAccountOfEvent(int orderId, int eventId, string contractCode, decimal totalValue);
        void CancelOrder(int orderId);

        /// <summary>
        /// app yeu cau nhan hoa don, ve cung
        /// </summary>
        /// <param name="input"></param>
        Task InvoiceTicketRequest(InvoiceTicketRequestDto input);
    }
}
