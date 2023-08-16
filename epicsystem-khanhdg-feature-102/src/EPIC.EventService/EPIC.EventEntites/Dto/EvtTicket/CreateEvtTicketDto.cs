using EPIC.EventEntites.Dto.EvtTicketMedia;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtTicket
{
    public class CreateEvtTicketDto
    {
        /// <summary>
        /// Id khung giờ
        /// </summary>
        public int EventDetailId { get; set; }
        /// <summary>
        /// Tên loại vé
        /// </summary>
        private string _name;
        [Required(ErrorMessage = "Tên loại vé không được để trống")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }
        /// <summary>
        /// Free vé hay không
        /// </summary>
        public bool IsFree { get; set; }
        /// <summary>
        /// Giá vé
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Số lượng vé
        /// </summary>
        [Required(ErrorMessage = "Số lượng vé không được để trống")]
        public int Quantity { get; set; }
        /// <summary>
        /// Số lượng mua tối thiểu trong 1 lần
        /// </summary>
        public int MinBuy { get; set; }
        /// <summary>
        /// Số lần mua tối đa trong 1 lần
        /// </summary>
        public int MaxBuy { get; set; }
        /// <summary>
        /// Thời gian bán
        /// </summary>
        [Required(ErrorMessage = "Thời gian bắt đầu bán vé không được để trống")]
        public DateTime StartSellDate { get; set; }
        /// <summary>
        /// Thời gian kết thúc bán
        /// </summary>
        [Required(ErrorMessage = "Thời gian kết thúc bán vé không được để trống")]
        public DateTime EndSellDate { get; set; }
        /// <summary>
        /// Show app
        /// </summary>
        public string IsShowApp { get; set; }
        /// <summary>
        /// Mô tả ngắn
        /// </summary>
        [Required(ErrorMessage = "Mô tả vé không được để trống")]
        public string Description { get; set; }
        /// <summary>
        /// Loại mô tả Markdown/HTML
        /// </summary>
        [StringRange(AllowableValues = new string[] { ContentTypes.MARKDOWN, ContentTypes.HTML }, ErrorMessage = "Vui lòng chọn loại Type : MARKDOWN , HTML")]
        public string ContentType { get; set; }
        /// <summary>
        /// nội dung mô tả
        /// </summary>
        public string OverviewContent { get; set; }
        /// <summary>
        /// Danh sách ảnh
        /// </summary>
        public List<UpdateTicketImageDto> UrlImages { get; set; }
    }
}
