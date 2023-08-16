using EPIC.CoreSharedEntities.Dto.TradingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class AppGarnerOrderDto
    {
        /// <summary>
        /// Id hợp đồng
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Id đại lý
        /// </summary>
        public int TradingProviderId { get; set; }

        public int TradingBankAccId { get; set; }

        /// <summary>
        /// Id phân phối sản phẩm
        /// </summary>
        public int DistributionId { get; set; }

        /// <summary>
        /// Mã Hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Tổng mức đầu tư
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Mô tả thông tin thanh toán
        /// </summary>
        public string PaymentNote { get; set; }
        public List<AppTradingBankAccountDto> TradingBankAccounts { get; set; }
    }
}
