using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    /// <summary>
    /// Sale Register đã được điều hướng, xem lại thuộc DLSC nào
    /// </summary>
    public class SaleRegisterDirectionToTradingProviderDto
    {
        ///dùng cho trường hợp xem lại
        public int? TradingProviderId { get; set; }
        public string TradingProviderName { get; set; }
        public int? SaleType { get; set; }
       
    }
}
