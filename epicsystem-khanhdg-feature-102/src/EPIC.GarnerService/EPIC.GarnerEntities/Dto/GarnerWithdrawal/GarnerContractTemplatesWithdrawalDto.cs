using EPIC.GarnerEntities.Dto.GarnerContractTemplateApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    public class GarnerContractTemplatesWithdrawalDto
    {
        public long OrderId { get; set; }
        public string ContractCode { get; set; }
        public List<GarnerContractTemplateAppDto> GarnerContractTemplates { get; set; }
    }
}
