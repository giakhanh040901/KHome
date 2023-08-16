using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class AssetManagerDto
    {
        /// <summary>
        /// Tổng tài sản
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Lợi tức nhận được đến thời điểm hiện tại
        /// </summary>
        public decimal AllProfit { get; set; }

        /// <summary>
        /// Tài sản Đầu tư
        /// </summary>
        public decimal AssetBond { get; set; }

        /// <summary>
        /// Tài sản tích lũy
        /// </summary>
        public decimal AssetGarner { get; set; }

        /// <summary>
        /// Giao dịch bất động sản
        /// </summary>
        public decimal TradingInvest { get; set; }

        /// <summary>
        /// Tài sản giao dịch bất động sản (Tổng giá trị chuyển tiền thành công)
        /// </summary>
        public decimal AssetRealEstate { get; set; }

        /// <summary>
        /// Cho thuê bất động sản
        /// </summary>
        public decimal RentInvest { get; set; }
        /// <summary>
        /// Mua sắm
        /// </summary>
        public decimal Shopping { get; set; }
        public List<TradingRecentlyDto> TradingRecently { get; set; }
    }

    public class TradingRecentlyDto
    {
        /// <summary>
        /// Các loại hình sản phẩm chung (1: BOND, 2: INVEST, 3: COMPANY_SHARES, 4: GARNER,...
        /// </summary>
        public int? GeneralProductType { get; set; }
        /// <summary>
        ///  Tên chính sách 
        /// </summary>
        public string PolicyName { get; set; }
        /// <summary>
        /// Kiểu giao dịch 1: Thu, 2: Chi
        /// </summary>
        public int TranType { get; set; }

        /// <summary>
        /// Loại giao dịch: 1: Thanh toán hợp đồng, 2: Chi trả lợi nhuận, 3: Rút vốn
        /// </summary>
        public int TranClassify { get; set; }

        /// <summary>
        /// Mã giao dịch
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// Số tiền
        /// </summary>
        public decimal PaymentAmnount { get; set; }
        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime? TranDate { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
    }
}
