using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BondShared
{
    public class CalculateProfitDto
    {
        public decimal Profit { get; set; }
        public decimal Tax { get; set; }
        public decimal ActuallyProfit { get; set; }
    }
}
