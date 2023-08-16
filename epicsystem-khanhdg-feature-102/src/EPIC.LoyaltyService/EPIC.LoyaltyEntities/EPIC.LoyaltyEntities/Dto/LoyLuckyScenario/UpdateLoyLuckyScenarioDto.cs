using DocumentFormat.OpenXml.Office2010.ExcelAc;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenarioDetail;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyScenario
{
    public class UpdateLoyLuckyScenarioDto
    {
        public int Id { get; set; }
        public int LuckyProgramId { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Số lượng giải thưởng
        /// </summary>
        public int? PrizeQuantity { get; set; }

        /// <summary>
        /// Ảnh đại diện kịch bản
        /// </summary>
        public IFormFile AvatarImageUrl { get; set; }

        /// <summary>
        /// Chi tiết kịch bản vòng quay
        /// </summary>
        public List<UpdateLoyLuckyScenarioDetailDto> LuckyScenarioDetails { get; set; }
    }
}
