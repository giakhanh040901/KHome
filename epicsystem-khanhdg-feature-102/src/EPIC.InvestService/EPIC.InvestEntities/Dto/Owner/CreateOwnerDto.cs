using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Owner
{
    public class CreateOwnerDto
    {
        public int BusinessCustomerId { get; set; }
        public int? PartnerId { get; set; }
        public decimal? BusinessTurnover { get; set; }
        public decimal? BusinessProfit { get; set; }
        public decimal? Roa { get; set; }
        public decimal? Roe { get; set; }
        public string Image { get; set; }
        public string Website { get; set; }
        public string Hotline { get; set; }
        public string Fanpage { get; set; }
    }
}
