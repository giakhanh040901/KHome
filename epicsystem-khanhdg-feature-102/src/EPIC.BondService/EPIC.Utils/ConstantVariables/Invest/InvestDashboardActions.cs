using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Invest
{
    /// <summary>
    /// Các loại hành động ở hoạt động gần đây của dashboard
    /// </summary>
    public static class InvestDashboardActions
    {
        /// <summary>
        /// Đặt lệnh đầu tư mới
        /// </summary>
        public const int DAT_LENH_DAU_TU_MOI = 1;

        /// <summary>
        /// Tạo yêu cầu tái tục
        /// </summary>
        public const int TAO_YEU_CAU_TAI_TUC = 2;

        /// <summary>
        /// Tạo yêu cầu rút tiền
        /// </summary>
        public const int TAO_YEU_CAU_RUT_TIEN = 3;

        /// <summary>
        /// Tạo yêu cầu nhận hợp đồng
        /// </summary>
        public const int TAO_YEU_CAU_NHAN_HOP_DONG = 4;
    }
}
