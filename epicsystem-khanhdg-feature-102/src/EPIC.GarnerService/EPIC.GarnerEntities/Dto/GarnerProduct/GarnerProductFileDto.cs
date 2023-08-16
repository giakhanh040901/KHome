using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProduct
{
    public class GarnerProductFileDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int DocumentType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public decimal? TotalValue { get; set; }
        public string Description { get; set; }
        public string Deleted { get; set; }
    }
}
