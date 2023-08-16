using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    /// <summary>
    /// Thông kê doanh số trong ngày
    /// </summary>
    public class AppStatisticOrderInDay
    {
        /// <summary>
        /// Doanh số
        /// </summary>
        public decimal InitTotalValue { get; set; }

        /// <summary>
        /// Ngày
        /// </summary>
        public DateTime Date { get; set; }
    }
}
