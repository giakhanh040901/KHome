using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    /// <summary>
    /// Sale đặt lệnh hộ cho nhà đầu tư cá nhân
    /// </summary>
    public class AppSaleCreateRstOrderDto : AppCreateRstOrderDto
    {
        /// <summary>
        /// Investor id của khách hàng được sale đặt lệnh hộ
        /// </summary>
        public int InvestorId { get; set; }
    }
}
