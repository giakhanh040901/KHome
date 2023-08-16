using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ProductBondSecondPrice
{
    public class ProductBondSecondPriceDto
    {
        public int PriceId { get; set; }
        public int TradingProviderId { get; set; }
        public int SecondaryId { get; set; }
        public DateTime? PriceDate { get; set; }
        public decimal? Price { get; set; }
    }
}
