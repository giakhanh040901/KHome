using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerPolicy
{
    public class GarnerPolicyMoreInfoDto : GarnerPolicyDto
    {
        public List<GarnerPolicyDetailDto> PolicyDetails { get; set; }
        public List<GarnerPolicyDetailTempDto> PolicyDetailTemps { get; set; }
        public List<GarnerContractTemplateTempDto> ContractTemplateTemps { get; set; }
        public List<GarnerContractTemplateDto> ContractTemplates { get; set; }
        public GarnerDistributionDto Distribution { get; set; }
    }

}
