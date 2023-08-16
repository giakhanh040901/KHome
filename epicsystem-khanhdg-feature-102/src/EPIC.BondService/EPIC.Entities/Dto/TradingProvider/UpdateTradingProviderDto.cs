using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.TradingProvider
{
    public class UpdateTradingProviderDto : CreateTradingProviderDto
    {
        public int TradingProviderId { get; set; }
    }
}
