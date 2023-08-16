using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    /// <summary>
    /// Chi tiết rút cho từng lệnh
    /// </summary>
    public class GarnerWithdrawalDetailDto
    {
        /// <summary>
        /// Id lệnh
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// Lợi tức rút
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// Phần trăm lợi tức khi rút
        /// </summary>
        public decimal ProfitRate { get; set; }
        /// <summary>
        /// Lợi tức khấu trừ
        /// </summary>
        public decimal DeductibleProfit { get; set; }
        /// <summary>
        /// Thuế lợi nhuận
        /// </summary>
        public decimal Tax { get; set; }
        /// <summary>
        /// Lợi tức thực nhận
        /// </summary>
        public decimal ActuallyProfit { get; set; }
        /// <summary>
        /// Phí rút
        /// </summary>
        public decimal WithdrawalFee { get; set; }
        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public decimal AmountReceived { get; set; } 
        
        /// <summary>
        /// Số tiền rút
        /// </summary>
        public decimal AmountMoney { get; set; }
    }
}
