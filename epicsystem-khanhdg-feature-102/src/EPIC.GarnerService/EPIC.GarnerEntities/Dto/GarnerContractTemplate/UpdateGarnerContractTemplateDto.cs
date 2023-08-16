using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerContractTemplate
{
    public class UpdateGarnerContractTemplateDto
    {
        public int Id { get; set; }

        [StringRange(AllowableValues = new string[] { Utils.DisplayType.TRUOC_KHI_DUYET, EPIC.Utils.DisplayType.SAU_KHI_DUYET })]
        public string DisplayType { get; set; }

        [IntegerRange(AllowableValues = new int[] { ContractSources.ONLINE, ContractSources.OFFLINE, ContractSources.ALL })]
        public int ContractSource { get; set; }
        public int ContractTemplateTempId { get; set; }
        public int ConfigContractId { get; set; }
        public int PolicyId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
