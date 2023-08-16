using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.CollectMoney
{
    /// <summary>
    /// Kết quả xử lý notify
    /// </summary>
    public class ResultNotificationDto
    {
        /// <summary>
        /// Tiền tố tài khoản
        /// </summary>
        public string PrefixAccount { get; set; }

        /// <summary>
        /// Mã hợp đồng = Mã dự án + OrderId 
        /// </summary>
        public string OrderCode { get; set; }
        public DateTime TranDate { get; internal set; }
        public decimal TranAmount { get; internal set; }
    }
}
