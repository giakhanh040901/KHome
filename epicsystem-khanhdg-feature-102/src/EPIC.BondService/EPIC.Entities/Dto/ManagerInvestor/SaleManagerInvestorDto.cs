using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Sale;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class SaleManagerInvestorDto : ViewManagerInvestorTemporaryDto
    {
        /// <summary>
        /// Thông tin sale (Có cả department)
        /// </summary>
        public ViewSaleDto Sale { get; set; }

        /// <summary>
        /// Quan hệ của Investor quét sale
        /// </summary>
        public SaleInfoByInvestorDto InvestorSale { get; set; }

        public BusinessCustomerDto BusinessCustomer { get; set; }
    }
}
