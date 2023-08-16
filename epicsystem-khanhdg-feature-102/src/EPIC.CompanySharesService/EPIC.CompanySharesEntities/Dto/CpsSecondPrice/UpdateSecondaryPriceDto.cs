using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsSecondPrice
{
    public class UpdateSecondaryPriceDto
    {
        public int PriceId { get; set; }
        public int TradingProviderId { get; set; }
        public int CpsSecondaryId { get; set; }
        public DateTime? PriceDate { get; set; }
        public decimal? Price { get; set; }
    }
}
