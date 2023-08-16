namespace EPIC.GarnerEntities.Dto.GarnerExportExcel
{
    public class GarnerListAdministrationDto : GarnerListInvestmentDto
    {
        /// <summary>
        /// Loại hình kỳ hạn
        /// </summary>
        public new string GarnerType { get; set; }

        /// <summary>
        /// Loại giao dich
        /// </summary>
        public string TranClassify { get; set; }

        /// <summary>
        /// Lợi tức
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// Phần trăm lợi tức
        /// </summary>
        public string ProfitRate { get; set; }

        /// <summary>
        /// Nguồn đặt
        /// </summary>
        public string Orderer { get; set; }

        /// <summary>
        /// Giá trị đầu tư theo hợp đồng
        /// </summary>
        public decimal InitTotalValue { get; set; }

        public string SaleReferralCodeSub { get; set; }

        public long OrderId { get; set; }
    }
}
