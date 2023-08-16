using EPIC.Utils;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    /// <summary>
    /// Hiển thị dòng tiền thanh toán của hợp đồng
    /// </summary>
    public class AppRstOrderCashFlowDto
    {
        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime? TranDate { get; set; }

        /// <summary>
        /// Số giao dịch
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// Số tiền
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Trạng thái thanh toán (1: Khởi tạo, 2: Đã thanh toán (phe duyet), 3: Huỷ thanh toán (huy duyet))
        /// <see cref="OrderPaymentStatus"/>
        /// </summary>
        public int Status { get; set; }
    }
}
