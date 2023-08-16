using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerContractTemplate
{
    public class GarnerContractTemplateForOrderDto
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public int TradingProviderId { get; set; }
        public string ContractTemplateUrl { get; set; }
        public int? ContractTemplateTempId { get; set; }
        public string Name { get; set; }
        public string FileInvestor { get; set; }
        public string FileBusinessCustomer { get; set; }
        public int ContractSource { get; set; }
        public int ContractType { get; set; }
        public int ConfigContractId { get; set; }
        public string DisplayType { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
