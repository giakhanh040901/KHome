using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.TradingFirstMessage
{
    public class SaveTradingFirstMessageDto
    {
        private string _message;
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string Message { get => _message; set => _message = value?.Trim(); }
    }
}
