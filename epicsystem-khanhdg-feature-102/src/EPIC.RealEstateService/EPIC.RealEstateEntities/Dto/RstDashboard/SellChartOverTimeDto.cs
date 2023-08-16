using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDashboard
{
    public class SellChartOverTimeDto
    {
        /// <summary>
        /// ngày bán
        /// </summary>
        public DateTime? DepositDate { get; set; }
        /// <summary>
        /// tổng giá bán trong ngày
        /// </summary>
        public decimal? TotalPrice { get; set; }
        /// <summary>
        /// tổng số căn hộ được bán trong ngày
        /// </summary>
        public int Total { get; set; }
    }
}
