using EPIC.InvestEntities.Dto.InvestShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class InvestOrderCashFlowDto
    {
        /// <summary>
        /// Dòng tiền dự tính
        /// </summary>
        public ProfitAndInterestDto ExpectedCashFlow { get; set; }
        /// <summary>
        /// Dòng tiền thực tế
        /// </summary>
        public List<InvestOrderInterestAndWithdrawalDto> ActualCashFlow { get; set; }
    }
}
