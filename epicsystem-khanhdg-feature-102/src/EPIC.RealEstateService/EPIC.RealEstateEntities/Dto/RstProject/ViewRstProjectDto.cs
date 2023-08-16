using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    /// <summary>
    /// Xem thông tin dự án kèm số lượng căn hộ
    /// </summary>
    public class ViewRstProjectDto : RstProjectDto
    {
        /// <summary>
        /// Tổng đã cọc
        /// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// Tổng đã phân phối
        /// </summary>
        public int DistributionQuantity { get; set; }

        /// <summary>
        /// Tổng đã bán
        /// </summary>
        public int SoldQuantity { get; set; }

        /// <summary>
        /// Tổng số lượng còn lại
        /// </summary>
        public int RemainingQuantity { get; set; }
    }
}
