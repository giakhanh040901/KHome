using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerDistributionSharedServices
    {
        bool CheckAddTotalValue(int distributionId, decimal totalValue);
        GarnerProductTradingProviderDto LimitCalculationTrading(int productId, int tradingProviderId);
    }
}
