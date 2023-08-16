using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Event
{
    /// <summary>
    /// Lịch sử thay đổi: các bảng update
    /// </summary>
    public static class EvtHistoryUpdateTables
    {
        /// <summary>
        /// Bảng sự kiện EVT_EVENT
        /// </summary>
        public const int EVT_EVENT = 1;
        public const int EVT_ORDER = 2;
        public const int EVT_ORDER_PAYMENT = 3;
        public const int EVT_ORDER_TICKET = 4;
    }
}
