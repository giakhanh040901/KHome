using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrder
{
    public class EvtOrderExtendedTimeDto : EvtOrderLockDto
    {
        /// <summary>
        /// Thời gian gia hạn (trên fe là phút nhưng dưới be lưu là giây)
        /// </summary>
        public int KeepTime { get; set; }
    }

    public class EvtOrderLockDto : EvtReasonDto
    {
        /// <summary>
        /// ID lệnh
        /// </summary>
        public int OrderId { get; set; }
    
    }

    public class EvtOrderTickLockDto : EvtReasonDto
    {
        /// <summary>
        /// ID vé của lệnh
        /// </summary>
        public int OrderTickId { get; set; }

    }

    public class EvtReasonDto
    {
        public int Reason { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Summary { get; set; }
    }

}
