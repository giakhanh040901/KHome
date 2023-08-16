using EPIC.InvestEntities.Dto.InvConfigContractCodeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvConfigContractCode
{
    public class InvConfigContractCodeDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<InvConfigContractCodeDetailDto> ConfigContractCodeDetails { get; set; }
    }
}
