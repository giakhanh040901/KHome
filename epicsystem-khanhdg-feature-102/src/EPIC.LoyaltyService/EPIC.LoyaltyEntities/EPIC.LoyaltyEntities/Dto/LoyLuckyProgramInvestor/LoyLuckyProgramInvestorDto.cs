namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor
{
    public class LoyLuckyProgramInvestorDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Id Nhà đầu tư
        /// </summary>
        public int InvestorId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Họ tên  
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Hạng thành viên
        /// </summary>
        public string RankName { get; set; }

        /// <summary>
        /// Điểm hiện tại
        /// </summary>
        public int TotalPoint { get; set; }

        /// <summary>
        /// Số lượng voucher đang có
        /// </summary>
        public int VoucherNum { get; set; }

        /// <summary>
        /// Đã tham gia chương trình hay chưa
        /// </summary>
        public bool IsJoin { get; set; }
    }
}
