using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.SharedEntities.Dto.InvestorOrder;
using System;

namespace EPIC.SharedDomain.Interfaces
{
    public interface IInvestorSharedTpsServices
    {
        /// <summary>
        /// Lấy thông tin giấy tờ nhà đầu tư theo tài khoản chứng khoán
        /// </summary>
        /// <param name="securityCompany"></param>
        /// <param name="stockTradingAccount"></param>
        /// <returns></returns>
        SCInvestorIdentificationDto InvestorIdenByStockTradingAccount(int securityCompany, string stockTradingAccount);

        /// <summary>
        /// Lấy thông tin sổ lệnh của nhà đầu tư theo tài khoản chứng khoán
        /// </summary>
        /// <param name="securityCompany"></param>
        /// <param name="stockTradingAccount"></param>
        /// <returns></returns>
        ListOrderInvestorByStockTradingAccountDto ListOrderInvestorByStockTradingAccount(int securityCompany, string stockTradingAccount, DateTime? startDate, DateTime? endDate);
    }
}
