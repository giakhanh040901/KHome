using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    public class AppRelatedEventsDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        public string UrlImage { get; set; }
        /// <summary>
        /// Mã Thành phố
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// Tên Thành phố
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// Giá vé
        /// </summary>
        public decimal? MinPrice { get; set; }
        /// <summary>
        /// Giá vé
        /// </summary>
        public decimal? MaxPrice { get; set; }
        /// <summary>
        /// Vé miễn phí hay không
        /// </summary>
        public bool IsFree { get; set; }

    }
}
