using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstContractTemplateTemp
{
    public class RstContractTemplateTempDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TradingProviderId { get; set; }
        public int? PartnerId { get; set; }
        public int ContractType { get; set; }
        public string Status { get; set; }
        public int ContractSource { get; set; }
        public string FileInvestor { get; set; }
        public string FileBusinessCustomer { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
