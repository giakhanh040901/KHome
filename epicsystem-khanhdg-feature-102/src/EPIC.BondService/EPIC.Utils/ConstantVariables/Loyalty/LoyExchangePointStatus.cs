using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Loyalty
{
    /// <summary>
    /// Trạng thái yêu cầu đổi điểm
    /// </summary>
    public static class LoyExchangePointStatus
    {
        /// <summary>
        /// Tiếp nhận
        /// </summary>
        public const int PENDING = 1;
        /// <summary>
        /// Đang giao
        /// </summary>
        public const int DELIVERY = 2;
        /// <summary>
        /// Đã nhận => Bỏ
        /// </summary>
        //public const int RECEIVED = 3;
        /// <summary>
        /// Hoàn thành
        /// </summary>
        public const int FINISHED = 4;
        /// <summary>
        /// Hủy yêu cầu
        /// </summary>
        public const int CANCELED = 5;
        /// <summary>
        /// Khởi tạo
        /// </summary>
        public const int CREATED = 6;
    }
}
