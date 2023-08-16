using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EmailInvest
{
    public class InvestSaleOrderActive : InvestOrderSuccessContent
    {
        /// <summary>
        /// Tên tu vấn viên
        /// </summary>
        public string SaleName { get; set; }
        public string InvestDate { get; set; }
    }
}
