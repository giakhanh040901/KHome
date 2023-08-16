using EPIC.EventEntites.Dto.EvtEventMediaDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMedia
{
    public class EvtEventMediaDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string GroupTitle { get; set; }
        public IEnumerable<EvtEventMediaDetailDto> EvtEventMediaDetail { get; set; }
    }
}
