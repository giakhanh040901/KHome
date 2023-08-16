using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstContractTemplate
{
    public class FilterRstContractTemplateDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "distributionId")]
        public int DistributionId { get; set; }

        [FromQuery(Name = "policyId")]
        public int? PolicyId { get; set; }

        [FromQuery(Name = "contractSource")]
        public int? ContractSource { get; set; }

        [FromQuery(Name = "status")]
        public string Status { get; set; }
    }
}
