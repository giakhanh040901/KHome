using EPIC.InvestEntities.Dto.ContractTemplateTemp;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ContractTemplate
{
    public class ContractTemplateDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int PolicyId { get; set; }
        public string DisplayType { get; set; }
        public int ContractSource { get; set; }
        public int? ContractTemplateTempId { get; set; }
        public int ConfigContractId { get; set; }
        public string FileUploadName { get; set; }
        public string FileUploadInvestorUrl { get; set; }
        public string FileUploadBusinessCustomerUrl { get; set; }
        public int? ContractType { get; set; }
        public string ContractTemplateName { get; set; }
        public string ConfigContractName { get; set; }
        public string PolicyName { get; set; }
        public DateTime? StartDate { get; set; }
        public PolicyDto Policy { get; set; }
        public InvestContractTemplateTempDto ContractTemplateTemp { get; set; }
        public InvConfigContractCodeDto ConfigContractCode { get; set; }
    }
}
