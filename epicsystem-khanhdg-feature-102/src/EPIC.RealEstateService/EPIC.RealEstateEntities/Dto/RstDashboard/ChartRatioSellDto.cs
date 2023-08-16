using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDashboard
{
    public class ChartRatioSellDto
    {
        /// <summary>
        /// tên của đại lý
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// id của phòng ban
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        ///  số lượng bán được
        /// </summary>
        public int TotalSell { get; set; }
    }
}
