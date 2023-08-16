using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.CollectMoney
{
    /// <summary>
    /// Kết quả yêu cầu thu hộ
    /// </summary>
    public class ResultRequestCollectMoneyDto
    {
        /// <summary>
        /// Số tài khoản
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Mã qr
        /// </summary>
        public string QrCode { get; set; }
    }
}
