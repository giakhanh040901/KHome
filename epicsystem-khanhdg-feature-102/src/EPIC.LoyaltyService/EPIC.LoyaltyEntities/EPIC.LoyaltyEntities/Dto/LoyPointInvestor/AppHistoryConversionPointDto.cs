using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyPointInvestor
{
    /// <summary>
    /// Lịch sử điểm thưởng
    /// </summary>
    public class AppHistoryConversionPointDto
    {
        /// <summary>
        ///Lịch sử điểm thưởng: 1 Tích điểm, 2: Đổi voucher
        /// <see cref="LoyConversionTypes"/>
        /// </summary>
        public int ConversionType { get; set; }

        /// <summary>
        /// Tên lịch sử/ tên voucher
        /// </summary>
        public string HistoryName { get; set; }

        /// <summary>
        /// Điểm
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Thời gian
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Trạng thái khi tích điểm ConversionType = 1: 4 Hoàn thành, 5 Hủy duyệt
        /// Trạng thái khi đổi voucher: 1: Khởi tạo, 2. Tiếp nhận Y/C, 3. Đang giao, 4.Hoàn thành, 5.Hủy yêu cầu
        /// </summary>
        public int? Status { get; set; }
    }
}
