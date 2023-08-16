namespace EPIC.LoyaltyEntities.Dto.LoyLuckyScenarioDetail
{
    public class CreateLoyLuckyScenarioDetailDto
    {
        public int? VoucherId { get; set; }
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public int SortOrder { get; set; }

        private string _name;
        /// <summary>
        /// Tên quà tặng nếu không chọn voucher
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

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
