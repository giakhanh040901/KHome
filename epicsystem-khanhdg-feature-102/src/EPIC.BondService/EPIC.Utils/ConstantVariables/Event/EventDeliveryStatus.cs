using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Event
{
    public static class EventDeliveryStatus
    {
        /// <summary>
        /// chờ xử lý
        /// </summary>
        public const int WAITING = 1;
        /// <summary>
        /// đang giao
        /// </summary>
        public const int DELIVERY = 2;
        
        /// <summary>
        /// Hoàn thành
        /// </summary>
        public const int COMPLETE = 3;
    }
}
