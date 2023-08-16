using EPIC.GarnerEntities.Dto.GarnerPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class AppGarnerPolicyGroupOrderDto
    {
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
        public List<EPIC.GarnerEntities.DataEntities.GarnerOrder> Orders { get; set; }
    }
}
