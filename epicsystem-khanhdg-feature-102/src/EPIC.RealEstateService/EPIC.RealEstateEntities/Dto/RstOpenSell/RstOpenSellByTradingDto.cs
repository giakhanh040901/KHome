using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSell
{
    public class RstOpenSellByTradingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// có phải bán hộ hay không
        /// </summary>
        public bool IsSalePartnership { get; set; }
    }
}
