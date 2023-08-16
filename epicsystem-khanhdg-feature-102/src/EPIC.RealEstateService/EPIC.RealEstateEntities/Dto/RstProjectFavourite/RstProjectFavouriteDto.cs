using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectFavourite
{
    public class RstProjectFavouriteDto
    {
        /// <summary>
        /// Id mở bán
        /// </summary>
        public int OpenSellId { get; set; }
        /// <summary>
        /// ID khách hàng
        /// </summary>
        public int InvestorId { get; set; }
    }
}
