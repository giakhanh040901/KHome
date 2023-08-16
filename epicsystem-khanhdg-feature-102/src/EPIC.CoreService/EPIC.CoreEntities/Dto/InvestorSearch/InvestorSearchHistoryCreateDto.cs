using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.InvestorSearch
{
    public class InvestorSearchHistoryCreateDto
    {
        /// <summary>
        /// ID sản phẩm của Invest
        /// </summary>
        public int? InvestDistributionId { get; set; }
        /// <summary>
        /// ID sản phẩm của Garner
        /// </summary>
        public int? GarnerPolicyId { get; set; }
        /// <summary>
        /// ID sản phẩm của BĐS (OpenSell)
        /// </summary>
        public int? RstOpenSellId { get; set; }
        /// <summary>
        /// Id sản phẩm của Event
        /// </summary>
        public int? EventId { get; set; }
    }
}
