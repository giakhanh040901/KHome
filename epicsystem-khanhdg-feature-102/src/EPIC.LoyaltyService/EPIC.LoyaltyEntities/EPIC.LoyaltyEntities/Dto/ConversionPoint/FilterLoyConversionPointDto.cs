using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.ConversionPoint
{
    public class FilterLoyConversionPointDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Lọc theo trạng thái: 1: Khởi tạo, 2. Tiếp nhận Y/C, 3. Đang giao, 4.Hoàn thành, 5.Hủy yêu cầu
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// Lọc theo ngày
        /// </summary>
        [FromQuery(Name = "date")]
        public DateTime? Date { get; set; }
    }
}
