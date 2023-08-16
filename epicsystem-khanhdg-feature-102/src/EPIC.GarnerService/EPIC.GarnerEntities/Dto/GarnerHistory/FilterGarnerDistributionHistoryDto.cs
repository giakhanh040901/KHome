using EPIC.EntitiesBase.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerHistory
{
    public class FilterGarnerDistributionHistoryDto : PagingRequestBaseDto
    {
        public int DistributionId { get; set; }
    }
}
