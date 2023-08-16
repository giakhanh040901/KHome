using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class CpsInfoTradingProvider
    {
        public int Id { get; set; }
        public int CpsInfoId { get; set; }
        public int PartnerId { get; set; }
        public int TradingProviderId { get; set; }
        public decimal? TotalInvestmentSub { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
    }
}
