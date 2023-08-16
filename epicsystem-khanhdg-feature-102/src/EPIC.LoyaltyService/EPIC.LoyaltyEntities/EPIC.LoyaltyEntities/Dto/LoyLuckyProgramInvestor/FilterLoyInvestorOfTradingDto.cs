using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor
{
    public class FilterLoyInvestorOfTradingDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Id chương trình
        /// </summary>
        [FromQuery(Name = "luckyProgramId")]
        public int? LuckyProgramId { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        [FromQuery(Name = "sex")]
        public string Sex { get; set; }

        /// <summary>
        /// Hạng thành viên
        /// </summary>
        [FromQuery(Name = "rankId")]
        public int? RankId { get; set; }

        /// <summary>
        /// Loại khách hàng: 1 Sale, 2 Khách hàng đã đầu tư, 3 Khách chưa đầu tư
        /// </summary>
        [FromQuery(Name = "customerType")]
        public int? CustomerType { get; set; }

        [FromQuery(Name = "isSelected")]
        public bool? IsSelected { get; set; }
    }
}
