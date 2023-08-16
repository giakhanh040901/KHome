using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InterestPayment
{
    public class PrepareApproveRequestInterestPaymentDto
    {
        public int? TradingBankAccId { get; set; }
        public List<long> InterestPaymentIds { get; set; }
    }
}
