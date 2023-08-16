namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor
{
    public class LoyInvestorOfTradingDto
    {
        public int InvestorId { get; set; }
        public int? UserId { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }
        /// <summary>
        /// Username của tài khoản
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Sdt của khách
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email của khách
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Giới tính của khách
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// Tên của khách
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Id Hạng
        /// </summary>
        public int? RankId { get; set; }

        /// <summary>
        /// Tên hạng
        /// </summary>
        public string RankName { get; set; }

        /// <summary>
        /// Nếu truyền chương trình, kiểm tra xem đã được tham gia hay chưa
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
