using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDashboard
{
    public class GarnerDashboardCashOutByMonth
    {
        /// <summary>
        /// Tháng
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// Tiền thực chi theo tháng
        /// </summary>
        public decimal Amount { get; set; }
    }
}
