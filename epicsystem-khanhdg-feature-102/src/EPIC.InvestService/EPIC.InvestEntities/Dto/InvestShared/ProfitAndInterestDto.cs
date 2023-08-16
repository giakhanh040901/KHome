using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestShared
{
    public class ProfitAndInterestDto
    {
        public List<ProfitInfoDto> ProfitInfo { get; set; } = new();
        
        /// <summary>
        /// Lợi tức cả kỳ
        /// </summary>
        public decimal AllProfit { get; set; }
        
        /// <summary>
        /// Thuế lợi tức cả kỳ
        /// </summary>
        public decimal TaxProfit { get; set; }
        
        /// <summary>
        /// Lợi tức thực nhận cả kỳ bao gồm thuế (E = A * D * B/365)
        /// </summary>
        public decimal ActuallyProfit { get; set; }
        
        /// <summary>
        /// Tiền tạm ứng trong kỳ
        /// </summary>
        public decimal AdvancePayment { get; set; }
    }
}
