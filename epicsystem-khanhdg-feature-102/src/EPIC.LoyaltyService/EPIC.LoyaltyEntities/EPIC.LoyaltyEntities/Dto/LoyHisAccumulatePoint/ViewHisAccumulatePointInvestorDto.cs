using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class ViewHisAccumulatePointInvestorDto
    {
        public int InvestorId { get; set; }
        public string Fullname { get; set; }
        public string IdNo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// Tổng điểm
        /// </summary>
        public int? LoyTotalPoint { get; set; }
        /// <summary>
        /// Điểm hiện tại
        /// </summary>
        public int? LoyCurrentPoint { get; set; }
    }
}
