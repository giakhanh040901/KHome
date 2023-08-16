using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    /// <summary>
    /// Dữ liệu trả về khi đặt lệnh thành công
    /// </summary>
    public class AppRstOrderDataSuccessDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Id Chi tiết mở bán
        /// </summary>
        public int OpenSellDetailId { get; set; }

        /// <summary>
        /// Thời gian hết hạn chuyển tiền lưu ngày giờ, đặt cả app lẫn cms đều có giờ này
        /// </summary>
        public DateTime? ExpTimeDeposit { get; set; }

        /// <summary>
        /// Mã căn
        /// </summary>
        public string ProductItemCode { get; set; }

        /// <summary>
        /// Tên dự án
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Ma hop dong
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Số tiền cọc
        /// </summary>
        public decimal DepositMoney { get; set; }

        /// <summary>
        /// Giá trị căn
        /// </summary>
        public decimal ProductItemPrice { get; set; }

        /// <summary>
        /// Mô tả thông tin thanh toán
        /// </summary>
        public string PaymentNote { get; set; }
        public List<AppTradingBankAccountDto> TradingBankAccounts { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }

        /// <summary>
        /// Hotline liên hệ của đại lý qua mở bán
        /// </summary>
        public string Hotline { get; set; }

        /// <summary>
        /// Thời gian giư
        /// </summary>
        public int? KeepTime { get; set; }
    }
}
