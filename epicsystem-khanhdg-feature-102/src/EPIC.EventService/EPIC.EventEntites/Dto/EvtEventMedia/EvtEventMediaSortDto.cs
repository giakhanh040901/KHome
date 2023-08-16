using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMedia
{
    public class EvtEventMediaSortDto
    {
        /// <summary>
        /// Sự kiện
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// Vị trí hình ảnh
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Thứ tự
        /// </summary>
        public IEnumerable<EvtEventMediaSortOrderDto> Sort { get; set; }
    }
}
