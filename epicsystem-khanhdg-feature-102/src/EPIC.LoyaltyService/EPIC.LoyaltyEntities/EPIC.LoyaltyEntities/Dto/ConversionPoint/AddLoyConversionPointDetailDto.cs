using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.ConversionPoint
{
    /// <summary>
    /// Chi tiết yêu cầu chuyển đổi
    /// </summary>
    public class AddLoyConversionPointDetailDto
    {
        /// <summary>
        /// Id voucher
        /// </summary>
        public int VoucherId { get; set; }

        /// <summary>
        /// Số lượng yêu cầu
        /// </summary>
        public int Quantity { get; set; }
    }
}
