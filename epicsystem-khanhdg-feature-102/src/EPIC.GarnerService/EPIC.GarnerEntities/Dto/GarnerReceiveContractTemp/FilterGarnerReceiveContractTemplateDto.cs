using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerReceiveContractTemp
{
    public class FilterGarnerReceiveContractTemplateDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "distributionId")]
        public int DistributionId { get; set; }
    }
}
