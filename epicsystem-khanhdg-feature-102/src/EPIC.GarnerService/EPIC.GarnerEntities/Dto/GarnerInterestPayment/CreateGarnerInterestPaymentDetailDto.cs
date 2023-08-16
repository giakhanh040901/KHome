using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerInterestPayment
{
    /// <summary>
    /// Thêm chi trả chi tiết
    /// </summary>
    public class CreateGarnerInterestPaymentDetailDto
    {
        /// <summary>
        /// Id hợp đồng
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Số tiền chi
        /// </summary>
        public decimal AmountReceived { get; set; }

        /// <summary>
        /// Thuế
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Phần trăm lợi nhuận
        /// </summary>
        public decimal ProfitRate { get; set; }

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public decimal Profit { get; set; }
    }
}
