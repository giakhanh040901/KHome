namespace EPIC.Utils
{
    /// <summary>
    /// Kiểu trả lãi
    /// </summary>
    public static class InterestTypes
    {
        public const int DINH_KY = 1;
        public const int CUOI_KY = 2;
        public const int NGAY_CO_DINH = 3;
        public const int NGAY_DAU_THANG = 4;
        public const int NGAY_CUOI_THANG = 5;

        /// <summary>
        /// Tên của Kiểu trả lãi
        /// </summary>
        public static string InterestTypeNames(int? interestType, int? repeatFixedDate)
        {
            return interestType switch
            {
                DINH_KY => "Định kỳ",
                CUOI_KY => "Cuối kỳ",
                NGAY_CO_DINH => $"Ngày {repeatFixedDate} hàng tháng",
                NGAY_DAU_THANG => "Ngày đầu tiên của tháng",
                NGAY_CUOI_THANG => "Ngày cuối cùng của tháng",
                _ => string.Empty
            };
        }
    }
}
