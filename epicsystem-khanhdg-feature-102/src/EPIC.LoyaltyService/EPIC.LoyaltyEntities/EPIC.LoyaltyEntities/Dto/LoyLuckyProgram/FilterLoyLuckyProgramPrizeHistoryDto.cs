using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgram
{
    public class FilterLoyLuckyProgramPrizeHistoryDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Hạng thành viên
        /// </summary>
        [FromQuery(Name = "rankId")]
        public int? RankId { get; set; }

        /// <summary>
        /// Tên chương trình
        /// </summary>
        [FromQuery(Name = "luckyProgramId")]
        public int? LuckyProgramId { get; set; }

        /// <summary>
        /// Thời gian trúng thưởng
        /// </summary>
        [FromQuery(Name = "createdDate")]
        public DateTime? CreatedDate { get; set; }
    }
}
