using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.TradingProvider
{
    public class ViewTradingInfoDto
    {
        public int? TradingProviderId { get; set; }
        public string TradingProviderName { get; set; }
        public string Avatar { get; set; }
        /// <summary>
        /// Tên hạng
        /// </summary>
        public string RankName { get; set; }
        /// <summary>
        /// Điểm tổng theo trading
        /// </summary>
        public int? TotalPoint { get; set; }
        /// <summary>
        /// Điểm hiện tại theo trading
        /// </summary>
        public int? CurrentPoint { get; set; }
    }
}
