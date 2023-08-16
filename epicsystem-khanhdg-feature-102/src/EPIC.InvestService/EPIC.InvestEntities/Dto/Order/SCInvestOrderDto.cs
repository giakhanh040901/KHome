using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.Investor;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.InvestEntities.Dto.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    /// <summary>
    /// Dto của Hợp đồng Invest không chứa Id
    /// </summary>
    public class SCInvestOrderDto
    {
        public decimal? TotalValue { get; set; }
        /// <summary>
        /// Số tiền đầu tư ban đầu
        /// </summary>
        public decimal? InitTotalValue { get; set; }
        public DateTime? BuyDate { get; set; }
        public string SaleReferralCode { get; set; }
        public string ContractCode { get; set; }
        public DateTime? PaymentFullDate { get; set; }

        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// % lợi tức
        /// </summary>
        public decimal? Profit { get; set; }

        /// <summary>
        /// Tổng lợi nhuận thực nhận
        /// </summary>
        public decimal? AllActuallyProfit { get; set; }
        public SCInvestorBankAccountDto InvestorBankAccount { get; set; }
    }
}
