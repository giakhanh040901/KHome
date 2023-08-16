using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMediaDetail
{
    public class AddEvtEventMediaDetailsDto
    {
        /// <summary>
        /// ID hình ảnh sự kiên
        /// </summary>
        public int EventMediaId { get; set; }
        /// <summary>
        /// Danh sách chi tiết hình ảnh sự kiện
        /// </summary>
        public List<CreateEvtEventMediaDetailDto> EventMediaDetails { get; set; }
    }
}
