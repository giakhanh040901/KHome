using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    /// <summary>
    /// Lấy danh sách order được nhóm bởi Chính sách
    /// </summary>
    public class AppGarnerOrderByPolicyDto
    {
        /// <summary>
        /// Id chính sách
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string PolicyName { get; set; }
        
        /// <summary>
        /// Tên đại lý
        /// </summary>
        public string TradingProviderName { get; set; }

        /// <summary>
        /// Icon của sản phẩm tích lũy
        /// </summary>
        public string ProductIcon { get; set; }

        /// <summary>
        /// Tổng số dư hiện tại
        /// </summary>
        public decimal TotalCurrentBalance { get; set; }

        /// <summary>
        /// Tổng lợi nhuận đến hiện tại
        /// </summary>
        public decimal TotalCurrentProfit { get; set; }

        /// <summary>
        /// Thời gian tích lũy (số ngày)
        /// </summary>
        public int CumulativeDays { get; set; }

        /// <summary>
        /// Số tiền rút tối thiểu
        /// </summary>
        public decimal MinWithdraw { get; set; }

        /// <summary>
        /// Số tiền rút tối đa
        /// </summary>
        public decimal? MaxWithdraw { get; set; } 

        /// <summary>
        /// Danh sách hợp đồng có trong nhóm chính sách này
        /// </summary>
        public List<AppGarnerOrderListDto> Orders { get; set; }
    }
}
