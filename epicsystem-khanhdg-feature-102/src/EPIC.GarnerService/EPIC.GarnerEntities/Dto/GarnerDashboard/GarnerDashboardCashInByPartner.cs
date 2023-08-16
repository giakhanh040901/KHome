using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDashboard
{
    public class GarnerDashboardCashInByPartner
    {
        /// <summary>
        /// Số dư
        /// </summary>
        public decimal Amout { get; set; }
        /// <summary>
        /// Tiền thừa
        /// </summary>
        public decimal Remain { get; set; }
        /// <summary>
        /// Id đại lý
        /// </summary>
        public int TradingProviderId { get; set; }
        /// <summary>
        /// Tên đại lý
        /// </summary>
        public string TradingProviderName { get; set; }
        /// <summary>
        /// Tên viết tắt của đại lý
        /// </summary>
        public string TradingProviderShortName { get; set; }
    }
}
