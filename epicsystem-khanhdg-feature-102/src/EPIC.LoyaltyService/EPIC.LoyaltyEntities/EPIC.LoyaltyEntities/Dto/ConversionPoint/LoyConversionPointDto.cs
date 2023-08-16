using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils;

namespace EPIC.LoyaltyEntities.Dto.ConversionPoint
{
    public class LoyConversionPointDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Đại lý sơ cấp
        /// </summary>
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Id nhà đầu tư
        /// </summary>
        public int? InvestorId { get; set; }

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
        /// Có trừ điểm không? Mặc định là có
        /// <see cref="YesNo"/>
        /// </summary>
        public string IsMinusPoint { get; set; }

        /// <summary>
        /// Ngày yêu cầu
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Trạng thái đổi điểm : 1. Khởi tạo, 2. Tiếp nhận Y/C, 3. Đang giao, 4.Hoàn thành, 5.Hủy yêu cầu
        /// <see cref="LoyConversionPointStatus"/>
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Điểm hiện tại của khách hàng tại thời điểm yêu cầu đổi điểm
        /// </summary>
        public int? CurrentPoint { get; set; }

        /// <summary>
        /// Nguồn
        /// </summary>
        public int? Source { get; set; }

        /// <summary>
        /// Chi tiết chuyển đổi
        /// </summary>
        public List<LoyConversionPointDetailDto> Details { get; set; }

        /// <summary>
        /// Chi tiết tiến trình
        /// </summary>
        public List<LoyConversionPointStatusLogDto> StatusLogs { get; set; }
    }
}
