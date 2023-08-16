using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtEventHistory;
using EPIC.EventEntites.Dto.EvtOrderTicketDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtAppEventHistoryServices
    {
        PagingResult<AppViewEventHistoryDto> FindAll(FilterEvtEventHistoryDto input);
        AppViewEventHistoryDetailDto FindByOrderId(int orderId);
        IEnumerable<AppViewOrderTicketDetailDto> FindByOrderDetailId(int orderDetailId);
        Task<IEnumerable<AppTradingBankAccountDto>> FindTradingBankAccountOfEvent(int orderId);
    }
}
