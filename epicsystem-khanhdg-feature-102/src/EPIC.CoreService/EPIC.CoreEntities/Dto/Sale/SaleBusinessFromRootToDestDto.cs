using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class SaleBusinessFromRootToDestDto
    {
        public int TradingProviderId { get; set; }
        public int BusinessCustomerId { get; set; }
        public string ReferralCode { get; set; }
    }
}
