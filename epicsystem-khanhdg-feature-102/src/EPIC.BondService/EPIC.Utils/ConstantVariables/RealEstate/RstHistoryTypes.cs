using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Loại hình của lịch sử (1: Mở bán, 2: Đặt cọc, 3: Thanh toán, 4: Huỷ cọc, 5: Giữ chỗ, 
    /// 6: Khoá căn, 7: Mở căn, 8: Bật Showapp, 9: Tắt Showapp, 10: Khởi tạo, 11: Cập nhật, 12: Phân phối)
    /// </summary>
    public static class RstHistoryTypes
    {
        public const int MoBan = 1;
        public const int DatCoc = 2;
        public const int ThanhToan = 3;
        public const int HuyCoc = 4;
        public const int GiuCho = 5;
        public const int KhoaCan = 6;
        public const int MoCan = 7;
        public const int BatShowApp = 8;
        public const int TatShowApp = 9;
        public const int KhoiTao = 10;
        public const int CapNhat = 11;
        public const int PhanPhoi = 12;
    }
}
