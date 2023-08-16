using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMediaDetail
{
    public class EvtEventMediaDetailSortDto
    {
        public int EventId { get; set; }
        public List<EvtEventMediaDetailSortOrderDto> Sort { get; set; }
    }
}
