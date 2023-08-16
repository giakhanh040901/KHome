using EPIC.LoyaltyEntities.Dto.ConversionPoint;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyPointInvestor
{
    /// <summary>
    /// Xem chi tiết Tab Danh sách ưu đãi
    /// </summary>
    public class PointInvestorConversionHistoryInfoDto
    {
        public int InvestorId { get; set; }

        public int ConversionPointId { get; set; }
        /// <summary>
        /// Sdt của khách
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email của khách
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Tên của khách
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Tên hạng
        /// </summary>
        public string RankName { get; set; }

        /// <summary>
        /// Điểm hiện tại/ Lấy trong ConversionPoint
        /// </summary>
        public int? LoyCurrentPoint { get; set; }

        /// <summary>
        /// Tổng điểm
        /// </summary>
        public int? LoyTotalPoint { get; set; }

        /// <summary>
        /// Điểm đổi Voucherx
        /// </summary>
        public int? VoucherPoint { get; set; }

        /// <summary>
        /// Tên voucher
        /// </summary>
        public string VoucherName { get; set; }

        /// <summary>
        /// Loại voucher (C: Cứng; DT: Điện tử)
        /// <see cref="LoyVoucherTypes"/>
        /// </summary>
        public string VoucherType { get; set; }

        /// <summary>
        /// Giá trị voucher
        /// </summary>
        public decimal? VoucherValue { get; set; }

        /// <summary>
        /// Ảnh Banner voucher
        /// </summary>
        public string BannerImageUrl { get; set; }

        /// <summary>
        /// Đơn vị của giá trị voucher (P: phần trăm; VND: VND)
        /// <see cref="LoyVoucherValueUnits"/>
        /// </summary>
        public string VoucherUnit { get; set; }

        /// <summary>
        /// Chi tiet tien trinh
        /// </summary>
        public List<LoyConversionPointStatusLogDto> ConversionPointStatusLogs { get; set; }
    }
}
