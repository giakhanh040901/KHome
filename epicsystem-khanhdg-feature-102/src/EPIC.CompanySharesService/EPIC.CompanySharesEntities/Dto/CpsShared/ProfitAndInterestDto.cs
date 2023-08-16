using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CompanySharesShared
{
    public class ProfitAndInterestDto
    {
        public List<ProfitInfoDto> ProfitInfo { get; set; } = new();
        public List<DividendInfoDto> DividendInfo { get; set; } = new();

        /// <summary>
        /// Lợi tức cả kỳ = ActuallyProfit * (1 - thuế lợi nhuận)
        /// </summary>
        public decimal AllProfit { get; set; }
        /// <summary>
        /// Thuế lợi tức cả kỳ
        /// </summary>
        public decimal TaxProfit { get; set; }
        /// <summary>
        /// Lợi tức thực nhận cả kỳ là không có thuế A * D * B/365
        /// </summary>
        public decimal ActuallyProfit { get; set; }
        /// <summary>
        /// Tiền tạm ứng trong kỳ (bỏ kỳ cuối)
        /// </summary>
        public decimal AdvancePayment { get; set; }
    }
}
