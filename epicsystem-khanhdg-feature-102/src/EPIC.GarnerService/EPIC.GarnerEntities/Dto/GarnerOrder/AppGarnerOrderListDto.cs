using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class AppGarnerOrderListDto
    {
        /// <summary>
        /// Id hợp đồng
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Ngày thanh toán đủ
        /// </summary>
        public DateTime? PaymentFullDate { get; set; }

        /// <summary>
        /// Ngày active hợp đồng
        /// </summary>
        public DateTime? ActiveDate { get; set; }

        /// <summary>
        /// Ngày bắt đầu tính tiền đầu hợp đồng
        /// </summary>
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Ngày đặt lệnh
        /// </summary>
        public DateTime? BuyDate { get; set; }

        /// <summary>
        /// Ngày tất toán
        /// </summary>
        public DateTime? SettlementDate { get; set; }

        /// <summary>
        /// Mã giới thiệu của tư vấn viên/ SaleReferralCodeSub thì ghi vào
        /// </summary>
        public string SaleReferralCode { get; set; }

        /// <summary>
        /// Tên của tư vấn viên
        /// </summary>
        public string SaleName { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư của hợp đồng
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư ban đầu của hợp đồng
        /// </summary>
        public decimal InitTotalValue { get; set; }

        /// <summary>
        /// Lợi nhuận thực nhận (Trạng thái active mới có: Lợi nhuận tính đến thời điểm hiện tại)
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// Trạng thái của hợp đồng
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Trạng thái khác của hợp đồng không theo trạng thái của bảng
        /// 1: Chờ duyệt rút vốn, 2: Chờ chuyển đổi, 3: Rút vốn thành công, 4: chuyển đổi thành công, 5: Hủy rút vốn
        /// </summary>
        public int? OtherStatus { get; set; }

        /// <summary>
        /// Id của yêu cầu rút vốn
        /// </summary>
        public long? WithdrawalId { get; set; }

        /// <summary>
        /// Số tiền yêu cầu rút vốn
        /// </summary>
        public decimal? WithdrawalMoney { get; set; }

        /// <summary>
        /// Số tiền yêu cầu rút vốn của từng lệnh
        /// </summary>
        public decimal? WithdrawalOrderMoney { get; set; }

        /// <summary>
        /// (Màn sổ lệnh) Ngày tạo hợp đồng/ Ngày tạo rút vốn 
        /// (Màn lịch sử) Ngày tất toán/ Ngày duyệt rút vốn
        /// </summary>
        public DateTime? CreatedDate { get; set; }
    }
}
