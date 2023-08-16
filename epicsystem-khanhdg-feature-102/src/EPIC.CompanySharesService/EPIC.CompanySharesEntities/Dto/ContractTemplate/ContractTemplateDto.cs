using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.ContractTemplate
{
    public class ContractTemplateDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int DistributionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ContractTempUrl { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ContractTypeName { get; set; }
        public string Status { get; set; }
        public string Deleted { get; set; }
    }
}
