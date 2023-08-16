using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EPIC.InvestEntities.Dto.Policy
{
    public class InvestPolicyFilterDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "distributionId")]
        public int DistributionId { get; set; }

        [FromQuery(Name = "status")]
        public string Status { get; set; }
    }
}
