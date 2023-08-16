using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class AppRstOrderDetailDto : AppRstProductItemDetailDto
    {
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Số tiền cọc phải nộp
        /// </summary>
        public decimal DepositMoney { get; set; }

        /// <summary>
        /// Tổng số tiền đã thanh toán cọc
        /// </summary>
        public decimal PaymentMoney { get; set; }

        /// <summary>
        /// Tổng số còn phải thanh toán cọc
        /// </summary>
        public decimal PayableAmount { get; set; }

        /// <summary>
        /// Hình thức thanh toán 1: Trả thẳng, 2: Trả góp ngân hàng
        /// </summary>
        public int? PaymentType { get; set; }

        /// <summary>
        /// Trạng thái lệnh (1: Khởi tạo, 2: Chờ thanh toán cọc, 3: Chờ ký hợp đồng, 4: Chờ duyệt hợp đồng, 5: Đã cọc)<br/>
        /// <see cref="RstOrderStatus"/>
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// Trạng thái của mở bán
        /// </summary>
        public int OpenSellDetailStatus { get; set; }

        /// <summary>
        /// Thời gian hết hạn chuyển tiền lưu ngày giờ, đặt cả app lẫn cms đều có giờ này
        /// </summary>
        public DateTime? ExpTimeDeposit { get; set; }

        /// <summary>
        /// Mô tả thông tin thanh toán
        /// </summary>
        public string PaymentNote { get; set; }

        /// <summary>
        /// Tab dòng tiền
        /// </summary>
        public List<AppRstOrderCashFlowDto> OrderCashFlows { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng nhận tiền thanh toán
        /// </summary>
        public List<AppTradingBankAccountDto> TradingBankAccounts { get; set; }

        /// <summary>
        /// Đồng sở hữu
        /// </summary>
        public List<AppOrderCoOwnersDto> OrderCoOwners { get; set; }

        /// <summary>
        /// Tab Giao dịch
        /// </summary>
        public List<AppPaymentTransactionDto> PaymentTransactions { get; set; }
    }
}
