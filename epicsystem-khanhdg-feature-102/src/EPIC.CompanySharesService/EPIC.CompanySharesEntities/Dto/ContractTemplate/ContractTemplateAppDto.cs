using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.ContractTemplate
{
    public class ContractTemplateAppDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int DistributionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
