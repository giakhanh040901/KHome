using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Dashboard
{
    public class GenerationCustomerChartDto
    {
        public string Month { get; set; }
        public int GenerationX { get; set; }
        public int Millennial { get; set; }
        public int GenerationZ { get; set; }
        public int BabyBoomer { get; set; }
    }
}
