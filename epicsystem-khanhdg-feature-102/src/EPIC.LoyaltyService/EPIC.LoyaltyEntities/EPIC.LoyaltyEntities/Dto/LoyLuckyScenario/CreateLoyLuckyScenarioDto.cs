﻿using EPIC.LoyaltyEntities.Dto.LoyLuckyRotationInterface;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenarioDetail;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyScenario
{
    public class CreateLoyLuckyScenarioDto
    {
        public int LuckyProgramId { get; set; }

        private string _name;
        public string Name 
        { 
            get => _name; 
            set => _name = value?.Trim(); 
        }

        /// <summary>
        /// Loại kịch bản
        /// </summary>
        public int LuckyScenarioType { get; set; }

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
        public List<CreateLoyLuckyScenarioDetailDto> LuckyScenarioDetails { get; set; }

        /// <summary>
        /// Giao diện vòng quay may mắn
        /// </summary>
        public CreateLoyLuckyRotationInterfaceDto LuckyRotationInterface { get; set; }
    }
}
