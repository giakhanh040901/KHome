using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerShared
{
    public class CalculateProfitDto
    {
        /// <summary>
        /// Lợi tức
        /// </summary>
        public decimal Profit { get; set; }
        /// <summary>
        /// Thuế
        /// </summary>
        public decimal Tax { get; set; }
        /// <summary>
        /// Lợi tức thực nhận
        /// </summary>
        public decimal ActualProfit { get; set; }

        /// <summary>
        /// Số ngày đầu tư
        /// </summary>
        public int? InvestmentDays { get; set; }
    }
}
