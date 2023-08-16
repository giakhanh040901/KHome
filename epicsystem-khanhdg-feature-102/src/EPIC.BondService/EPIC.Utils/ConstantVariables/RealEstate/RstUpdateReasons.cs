using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Lý do update (nếu có)
    /// </summary>
    public class RstUpdateReasons
    {
        /// <summary>
        ///  Lý do khác
        /// </summary>
        public const int KHAC = 1;

        /// <summary>
        /// (ProductItem, OpenSellDetail: Khóa) Đã bán Offline
        /// </summary>
        public const int KHOA_BAN_OFFLINE = 2;

        /// <summary>
        /// (ProductItem, OpenSellDetail: Khóa) Giữ căn cho khách ngoại giao
        /// </summary>
        public const int KHOA_GIU_CAN_CHO_KHACH = 3;

        /// <summary>
        /// (ProductItem, OpenSellDetail: Khóa) Khóa làm tài sản đảm bảo
        /// </summary>
        public const int KHOA_TAI_SAN_DAM_BAO = 4;

        /// <summary>
        /// (ProductItem: Khóa) Hủy phân phối
        /// </summary>
        public const int KHOA_HUY_PHAN_PHOI = 5;

        /// <summary>
        /// (OpenSellDetail: Khóa) Hủy mở bán
        /// </summary>
        public const int KHOA_HUY_MO_BAN = 6;

        /// <summary>
        /// Mở khóa
        /// </summary>
        public const int MO_KHOA = 7;

        /// <summary>
        /// (Order) Lý do gia hạn đặt cọc, khách ngoại giao
        /// </summary>
        public const int KHACH_NGOAI_GIAO = 8;
        /// <summary>
        /// (Order) Lý do gia hạn đặt cọc, khách khóa căn
        /// </summary>
        public const int KHACH_XIN_GIA_HAN_THOI_GIAN = 9;
        /// <summary>
        /// (Order) Lý do gia hạn đặt cọc, khách khóa căn
        /// </summary>
        public const int LY_DO_GIA_HAN_KHAC = 10;

    }
}
