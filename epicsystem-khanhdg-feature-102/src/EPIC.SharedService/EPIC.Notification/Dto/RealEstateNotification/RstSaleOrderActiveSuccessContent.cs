using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.RealEstateNotification
{
    public class RstSaleOrderActiveSuccessContent : RstOrderActiveSuccessContent
    {
        /// <summary>
        /// Tên tu vấn viên
        /// </summary>
        public string SaleName { get; set; }
        /// <summary>
        /// Thời gian đặt cọc (Thời gian hđ được duyệt) 
        /// </summary>
        public string ApproveDate { get; set; }
    }
}
