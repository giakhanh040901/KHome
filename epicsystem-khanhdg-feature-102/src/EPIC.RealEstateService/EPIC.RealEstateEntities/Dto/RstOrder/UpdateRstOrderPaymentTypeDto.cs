using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    /// <summary>
    /// Cập nhật hình thức thanh toán
    /// </summary>
    public class UpdateRstOrderPaymentTypeDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Hình thức thanh toán mua nhà: 1: Trả thẳng, 2: Trả góp ngân hàng
        /// </summary>
        public int? PaymentType { get; set; }
    }
}
