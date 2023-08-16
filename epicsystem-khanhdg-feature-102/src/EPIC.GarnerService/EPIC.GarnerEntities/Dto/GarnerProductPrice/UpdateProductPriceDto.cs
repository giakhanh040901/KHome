using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProductPrice
{
    public class UpdateProductPriceDto
    {
        public int Id { get; set; }
        public int DitributionId { get; set; }
        public DateTime PriceDate { get; set; }
        public decimal Price { get; set; }
    }
}
