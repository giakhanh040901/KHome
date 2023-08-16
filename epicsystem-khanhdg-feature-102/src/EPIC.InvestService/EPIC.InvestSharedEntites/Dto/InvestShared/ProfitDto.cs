using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestSharedEntites.Dto.InvestShared
{
    /// <summary>
    /// Lợi tức dự kiến
    /// </summary>
    public class ProfitDto
    {
        /// <summary>
        /// Kỳ nhận lợi tức
        /// </summary>
        public int? ProfitPeriod { get; set; }

        /// <summary>
        /// Tên kỳ nhận lợi tức
        /// </summary>
        public string ProfitPeriodName { get; set; }

        public DateTime? ReceiveDate { get; set; }
        public decimal ReceiveValue { get; set; }
        public int Status { get; set; }
    }
}
