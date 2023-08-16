using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyPointInvestor
{
    /// <summary>
    /// Lịch sử quy đổi điểm của nhà đầu tư/ Tab Danh sách ưu đãi
    /// </summary>
    public class PointInvestorConversionHistoryDto
    {
        public int ConversionPointDetailId { get; set; }
        public int VoucherId { get; set; }

        /// <summary>
        /// Tên voucher
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loại voucher (C: Cứng; DT: Điện tử)
        /// <see cref="LoyVoucherTypes"/>
        /// </summary>
        public string VoucherType { get; set; }

        /// <summary>
        /// Giá trị voucher
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Điểm quy đổi
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Ngày áp dụng
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>

        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Mã lô voucher
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Loại voucher
        /// </summary>
        public string UseType { get; set; }

        /// <summary>
        /// Trạng thái đổi điểm : 1. Khởi tạo, 2. Tiếp nhận Y/C, 3. Đang giao, 4.Hoàn thành, 5.Hủy yêu cầu
        /// </summary>
        public int? ConversionPointStatus { get; set; }

        /// <summary>
        /// Số lượng phát hành
        /// </summary>
        public int? PublishNum { get; set; }

        /// <summary>
        /// Số lượt đổi tối đa mỗi khách
        /// </summary>
        public int? ExchangeRoundNum { get; set; }

        /// <summary>
        /// Hiển thị trên app
        /// </summary>
        public string IsShowApp { get; set; }

        /// <summary>
        /// Đặt làm nổi bật
        /// </summary>
        public string IsHot { get; set; }

        /// <summary>
        /// Số lượng voucher
        /// </summary>
        public int ConversionPointDetailQuantity { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? ConversionPointFinishedDate { get; set; }

        /// <summary>
        /// Đơn vị của giá trị voucher (P: phần trăm; VND: VND)
        /// <see cref="LoyVoucherValueUnits"/>
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Voucher hết hạn hay chưa
        /// </summary>
        public bool IsExpired { get; set; }
    }
}
