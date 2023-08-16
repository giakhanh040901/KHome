using EPIC.EntitiesBase.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerHistory
{
    public class FilterGarnerHistoryDto : PagingRequestBaseDto
    {
        public int UploadTable { get; set; }
        public int? RealTableId { get; set; }
    }
}
