namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public class RstOrderStatus
    {
        public const int KHOI_TAO = 1;
        public const int CHO_THANH_TOAN_COC = 2;
        public const int CHO_KY_HOP_DONG = 3;
        public const int CHO_DUYET_HOP_DONG_COC = 4;
        public const int DA_COC = 5;
        public const int DA_BAN = 6;

        /// <summary>
        /// Khi đưa trạng thái căn hộ về trạng thái ban đầu, hợp đồng có thanh toán sẽ sang phong tỏa
        /// </summary>
        public const int PHONG_TOA = 7;
    }

    public class RstAppOrderGroupStatus
    {

        /// <summary>
        /// Tìm kiếm sổ lệnh theo trạng thái chờ giao dịch cọc hoặc giữ chỗ
        /// </summary>
        public const int SO_LENH = 1;

        /// <summary>
        /// Tìm kiếm sổ lệnh theo trạng đang giao dịch cọc, chờ duyệt hợp đồng cọc
        /// </summary>
        public const int DANG_GIAO_DICH = 2;

        /// <summary>
        /// Tìm kiếm sổ lệnh theo Căn hộ đã đặt cọc thành công và đã duyệt hợp đồng mua bán kể cả chưa thanh toán đủ hợp đồng
        /// </summary>
        public const int DANG_SO_HUU = 3;
    }
}
