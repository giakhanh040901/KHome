using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class AppRstOrderDto
    {
        /// <summary>
        ///  Id của Order
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id của sản phẩm mở bán
        /// </summary>
        public int OpenSellDetailId { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// Tên dự án
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Mã căn/mã sản phẩm
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Số căn/tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kiểu phòng ngủ - số phòng (1: 1 phòng ngủ, 2: 2 phòng ngủ, 3: 3 phòng ngủ, 4: 4 phòng ngủ, 5: 5 phòng ngủ, 6: 6 phòng ngủ, 
        /// 7: 7 phòng ngủ, 8: 8 phòng ngủ, 9: 1 phòng ngủ + 1, 10: 2 phòng ngủ + 1, 11: 3 phòng ngủ + 1, 12: 4 phòng ngủ + 1)
        /// <see cref="RstRoomTypes"/>
        /// </summary>
        public int? RoomType { get; set; }

        /// <summary>
        /// Hướng cửa (1: Đông, 2: Tây, 3: Nam, 4: Bắc, 5: Đông Nam, 6: Đông Bắc, 7: Tây Nam, 8: Tây Bắc, 9: Đông Nam + Tây Bắc, 
        /// 10: Đông Nam + Đông Bắc, 11: Tây Nam + Tây Bắc, 12: Đông Nam + Tây Bắc, 13: Đông Bắc + Tây Bắc, 14: Đông Bắc + Tây Nam)
        /// <see cref="RstDirections"/>
        /// </summary>
        public int? DoorDirection { get; set; }

        /// <summary>
        /// Diện tích tính giá
        /// </summary>
        public decimal? PriceArea { get; set; }

        /// <summary>
        /// Giá đặt cọc
        /// </summary>
        public decimal DepositMoney { get; set; }

        /// <summary>
        /// Tổng số tiền thanh toán cọc
        /// </summary>
        public decimal PaymentMoney { get; set; }

        /// <summary>
        /// Đường dẫn ảnh đại diện
        /// </summary>
        public string UrlImage { get; set; }

        /// <summary>
        /// Trạng thái của hợp đồng
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Trạng thái của căn hộ
        /// </summary>
        public int ProductItemStatus { get; set; }

        /// <summary>
        /// Trạng thái của sản phẩm mở bán
        /// </summary>
        public int OpenSellDetailStatus { get; set; }

        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public DateTime? ExpTimeDeposit { get; set; }

        /// <summary>
        /// Thời gian đặt cọc
        /// </summary>
        public int? KeepTime { get; set; }

        /// <summary>
        /// Hợp đồng trạng thái Khởi tạo hoặc chờ thanh toán khi chưa thanh toán đủ tiền khóa căn hoặc hết thời gian cọc (nếu có)
        /// Nếu căn hộ chưa có người khác giao dịch thì được tiếp tục giao dịch tiếp (Y/N)
        /// </summary>
        public string CanContinueTrading { get; set; }
        /// <summary>
        /// Mô tả thông tin thanh toán
        /// </summary>
        public string PaymentNote { get; set; }

        /// <summary>
        /// Tài khoản nhận tiền thanh toán
        /// </summary>
        public List<AppTradingBankAccountDto> TradingBankAccounts { get; set; }
    }
}
