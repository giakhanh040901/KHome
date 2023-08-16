using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class AppCountRstProductItemSignalRDto
    {
        public int ProjectId { get; set; }

        /// <summary>
        /// Nếu là đếm cho đại lý
        /// </summary>
        public int? OpenSellId { get; set; }

        /// <summary>
        /// Số lượng chưa mở bán
        /// </summary>
        public int? NotOpenCount { get; set; }

        /// <summary>
        /// Tổng đang mở bán
        /// </summary>
        public int? OpenCount { get; set; }

        /// <summary>
        /// Tổng đã cọc
        /// </summary>
        public int? DepositCount { get; set; }
        /// <summary>
        /// Tổng giữ chỗ
        /// </summary>
        public int? HoldCount { get; set; }

        /// <summary>
        /// Tổng đang khóa căn
        /// </summary>
        public int? LockCount { get; set; }

        /// <summary>
        /// Tổng đã bán
        /// </summary>
        public int? SoldCount { get; set; }
    }
}
