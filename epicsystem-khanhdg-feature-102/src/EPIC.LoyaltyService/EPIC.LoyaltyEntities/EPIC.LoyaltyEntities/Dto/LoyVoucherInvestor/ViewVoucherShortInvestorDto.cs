using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor
{
    public class ViewVoucherShortInvestorDto
    {
        public int VoucherInvestorId { get; set; }
        public int InvestorId { get; set; }
        /// <summary>
        /// Ngày giao
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
