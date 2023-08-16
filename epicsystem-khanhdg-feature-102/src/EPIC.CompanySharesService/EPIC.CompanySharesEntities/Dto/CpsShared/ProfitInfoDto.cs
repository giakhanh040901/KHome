using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CompanySharesShared
{
    /// <summary>
    /// Thông tin lợi tức
    /// </summary>
    public class ProfitInfoDto
    {
        public int ProfitId { get; set; }

        /// <summary>
        /// Kỳ nhận (kỳ 1, kỳ 2,...)
        /// </summary>
        public int ReceivePeriod { get; set; }

        /// <summary>
        /// Lợi tức (lãi suất)
        /// </summary>
        public decimal? ProfitRate { get; set; }

        /// <summary>
        /// Ngày trả lợi tức
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// Lợi tức
        /// </summary>
        public decimal? Profit { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân
        /// </summary>
        public decimal? Tax { get; set; }

        /// <summary>
        /// Lợi tức thực nhận, nếu là kỳ cuối + số tiền đầu tư
        /// </summary>
        public decimal? ActuallyProfit { get; set; }

        /// <summary>
        /// Số ngày
        /// </summary>
        public int? NumberOfDays { get; set; }

        /// <summary>
        /// Trạng thái (đến hạn, chưa đến hạn)
        /// </summary>
        public int? Status { get; set; }
    }
}
