using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class AppRstOrderFilterDto
    {
        /// <summary>
        /// Trạng thái bộ lọc: 2 Đang giao dịch đặt cọc, 4: Chờ duyệt hợp đồng cọc, 5: Chờ duyệt hợp đồng mua BĐS (đã cọc)
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        [FromQuery(Name ="startDate")]
        public DateTime? StartDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set;}
    }
}
