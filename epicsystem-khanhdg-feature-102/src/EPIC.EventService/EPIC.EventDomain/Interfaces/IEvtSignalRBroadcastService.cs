using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtSignalRBroadcastService
    {
        /// <summary>
        /// Quét mã QR vé và giờ check in check out
        /// </summary>
        /// <param name="orderTicketDetailId">Id orderTicketDetail</param>
        /// <returns></returns>
        Task BroadcastOrderTicketDetail(int orderTicketDetailId);
        /// <summary>
        /// Các thông số vé của sự kiện
        /// </summary>
        /// <param name="eventId">Id vé</param>
        /// <returns></returns>
        Task BroadcastEventQuantityTicket(int eventId);
        /// <summary>
        /// Tắt thời gian chờ thanh toán của order trên app khi phê duyệt thanh toán CMS
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task BroadcastOrderExpiredTime(int orderId);
        /// <summary>
        /// Trạng thái lệnh khi active hợp đồng hợp lệ
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task BroadcastOrderActive(int orderId);
        Task BroadcastOrderPaymentActive(int orderPaymentId);
    }
}
