namespace EPIC.LoyaltyEntities.Dto.LoyLuckyScenarioDetail
{
    public class LoyLuckyScenarioDetailDto
    {
        public int Id { get; set; }

        public int LuckyScenarioId { get; set; }

        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Quà tặng/ Voucher
        /// </summary>
        public int? VoucherId { get; set; }

        /// <summary>
        /// Tên voucher
        /// </summary>
        public string VoucherName { get; set; }

        /// <summary>
        /// Tên quà tặng nếu không chọn voucher
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// Xác suất may mắn
        /// </summary>
        public double? Probability { get; set; }
    }
}
