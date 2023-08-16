using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedEntities.Dto.InvestorTelesale
{
    public class InvestorTelesaleDto
    {
        public string IdNo { get; set; }
        public string Fullname { get; set; }
        public List<InvestInfoDto> InvestInfo { get; set; }
        public List<GarnerInfoDto> GarnerInfo { get; set; }
    }
}
