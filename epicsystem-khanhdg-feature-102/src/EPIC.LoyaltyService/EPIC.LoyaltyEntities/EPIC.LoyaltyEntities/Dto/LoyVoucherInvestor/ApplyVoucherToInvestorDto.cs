using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor
{
    public class ApplyVoucherToInvestorDto
    {
        /// <summary>
        /// Id của voucher
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Danh sách id của investor 
        /// </summary>
        public List<int> ListInvestorId { get; set; }
    }
}
