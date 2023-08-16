using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.ConversionPoint
{
    public class FindAllLoyConversionPointDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Đại lý sơ cấp
        /// </summary>
        public int? TradingProviderId { get; set; }

        public int InvestorId { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Tổng điểm chuyển đổi
        /// </summary>
        public int TotalConversionPoint { get; set; }

        /// <summary>
        /// Điểm hiện tại của khách hàng tại thời điểm yêu cầu đổi điểm Lấy trong ConversionPoint
        /// </summary>
        public int? LoyCurrentPoint { get; set; }

        /// <summary>
        /// Loại yêu cầu: 1: Đổi Voucher, 2: Đổi điểm
        /// <see cref="LoyRequestTypes"/>
        /// </summary>
        public int RequestType { get; set; }

        /// <summary>
        /// Loại cấp phát: 1: Khách hàng đổi điểm (Mặc định khi tạo trên App), 2: Tặng khách hàng
        /// <see cref="LoyAllocationTypes"/>
        /// </summary>
        public int AllocationType { get; set; }

        /// <summary>
        /// Ngày yêu cầu
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Trạng thái: 1: Khởi tạo, 2. Tiếp nhận Y/C, 3. Đang giao, 4.Hoàn thành, 5.Hủy yêu cầu
        /// <see cref="LoyConversionPointStatus"/>
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Ngày tiếp nhận Yêu cầu
        /// </summary>
        public DateTime? PendingDate { get; set; }

        /// <summary>
        /// Ngày đang giao
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Ngày hoàn thành
        /// </summary>
        public DateTime? FinishedDate { get; set; }

        /// <summary>
        /// Ngày hủy
        /// </summary>
        public DateTime? CanceledDate { get; set; }

        /// <summary>
        /// Nguồn
        /// </summary>
        public int? Source { get; set; }
    }
}
