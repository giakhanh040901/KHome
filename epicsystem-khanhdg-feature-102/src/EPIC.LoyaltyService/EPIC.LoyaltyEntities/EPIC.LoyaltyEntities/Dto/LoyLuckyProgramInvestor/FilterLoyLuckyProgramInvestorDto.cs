using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor
{
    public class FilterLoyLuckyProgramInvestorDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "luckyProgramId")]
        public int LuckyProgramId { get; set; }

        /// <summary>
        /// Bộ lọc đã tham gia hay chưa
        /// </summary>
        [FromQuery(Name = "isJoin")]
        public bool? IsJoin { get; set; }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
