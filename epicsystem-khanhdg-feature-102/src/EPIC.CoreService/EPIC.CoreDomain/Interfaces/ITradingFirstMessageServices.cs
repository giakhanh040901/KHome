using EPIC.CoreEntities.Dto.TradingFirstMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ITradingFirstMessageServices
    {
        public void SaveMessage(SaveTradingFirstMessageDto dto);
        public List<ViewTradingFirstMessageDto> FindAll();
        public ViewTradingFirstMessageDto FindByTrading(int tradingProviderId);
    }
}
