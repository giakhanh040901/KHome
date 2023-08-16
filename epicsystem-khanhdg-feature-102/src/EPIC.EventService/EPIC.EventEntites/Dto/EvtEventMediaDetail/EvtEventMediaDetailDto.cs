using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMediaDetail
{
    public class EvtEventMediaDetailDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id đại lý
        /// </summary>
        public int TradingProviderId { get; set; }
        /// <summary>
        /// Id nhóm hình ảnh
        /// </summary>
        public int EventMediaId { get; set; }
        /// <summary>
        /// Tên nhóm hình ảnh
        /// </summary>
        public string GroupTitle { get; set; }
        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string UrlImage { get; set; }
        /// <summary>
        /// Loại hình ảnh
        /// </summary>
        public string MediaType { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Vị trí ảnh
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public int SortOrder { get; set; }
        public string Deleted { get; set; }
    }
}
