using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDashboard
{
    public class GarnerDashboardCashInByTrading
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
        /// Id phòng ban
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }
    }
}
