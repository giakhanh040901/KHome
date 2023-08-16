using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Event
{
    /// <summary>
    /// Trạng thái vé đã tham gia hay chưa sau khi đặt lệnh và thanh toán thành công
    /// </summary>
    public static class EvtOrderTicketStatus
    {
        public const int CHUA_THAM_GIA = 1;
        public const int DA_THAM_GIA = 2;
        public const int TAM_KHOA = 3;
    }
}
