using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor
{
    public class DataNotificationAddVoucherToInvestorDto
    {
        public int InvestorId { get; set; }
        public int? TradingProviderId { get; set; }
        public string Fullname { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string VoucherType { get; set; }
        public string VoucherName { get; set; }
    }
}
