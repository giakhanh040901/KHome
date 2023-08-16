using EPIC.EventEntites.Dto.EvtEventMediaDetail;
using EPIC.Utils.ConstantVariables.Media;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMedia
{
    public class AppEventMediaDto
    {
        /// <summary>
        /// id event media
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// id event
        /// </summary>
        public int EventId { get; set; }
        public string GroupTitle { get; set; }
        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        //[Required]
        public string UrlImage { get; set; }

        /// <summary>
        /// Đường dẫn điều hướng khi click ảnh
        /// </summary>
        public string UrlPath { get; set; }

        /// <summary>
        /// IMAGE: ảnh, VIDEO: video
        /// <see cref="MediaTypes"/>
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// Vị trí
        /// </summary>
        public string Location { get; set; }
        public IEnumerable<AppEventMediaDetailDto> EvtEventMediaDetails { get; set; }
    }
    public class AppEventMediaDetailDto
    {
        /// <summary>
        /// id event media detail
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ảnh
        /// </summary>
        public string UrlImage { get; set; }
        /// <summary>
        /// sắp xếp
        /// </summary>
        public int SortOrder { get; set; }
        /// <summary>
        /// IMAGE: ảnh, VIDEO: video
        /// <see cref="MediaTypes"/>
        /// </summary>
        public string MediaType { get; set; }
    }

}
