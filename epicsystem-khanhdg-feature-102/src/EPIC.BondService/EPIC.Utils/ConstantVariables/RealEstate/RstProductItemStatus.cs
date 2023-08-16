using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Trạng thái của sản phẩm (1: Khởi tạo (có thể là chưa mở bán hoặc đang mở bán) 2: Giữ chỗ, 3: Khóa căn, 4: Đã cọc, 5: Đã bán) 
    /// 2 trạng thái 6: chưa mở bán và 7: đang bán 
    /// </summary>
    public static class RstProductItemStatus
    {
        public const int KHOI_TAO = 1;
        /// <summary>
        /// Trạng thái logic khi đang còn thời gian đếm ngược chuyển tiền cọc
        /// </summary>
        public const int GIU_CHO = 2;
        public const int KHOA_CAN = 3;
        public const int DA_COC = 4;
        public const int DA_BAN = 5;
        /// <summary>
        /// Trạng thái logic chưa mở bán 
        /// Xem trong Mở bán khi CHƯA tới ngày bắt đầu mở bán
        /// Xem sản phẩm căn ở Bảng hàng dự án - phân phối khi CHƯA được tạo mở bán
        /// </summary>
        public const int LOGIC_CHUA_MO_BAN = 6;
        /// <summary>
        /// Trạng thái logic đang mở bán 
        /// Xem trong Mở bán khi ĐÃ tới mở bán
        /// Xem sản phẩm căn ở Bảng hàng dự án - phân phối khi đã được tạo mở bán
        /// </summary>
        public const int LOGIC_DANG_MO_BAN = 7;
    }
}
