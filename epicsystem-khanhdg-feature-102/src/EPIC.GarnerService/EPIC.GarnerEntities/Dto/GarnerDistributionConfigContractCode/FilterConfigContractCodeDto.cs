using EPIC.EntitiesBase.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode
{
    public class FilterConfigContractCodeDto : PagingRequestBaseDto
    {
        public string Status { get; set; }
    }
}
