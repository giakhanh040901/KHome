using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BondShared
{
    /// <summary>
    /// Số lượng đơn giá trái phiếu khi đặt lệnh
    /// </summary>
    public class QuantityAndUnitPriceDto
    {
        /// <summary>
        /// Số lượng trái phiếu đã làm tròn
        /// </summary>
        public long Quantity { get; set; }
        /// <summary>
        /// Đơn giá chia ngược lại
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
