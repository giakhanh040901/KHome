using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedServices.Interfaces
{
    public interface ILogicInvestorTradingSharedServices
    {
        List<int> FindListTradingProviderForApp(int? investorId, bool isSaleView = false);
        List<int> FindListInvestorTradingProviderForApp(int? investorId);
        List<int> FindListSaleTradingProviderForApp(int? investorId);
    }
}
