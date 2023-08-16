using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Shared
{
    /// <summary>
    /// Thông tin sinh mã hợp đồng thừ ConfigContractCode
    /// </summary>
    public class OrderContractCodeDto
    {
        public int ConfigContractCodeId { get; set; }

        /// <summary>
        /// Id order
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Loại sản phẩm : Ví dụ Đầu tư cổ phần: dautucophan
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string PolicyCode { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Tên các chữ cái đầu của nhà đầu tư
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? Now { get; set; }

        /// <summary>
        /// Ngày thanh toán đủ
        /// </summary>
        public DateTime? PaymentFullDate { get; set; }

        /// <summary>
        /// Ngày đặt hợp đồng
        /// </summary>
        public DateTime? BuyDate { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }
        public string ShortNameBusiness { get; set; }
        /// <summary>
        /// Tên viết tắt dự án
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// RST mã căn hộ
        /// </summary>
        public string RstProductItemCode { get; set; }

        /// <summary>
        /// Mã sự kiện
        /// </summary>
        public string EventCode { get; set; }
    }
}
