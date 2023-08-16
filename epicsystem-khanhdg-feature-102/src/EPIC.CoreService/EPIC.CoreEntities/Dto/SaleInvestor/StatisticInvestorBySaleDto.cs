using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.SaleInvestor
{
    public class StatisticInvestorBySaleDto
    {
        /// <summary>
        /// Danh sách Sale
        /// </summary>
        public List<ViewInvestorsBySaleDto> Investors { get; set; }

        /// <summary>
        /// Thống kê biểu đồ 
        /// </summary>
        public List<StatisticOrderByInvestorWithTimeDto> StatisticInvestorWithTime { get; set; }
    }
}
