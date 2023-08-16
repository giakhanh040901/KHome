using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InterestPayment
{
    /// <summary>
    /// Lập tái tục đầu tư của hợp đồng đến hạn 
    /// </summary>
    public class CreateInvestPaymentLastPeriodOrderDto : InvestInterestPaymentCreateDto
    {
        public int TradingProviderId { get;set; }
        public DateTime InvestDateNew { get; set; }
        public string ApproveIp { get; set; }
        public string CreatedBy { get; set; }
    }
}
