using EPIC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class CpsSecondaryPrice : IFullAudited
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int SecondaryId { get; set; }
        public DateTime? PriceDate { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
    }
}
