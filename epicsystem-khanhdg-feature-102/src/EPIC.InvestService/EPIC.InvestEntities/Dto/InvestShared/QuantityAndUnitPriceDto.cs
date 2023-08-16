using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestShared
{
    public class QuantityAndUnitPriceDto
    {
        /// <summary>
        /// Số lượng đã làm tròn
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Đơn giá chia ngược lại
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
