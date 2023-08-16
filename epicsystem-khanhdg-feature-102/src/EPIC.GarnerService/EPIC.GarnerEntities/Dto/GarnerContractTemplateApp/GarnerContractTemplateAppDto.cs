using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerContractTemplateApp
{
    public class GarnerContractTemplateAppDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int PolicyId { get; set; }
        public string Name { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ConfigContractId { get; set; }
        public int ContractTemplateTempId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
