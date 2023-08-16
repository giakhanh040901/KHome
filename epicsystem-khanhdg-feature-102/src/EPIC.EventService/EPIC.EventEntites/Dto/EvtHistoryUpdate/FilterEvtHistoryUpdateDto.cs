using EPIC.EntitiesBase.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtHistoryUpdate
{
    public class FilterEvtHistoryUpdateDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Bảng cập nhật: 1: EVT_EVENT, 2: EVT_ORDER, 3: EVT_ORDER_PAYMENT
        /// </summary>
        public int? UpdateTable { get; set; }
        /// <summary>
        /// Id dữ liệu trong bảng thật
        /// </summary>
        public int? RealTableId { get; set; }
    }
}
