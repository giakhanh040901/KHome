namespace EPIC.Utils.ConstantVariables.Identity
{
    /// <summary>
    /// Loại white list ip
    /// </summary>
    public static class WhiteListIpTypes
    {
        public const string TRADING_KEY = "TradingProviderId";
        /// <summary>
        /// Lấy thông tin khách hàng cho tps
        /// </summary>
        public const int ThongTinKhachDauTuTps = 1;
        /// <summary>
        /// Lấy thông tin khách hàng cho bên bán data telesale
        /// </summary>
        public const int ThongTinKhachDauTuTelesale = 2;

        public const int InvestDuyetHopDong = 3;
        public const int InvestDuyetVaoTien = 4;
        public const int InvestDuyetChiTien = 5;

        public const int GarnerDuyetHopDong = 6;
        public const int GarnerDuyetVaoTien = 7;
        public const int GarnerDuyetChiTien = 8;

        public const int RstDuyetHopDong = 9;
        public const int RstDuyetVaoTien = 10;

        /// <summary>
        /// Các trường hợp phải truyền tradingProviderId
        /// </summary>
        public static readonly int[] MustPassTradingId =
        {
            InvestDuyetHopDong, InvestDuyetVaoTien, InvestDuyetChiTien,
            GarnerDuyetHopDong, GarnerDuyetVaoTien, GarnerDuyetChiTien, RstDuyetHopDong, RstDuyetVaoTien
        };

        /// <summary>
        /// Các trường hợp check trên root
        /// </summary>
        public static readonly int[] MustCheckRoot =
        {
            ThongTinKhachDauTuTps, ThongTinKhachDauTuTelesale
        };
    }
}
