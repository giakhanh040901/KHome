using EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstConfigContractCode
{
    public class RstConfigContractCodeDto
    {
        public int Id { get; set; }
        public int? TradingProviderId { get; set; }
        public int? PartnerId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<RstConfigContractCodeDetailDto> ConfigContractCodeDetails { get; set; }
    }
}
