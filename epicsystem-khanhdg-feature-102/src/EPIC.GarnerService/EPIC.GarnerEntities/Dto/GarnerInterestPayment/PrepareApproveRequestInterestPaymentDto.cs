using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerInterestPayment
{
    public class PrepareApproveRequestInterestPaymentDto 
    {
        public int? TradingBankAccId { get; set; }
        public List<long> InterestPaymentIds { get; set; }
    }
}
