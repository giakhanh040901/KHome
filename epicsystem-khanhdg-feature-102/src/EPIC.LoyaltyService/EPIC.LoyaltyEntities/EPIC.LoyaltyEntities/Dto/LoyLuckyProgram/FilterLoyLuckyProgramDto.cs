using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgram
{
    public class FilterLoyLuckyProgramDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Trạng thái: 1 Khởi tạo, 2 Tạm dừng
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// Đã hết hạn hay chưa
        /// </summary>
        [FromQuery(Name = "isExpried")]
        public bool? IsExpried { get; set; }
    }
}
