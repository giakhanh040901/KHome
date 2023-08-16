using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using EPIC.Utils.ConstantVariables.Loyalty;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class FindRequestConsumePointDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// Lọc theo ngày của trạng thái đã chọn
        /// </summary>
        [FromQuery(Name = "date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// <see cref="LoyFindRequestExchangePointStatusDates"/>
        /// 1: Bằng ngày; 2: Tất cả ngày 
        /// </summary>
        [FromQuery(Name = "statusDate")]
        public int? StatusDate { get; set; }
    }
}
