using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Order
{
    /// <summary>
    /// Lợi tức dự kiến
    /// </summary>
    public class ProfitAppDto
    {
        /// <summary>
        /// Kỳ nhận lợi tức
        /// </summary>
        public int? ProfitPeriod { get; set; }
        /// <summary>
        /// Kỳ nhận trái tức
        /// </summary>
        public int? CouponPeriod { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public decimal ReceiveValue { get; set; }
        public int Status { get; set; }
    }
}
