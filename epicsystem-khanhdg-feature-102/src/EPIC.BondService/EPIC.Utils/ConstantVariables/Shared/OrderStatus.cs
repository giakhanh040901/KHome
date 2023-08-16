using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Shared
{
    /// <summary>
    /// Trạng thái sổ lệnh
    /// </summary>
    public static class OrderStatus
    {
        public const int KHOI_TAO = 1;
        public const int CHO_THANH_TOAN = 2;
        public const int CHO_KY_HOP_DONG = 3;
        public const int CHO_DUYET_HOP_DONG = 4;
        public const int DANG_DAU_TU = 5;
        public const int PHONG_TOA = 6;
        public const int GIAI_TOA = 7;
        public const int TAT_TOAN = 8;
    }

    public class OrderGroupStatus
    {
        /// <summary>
        /// Tìm kiếm sổ lệnh theo trạng thái khởi tạo và chờ ký hợp đồng
        /// </summary>
        public const int SO_LENH = 1;

        /// <summary>
        /// Tìm kiếm sổ lệnh theo trạng thái xử lý hợp đồng
        /// </summary>
        public const int XU_LY_HOP_DONG = 2;

        /// <summary>
        /// Tìm kiếm sổ lệnh theo trạng thái đang đầu tư
        /// </summary>
        public const int DANG_DAU_TU = 3;

        /// <summary>
        /// Tìm kiếm sổ lệnh theo trạng thái phong tỏa hoặc giải tỏa
        /// </summary>
        public const int PHONG_TOA = 4;
    }

    public class AppOrderGroupStatus
    {

        /// <summary>
        /// Tìm kiếm sổ lệnh theo trạng thái đang đầu tư và phong tỏa
        /// </summary>
        public const int DANG_DAU_TU = 1;

        /// <summary>
        /// Tìm kiếm sổ lệnh theo trạng thái khởi tạo và chờ ký hợp đồng
        /// </summary>
        public const int SO_LENH = 2;

        /// <summary>
        /// Tìm kiếm sổ lệnh theo trạng thái tất toán và tất toán trước hạn
        /// </summary>
        public const int DA_TAT_TOAN = 3;
    }
}
