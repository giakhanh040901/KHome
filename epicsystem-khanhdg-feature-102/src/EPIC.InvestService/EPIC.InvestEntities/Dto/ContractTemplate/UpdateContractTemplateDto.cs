using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ContractTemplate
{
    public class UpdateContractTemplateDto
    {
        public int Id { get; set; }

        [StringRange(AllowableValues = new string[] { Utils.DisplayType.TRUOC_KHI_DUYET, EPIC.Utils.DisplayType.SAU_KHI_DUYET })]
        public string DisplayType { get; set; }

        [IntegerRange(AllowableValues = new int[] { ContractSources.ONLINE, ContractSources.OFFLINE, ContractSources.ALL })]
        public int ContractSource { get; set; }
        public int? ContractTemplateTempId { get; set; }
        public string FileUploadName { get; set; }
        public IFormFile FileUploadInvestorUrl { get; set; }
        public IFormFile FileUploadBusinessCustomerUrl { get; set; }
        public int? ContractType { get; set; }
        public int ConfigContractId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
