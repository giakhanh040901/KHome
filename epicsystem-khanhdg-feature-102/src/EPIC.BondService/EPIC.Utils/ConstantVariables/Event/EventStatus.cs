using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Event
{
    /// <summary>
    /// Trạng thái sự kiện
    /// </summary>
    public static class EventStatus
    {
        public const int KHOI_TAO = 1;
        public const int DANG_MO_BAN = 2;
        public const int TAM_DUNG = 3;
        public const int HUY_SU_KIEN = 4;
        public const int KET_THUC = 5;
    }

    /// <summary>
    /// Trạng thái khung giờ
    /// </summary>
    public static class EventDetailStatus
    {
        /// <summary>
        /// Bản ghi nháp
        /// </summary>
        public const int NHAP = 0;
        public const int KICH_HOAT = 1;
        public const int HUY_KICH_HOAT = 2;
        public const int HET_VE = 3;
    }
}
