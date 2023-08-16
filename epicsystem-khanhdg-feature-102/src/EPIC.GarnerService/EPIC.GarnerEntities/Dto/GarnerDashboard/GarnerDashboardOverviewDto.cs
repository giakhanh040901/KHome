using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDashboard
{
    /// <summary>
    /// Tổng quan Dashboard
    /// </summary>
    public class GarnerDashboardOverviewDto
    {
        /// <summary>
        /// Tiền vào/ra trong ngày
        /// </summary>
        public decimal MoneyDay { get; set; }
        /// <summary>
        /// Lũy kế
        /// </summary>
        public decimal Cummulative { get; set; }
    }
}
