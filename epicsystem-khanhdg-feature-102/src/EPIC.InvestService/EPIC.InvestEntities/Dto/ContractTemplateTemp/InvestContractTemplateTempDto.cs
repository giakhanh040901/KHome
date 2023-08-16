using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ContractTemplateTemp
{
    public class InvestContractTemplateTempDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ContractType { get; set; }
        public string Status { get; set; }
        public int ContractSource { get; set; }
        public string FileInvestor { get; set; }
        public string FileBusinessCustomer { get; set; }
        public string Description { get; set; }
    }
}
