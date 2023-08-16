using EPIC.CoreSharedEntities.Dto.TradingProvider;
using System;
using System.Collections.Generic;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class AppGarnerOrderDetailDto
    {
        public long Id { get; set; }

        /// <summary>
        /// Id chính sách
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Số tiền rút tối thiểu
        /// </summary>
        public decimal MinWithdrawal { get; set; }

        /// <summary>
        /// Số tiền rút tối đa
        /// </summary>
        public decimal? MaxWithdrawal { get; set; }

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
        /// Gía trị đầu tư
        /// </summary>
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Trạng thái của bất động sản
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Chọn kỳ hạn Lợi tức
        /// </summary>
        public decimal? Profit { get; set; }

        /// <summary>
        /// Chọn kỳ hạn % Lợi tức
        /// </summary>
        public decimal? PercentProfit { get; set; }

        /// <summary>
        /// Chọn kỳ hạn Thu nhập cuối kỳ
        /// </summary>
        public decimal? ActuallyProfit { get; set; }

        /// <summary>
        /// Tên ngân hàng khách cá nhân
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng khách cá nhân
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// Chủ tài khoản khách cá nhân
        /// </summary>
        public string OwnerAccount { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string ContractAddress { get; set; }

        /// <summary>
        /// Mã giới thiệu nhân viên tư vấn
        /// </summary>
        public string SaleReferralCode { get; set; }

        /// <summary>
        /// Tên tư vấn viên
        /// </summary>
        public string SaleName { get; set; }

        /// <summary>
        /// Mô tả thông tin thanh toán
        /// </summary>
        public string PaymentNote { get; set; }

        /// <summary>
        /// Tên kiểu trả lợi tức
        /// </summary>
        public string InterestTypeName { get; set; }
        /// <summary>
        /// Lợi tức dự kiến
        /// </summary>
        public decimal ExpectedProfit { get; set; }

        public List<AppTradingBankAccountDto> TradingBankAccounts { get; set; }
    }
}
