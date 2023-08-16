using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class ViewDistributionByTradingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get;set; }
        public bool IsSalePartnership { get; set; }
    }
}
