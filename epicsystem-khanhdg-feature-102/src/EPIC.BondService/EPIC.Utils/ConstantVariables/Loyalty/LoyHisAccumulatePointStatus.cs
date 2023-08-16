using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Loyalty
{
    /// <summary>
    /// Trạng thái lệnh tích điểm/tiêu điểm
    /// </summary>
    public static class LoyHisAccumulatePointStatus
    {
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
