using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Loyalty
{
    /// <summary>
    /// Trạng thái chuyển đổi điểm
    /// </summary>
    public static class LoyConversionPointStatus
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        public const int CREATED = 1;
        // <summary>
        /// Tiếp nhận
        /// </summary>
        public const int PENDING = 2;
        /// <summary>
        /// Đang giao
        /// </summary>
        public const int DELIVERY = 3;
        /// <summary>
        /// Hoàn thành
        /// </summary>
        public const int FINISHED = 4;
        /// <summary>
        /// Hủy yêu cầu
        /// </summary>
        public const int CANCELED = 5;

        public static string ConversionPointStatusName(int? status)
        {
            return status switch
            {
                CREATED => "Khởi tạo",
                PENDING => "Tiếp nhận yêu cầu",
                DELIVERY => "Đang giao",
                FINISHED => "Hoàn thành",
                CANCELED => "Hủy yêu cầu",
                _ => "",
            };
        }
    }
}
