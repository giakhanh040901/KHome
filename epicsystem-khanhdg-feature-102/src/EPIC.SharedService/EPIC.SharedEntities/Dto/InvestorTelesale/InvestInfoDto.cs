using EPIC.InvestEntities.Dto.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedEntities.Dto.InvestorTelesale
{
    public class InvestInfoDto
    {
        /// <summary>
        /// Số tiền đầu tư ban đầu
        /// </summary>
        public decimal? InitTotalValue { get; set; }
        public DateTime? BuyDate { get; set; }
        public InvestPolicyDto Policy { get; set; }
        public InvestPolicyDetailDto PolicyDetail { get; set; }
    }
}
