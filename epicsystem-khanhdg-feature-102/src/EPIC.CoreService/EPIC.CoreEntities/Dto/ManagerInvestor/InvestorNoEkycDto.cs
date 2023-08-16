using EPIC.Entities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    public class InvestorNoEkycDto
    {
        public int InvestorId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PlainPhone { get; set; }
        public string PlainEmail { get; set; }
        public string ReferralCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? Source { get; set; }

        /// <summary>
        /// Trạng thái của user
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Trạng thái của nhà đầu tư
        /// </summary>
        public string InvestorStatus { get; set; }
        //public int? TradingProviderId { get; set; }
        public int? Step { get; set; }
        public int? UserId { get; set; }
        

    }
}
