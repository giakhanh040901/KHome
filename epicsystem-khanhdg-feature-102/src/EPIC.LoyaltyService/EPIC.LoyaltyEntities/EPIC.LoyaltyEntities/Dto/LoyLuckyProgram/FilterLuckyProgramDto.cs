using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgram
{
    public class FilterLuckyProgramDto : PagingRequestBaseDto
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
        /// Trạng thái: 1 Khởi tạo
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
