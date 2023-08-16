using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class AppWithdrawalRequestDto
    {
        /// <summary>
        /// Id chính sách
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// Id ngân hàng thụ hưởng nhận
        /// </summary>
        public int BankAccountId { get; set; }

        /// <summary>
        /// Số tiền rút
        /// </summary>
        public decimal AmountMoney { get; set; }

        /// <summary>
        /// Mã Otp
        /// </summary>
        public string Otp { get; set; }
        /// <summary>
        /// Ngày yêu cầu rút vốn
        /// </summary>
        public DateTime? WithdrawDate { get; set; }
    }
}
