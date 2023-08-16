using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerContractTemplate
{
    public class GarnerContractTemplateDto
    {
        public int Id { get; set; }

        [Obsolete("bỏ")]
        public string Code { get; set; }

        [Obsolete("bỏ")]
        public string Name { get; set; }

        [Obsolete("bỏ")]
        public string ContractTempUrl { get; set; }
        public int PolicyId { get; set; }

        [Obsolete("bỏ")]
        public string Type { get; set; }
        public string DisplayType { get; set; }

        [Obsolete("bỏ")]
        public int ContractType { get; set; }
        public string Status { get; set; }
        public int ContractSource { get; set; }
        public int ContractTemplateTempId { get; set; }
        public int ConfigContractId { get; set; }
        public DateTime? StartDate { get; set; }
        public GarnerPolicyDto Policy { get; set; }
        public GarnerContractTemplateTempDto ContractTemplateTemp { get; set; }
        public GarnerConfigContractCodeDto ConfigContractCode { get; set; }
    }
}
