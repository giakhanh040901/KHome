using EPIC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class CpsIssuer: IFullAudited
    {
        public int Id { get; set; }
        public int BusinessCustomerId { get; set; }
        public int? PartnerId { get; set; }
        public int? Status { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? BusinessTurnover { get; set; }
        public decimal? BusinessProfit { get; set; }
        public decimal? ROA { get; set; }
        public decimal? ROE { get; set; }
        public string Image { get; set; }
        public DateTime? CreatedDate { get; set ; }
    }

}