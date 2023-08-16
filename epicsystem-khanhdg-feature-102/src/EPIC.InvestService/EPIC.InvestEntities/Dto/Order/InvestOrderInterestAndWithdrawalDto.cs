using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    /// <summary>
    /// Xem dòng tiền thực tế
    /// </summary>
    public class InvestOrderInterestAndWithdrawalDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ngày trả
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// Số dư còn lại trước khi rút
        /// </summary>
        public decimal Surplus { get;set; }

        /// <summary>
        /// Số tiền rút
        /// </summary>
        public decimal? WithdrawalMoney { get; set; }

        /// <summary>
        /// Số ngày
        /// </summary>
        public int NumberOfDays { get; set; }   

        public decimal? Profit { get; set; }

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public decimal? ActuallyAmount { get; set; }

        /// <summary>
        /// Lợi tức khấu trừ
        /// </summary>
        public decimal? DeductibleProfit { get; set; }

        /// <summary>
        /// Thuế rút
        /// </summary>
        public decimal? Tax { get; set; }

        /// <summary>
        /// Lợi tức thực nhận
        /// </summary>
        public decimal? ActuallyProfit { get; set; }

        /// <summary>
        /// 1 Khởi tạo, 2: Thành công, 3: Hủy duyệt
        /// </summary>
        public int? Status { get; set; }
    }
}
