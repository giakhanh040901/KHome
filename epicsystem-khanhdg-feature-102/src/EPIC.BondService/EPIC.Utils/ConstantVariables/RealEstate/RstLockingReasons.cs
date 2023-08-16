using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public static class RstLockingReasons
    {
        /// <summary>
        /// Đã bán offline
        /// </summary>
        public const int DA_BAN_OFFLINE = 1;
        /// <summary>
        /// Giữ căn cho khác ngoại giao
        /// </summary>
        public const int GIU_CAN_CHO_KHACH_NGOAI_GIAO = 2;
        /// <summary>
        /// Huỷ phân phối
        /// </summary>
        public const int HUY_PHAN_PHOI = 3;
        /// <summary>
        /// Khác
        /// </summary>
        public const int KHAC = 4;
    }
}
