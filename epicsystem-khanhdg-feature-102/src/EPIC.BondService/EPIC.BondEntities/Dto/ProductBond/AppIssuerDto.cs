using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ProductBond
{
    public class AppIssuerDto
    {
        /// <summary>
        /// Bảng BusinessCustomer
        /// </summary>
        public string Name { get; set; }
        public string TradingAddress { get; set; }
        public string RepName { get; set; }
        public decimal? Capital { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Fanpage { get; set; }
        /// <summary>
        /// Bảng BondIssuer
        /// </summary>
        public int IssuerId { get; set; }
        public decimal? BusinessTurover { get; set; }
        public decimal? BusinessProfit { get; set; }
        public decimal? ROA { get; set; }
        public decimal? ROE { get; set; }
        public string Image { get; set; }
    }
}
