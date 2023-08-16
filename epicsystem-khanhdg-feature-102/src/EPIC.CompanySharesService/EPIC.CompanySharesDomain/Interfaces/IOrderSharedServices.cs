using EPIC.CompanySharesEntities.Dto.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface IOrderSharedServices
    {
        List<SCOrderDto> GetListInvestOrderByInvestor(int securityCompany, string stockTradingAccount);
        AppOrderInvestorDetailDto AppSaleViewOrder(int orderId);
        AppOrderInvestorDetailDto ViewOrderDetail(int orderId, int investorId);
    }
}
