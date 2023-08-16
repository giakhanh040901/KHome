using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.SaleInvestor
{
    public class StatisticOrderByInvestorDto
    {
        public decimal InitTotalValue { get; set; }
        public decimal TotalValue { get; set; }
        public int ProjectType { get; set; }
        /// <summary>
        /// Ngày có hiệu lực
        /// </summary>
        public DateTime ActiveDate { get; set; }
    }
}
