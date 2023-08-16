namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class RstCountProductItemSignalRDtoBase
    {
        /// <summary>
        /// Dự án nào
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Số lượng chưa mở bán
        /// </summary>
        public int NotOpenCount { get; set; }
        /// <summary>
        /// Số lượng đang mở bán
        /// </summary>
        public int OpenCount { get; set; }
        /// <summary>
        /// Số lượng giữ chỗ
        /// </summary>
        public int HoldCount { get; set; }
        /// <summary>
        /// Số lượng khoá căn
        /// </summary>
        public int LockCount { get; set; }
        /// <summary>
        /// Số lượng đặt cọc
        /// </summary>
        public int DepositCount { get; set; }
        /// <summary>
        /// Số lượng đã bán
        /// </summary>
        public int SoldCount { get; set; }
    }

    public class AppCountProductItemSignalRDto : RstCountProductItemSignalRDtoBase
    {
        public int OpenSellId { get; set; }

        /// <summary>
        /// Tổng số căn hộ giữ chỗ có phát sinh thanh toán
        /// </summary>
        public int PaymentOrderArisingCount { get; set; }
    }

    public class RstCountProductItemTradingSignalRDto : RstCountProductItemSignalRDtoBase
    {
        public int TradingProviderId { get; set; }
        public int DistributionId { get; set; }
    }
}
