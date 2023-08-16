using EPIC.EventEntites.Dto.EvtEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventHistory
{
    public class AppViewEventHistoryDto : AppViewEventDto
    {
        public int OrderId { get; set; }
        /// <summary>
        /// Trạng thái của sổ lệnh
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Sự kiện đã diễn ra hay chưa
        /// </summary>
        public bool IsTookPlace { get; set; }
        /// <summary>
        /// Ngày hết hạn thanh toán vé
        /// </summary>
        public DateTime? ExpiredTime { get; set; }
        /// <summary>
        /// Ngày để sort
        /// </summary>
        public DateTime? SortDate { get; set; }
        /// <summary>
        /// Số lượng vé đăng ký
        /// </summary>
        public int Quantity { get; set; }
    }
}
