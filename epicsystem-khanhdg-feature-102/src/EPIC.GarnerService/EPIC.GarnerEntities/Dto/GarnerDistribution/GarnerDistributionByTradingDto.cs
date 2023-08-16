using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDistribution
{
    public class GarnerDistributionByTradingDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set;}

        /// <summary>
        /// Có phải là bán hộ hay không
        /// </summary>
        public bool IsSalePartnership { get; set; }
    }
}
