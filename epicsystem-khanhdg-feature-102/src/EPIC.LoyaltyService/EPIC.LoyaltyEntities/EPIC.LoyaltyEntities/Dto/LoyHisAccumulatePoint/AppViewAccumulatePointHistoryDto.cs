using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class AppViewAccumulatePointHistoryDto
    {
        /// <summary>
        /// Id tích/tiêu điểm
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Số điểm
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Tích điểm hay tiêu điểm
        /// 1: Tích điểm, 2: Tiêu điểm<br/>
        /// <see cref="LoyPointTypes"/>
        /// </summary>
        public int PointType { get; set; }

        /// <summary>
        /// Lý do key
        /// </summary>
        public int? Reason { get; set; }

        /// <summary>
        /// Lý do value
        /// </summary>
        public string ReasonName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? ExchangedPointStatus { get; set; }
    }
}
