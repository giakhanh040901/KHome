using EPIC.Entities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    /// <summary>
    /// Báo cáo tổng chi
    /// </summary>
    public class ActualTotalSpend : ActualExpendReport
    {
        /// <summary>
        /// Tổng giá trị chi trả đầu tư trong từng ngày
        /// </summary>
        public Decimal DayPaymentAmount { get; set; }
    }
}
