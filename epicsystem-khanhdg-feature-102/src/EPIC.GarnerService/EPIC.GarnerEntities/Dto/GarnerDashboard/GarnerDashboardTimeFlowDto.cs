using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDashboard
{
    public class GarnerDashboardTimeFlowDto
    {
        /// <summary>
        /// Ngày
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Giá trị theo ngày
        /// </summary>
        public decimal Value { get; set; }
    }
}
