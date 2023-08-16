using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    /// <summary>
    /// Giá trị lock căn và giá trị cọc của căn hộ
    /// </summary>
    public class RstProductItemPriceDto
    {
        /// <summary>
        /// Giá trị Lock căn
        /// </summary>
        public decimal LockPrice { get; set; }

        /// <summary>
        /// Giá trị cọc
        /// </summary>
        public decimal DepositPrice { get; set; }

        /// <summary>
        /// Chính sách phân phối được lấy để tính giá cọc, giá lock
        /// </summary>
        public int DistributionPolicyId { get; set; }
    }
}
