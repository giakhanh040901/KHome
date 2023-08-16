using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestSharedEntites.Dto.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IInvestOrderShareServices
    {
        Task<AppInvestOrderInvestorDetailDto> ViewOrderDetail(int orderId, int investorId);
        Task<AppInvestOrderInvestorDetailDto> AppSaleViewOrder(int orderId);
        List<SCInvestOrderDto> GetListInvestOrderByInvestor(int securityCompany, string stockTradingAccount, DateTime? startDate, DateTime? endDate);
        Task<List<AppTradingBankAccountDto>> TradingBankAccountOfDistribution(AppOrderDto input, int distributionId);
        Task<List<AppTradingBankAccountDto>> GetTradingBankAccountOfDistribution(int orderId, int distributionId);
    }
}
