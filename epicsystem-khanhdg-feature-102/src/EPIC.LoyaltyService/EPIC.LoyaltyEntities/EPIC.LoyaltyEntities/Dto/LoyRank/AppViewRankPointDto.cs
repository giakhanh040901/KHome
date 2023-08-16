using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyRank
{
    public class AppViewRankPointDto
    {
        /// <summary>
        /// Điểm hiện tại
        /// </summary>
        public int? LoyCurrentPoint { get; set; }
        /// <summary>
        /// Tổng điểm
        /// </summary>
        public int? LoyTotalPoint { get; set; }
        /// <summary>
        /// Điểm tạm tính
        /// </summary>
        public int? LoyTempPoint { get; set; }
        /// <summary>
        /// Id hạng
        /// </summary>
        public int? RankId { get; set; }
        /// <summary>
        /// Tên hạng
        /// </summary>
        public string RankName { get; set; }
        /// <summary>
        /// Điểm bắt đầu hạng
        /// </summary>
        public int? PointStart { get; set; }
        /// <summary>
        /// Điểm kết thúc hạng
        /// </summary>
        public int? PointEnd { get; set; }

        /// <summary>
        /// Điểm bắt đầu của hạng tiếp theo
        /// </summary>
        public int? NextPointRank { get; set; }
    }
}
