using EPIC.Utils.ConstantVariables.Loyalty;
using System;

namespace EPIC.LoyaltyEntities.Dto.ConversionPoint
{
    /// <summary>
    /// Ưu đãi của nhà đầu tư
    /// </summary>
    public class AppLoyConversionPointByInvestorDto
    {
        /// <summary>
        /// Id của chi tiết quy đổi
        /// </summary>
        public int ConversionPointDetailId { get; set; }

        /// <summary>
        /// Trạng thái: 1: Khởi tạo, 2. Tiếp nhận Y/C, 3. Đang giao, 4.Hoàn thành, 5.Hủy yêu cầu
        /// <see cref="LoyConversionPointStatus"/>
        /// </summary>
        public int ConversionPointStatus { get; set; }
        /// <summary>
        /// Id voucher
        /// </summary>
        public int VoucherId { get; set; }
        /// <summary>
        /// Tên voucher
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên hiển thị trên ứng dụng
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Ảnh voucher
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Loại voucher (C: Cứng; DT: Điện tử)
        /// <see cref="LoyVoucherTypes"/>
        /// </summary>
        public string VoucherType { get; set; }
        /// <summary>
        /// Link voucher
        /// </summary>
        public string LinkVoucher { get; set; }

        /// <summary>
        /// Điểm quy đổi
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Ngày áp dụng
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? ExpiredDate { get; set; }
    }
}
