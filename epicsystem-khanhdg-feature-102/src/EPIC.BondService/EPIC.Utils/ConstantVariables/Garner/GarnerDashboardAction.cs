using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Garner
{
    /// <summary>
    /// Các loại hành động ở cục bên phải của dashboard
    /// </summary>
    public static class GarnerDashboardAction
    {
        /// <summary>
        /// Đặt lệnh đầu tư mới
        /// </summary>
        public const int DAT_LENH_DAU_TU_MOI = 1; 
        /// <summary>
        /// Tạo yêu cầu rút tiền
        /// </summary>
        public const int TAO_YEU_CAU_RUT_TIEN = 2; 
        /// <summary>
        /// Tạo yêu cầu nhận hợp đồng
        /// </summary>
        public const int TAO_YEU_CAU_NHAN_HOP_DONG = 3;
    }
}
