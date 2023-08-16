using EPIC.BondEntities.Dto.BondOrder;
using EPIC.Entities.Dto.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondOrderShareService
    {
        AppOrderInvestorDetailDto AppSaleViewOrder(int orderId);

        /// <summary>
        /// lấy thông tin sổ lệnh của nhà đầu tư theo mã chứng khoán
        /// </summary>
        /// <param name="securityCompany"></param>
        /// <param name="stockTradingAccount"></param>
        /// <returns></returns>
        List<SCBondOrderDto> GetListInvestOrderByInvestor(int securityCompany, string stockTradingAccount);
        AppOrderInvestorDetailDto ViewOrderDetail(int orderId, int investorId);
    }
}
