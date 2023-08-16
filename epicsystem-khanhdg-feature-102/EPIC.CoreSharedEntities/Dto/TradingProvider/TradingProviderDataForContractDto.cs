using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.TradingProvider
{
    public class TradingProviderDataForContractDto
    {
        /// <summary>
        /// Thông tin đại lý sơ cấp
        /// </summary>
        public EPIC.Entities.DataEntities.TradingProvider TradingProvider { get; set; }
        /// <summary>
        /// Thông tin doanh nghiệp
        /// </summary>
        public BusinessCustomerDto BusinessCustomerTrading { get; set; }
        /// <summary>
        /// thông tin tài khoản thụ hưởng
        /// </summary>
        public BusinessCustomerBankDto TradingBank { get; set; }
    }
}
