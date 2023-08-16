using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgram
{
    public class FilterLoyLuckyProgramHistoryDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Trạng thái trúng thưởng
        /// </summary>
        [FromQuery(Name = "isPrize")]
        public bool? IsPrize { get; set; }

        /// <summary>
        /// Tên chương trình
        /// </summary>
        [FromQuery(Name = "luckyProgramId")]
        public int? LuckyProgramId { get; set; }

        /// <summary>
        /// Thời gian tham gia
        /// </summary>
        [FromQuery(Name = "createdDate")]
        public DateTime? CreatedDate { get; set; }
    }
}
