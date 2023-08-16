using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMediaDetail
{
    public class CreateEvtEventMediaDetailsDto
    {
        /// <summary>
        /// ID Event
        /// </summary>
        public int EventId { get; set; }

        
        private string _groupTitle;
        /// <summary>
        /// Tên nhóm hình ảnh
        /// </summary>
        [StringLength(256, ErrorMessage = "Tên nhóm không được dài hơn 256 ký tự")]
        public string GroupTitle
        {
            get => _groupTitle;
            set => _groupTitle = value?.Trim();
        }

        /// <summary>
        /// Chi tiết nhóm hình ảnh
        /// </summary>
        public IEnumerable<CreateEvtEventMediaDetailDto> EventMediaDetails { get; set; }
    }
}
