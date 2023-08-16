using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtHistoryUpdate;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{

    public interface IEvtOrderService 
    {
        /// <summary>
        /// thêm sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<int> Add(CreateEvtOrderDto input);

        /// <summary>
        /// cập nhật sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void Update(UpdateEvtOrderDto input);

        /// <summary>
        /// xóa sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Hủy sổ lệnh
        /// </summary>
        /// <param name="orderIds"></param>
        void Cancel(List<int> orderIds);

        /// <summary>
        /// phê duyệt số lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task Approve(int orderId);

        /// <summary>
        /// danh sach sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EvtOrderDto> FindAll(FilterEvtOrderDto input);

        /// <summary>
        /// chi tiết sổ lệnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EvtOrderDto GetById(int id);

        /// <summary>
        /// gia han thoi gian
        /// <see cref="EvtUpdateReason"/>
        /// </summary>
        /// <param name="input"></param>
        void ExtendedTime(EvtOrderExtendedTimeDto input);

        PagingResult<EvtHistoryUpdateDto> FindAllByTable(FilterEvtHistoryUpdateDto input);

        /// <summary>
        /// Thông tin vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EvtOrderTicketInfo> GetOrderTicketInfoById(FilterEvtOrderTicketInfoDto input);

        PagingResult<EvtOrderDto> FindAllDeliveryInvoice(FilterEvtOrderDeliveryInvoiceDto input);

        /// <summary>
        /// đổi trạng thái giao nhận vé hoặc hóa đơn
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="deliveryStatus"></param>
        /// <see cref="EvtOrderDeliveryTypes"/>
        /// <param name="type"></param>
        void ChangeDeliveryStatus(int orderId, int deliveryStatus, int type = EvtOrderDeliveryTypes.TICKET);
        PagingResult<EvtOrderDto> FindAllDelivery(FilterEvtOrderDeliveryTicketDto input);
        Task ReceiveHardTicket(int orderId);

        /// <summary>
        /// yeu cau nhan hoa don
        /// </summary>
        /// <param name="orderId"></param>
        Task InvoiceRequest(int orderId);
    }
}
