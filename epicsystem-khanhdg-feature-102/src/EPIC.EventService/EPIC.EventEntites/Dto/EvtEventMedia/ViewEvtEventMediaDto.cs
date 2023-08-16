using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMedia
{
    public class ViewEvtEventMediaDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID đại lý
        /// </summary>
        public int TradingProviderId { get; set; }
        /// <summary>
        /// ID sự kiện
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// Tên nhóm hình ảnh
        /// </summary>
        public string GroupTitle { get; set; }
        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string UrlImage { get; set; }
        /// <summary>
        /// Đường dẫn điều hướng (Khi click vào ảnh)
        /// </summary>
        public string UrlPath { get; set; }
        /// <summary>
        /// Loại hình ảnh (Image/Video)
        /// </summary>
        public string MediaType { get; set; }
        /// <summary>
        /// Vị trí
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Thứ tự
        /// </summary>
        public int SortOrder { get; set; }
        public string Deleted { get; set; }
    }
}
