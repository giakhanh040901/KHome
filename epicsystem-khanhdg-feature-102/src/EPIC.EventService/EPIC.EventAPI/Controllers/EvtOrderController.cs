using EPIC.DataAccess.Models;
using EPIC.EventDomain.Implements;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtHistoryUpdate;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.Controllers;
using EPIC.Utils.Net.MimeTypes;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.EventAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/event/order")]
    [ApiController]
    public class EvtOrderController : BaseController
    {
        private readonly IEvtOrderService _evtOrderServices;
        private readonly IEvtOrderValidService _evtOrderValidServices;
        private readonly EventNotificationServices _eventNotificationServices;
        private readonly IEvtDeliveryTicketTemplateService _evtDeliveryTicketTemplateService;

        public EvtOrderController(ILogger<EvtOrderController> logger, 
                IEvtOrderService evtOrderServices, 
                IEvtOrderValidService evtOrderValidServices,
                EventNotificationServices eventNotificationServices,
                IEvtDeliveryTicketTemplateService evtDeliveryTicketTemplateService
                )
        {
            _logger = logger;
            _evtOrderServices = evtOrderServices;
            _evtOrderValidServices = evtOrderValidServices;
            _eventNotificationServices = eventNotificationServices;
            _evtDeliveryTicketTemplateService = evtDeliveryTicketTemplateService;
        }

        /// <summary>
        /// thêm sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> Add([FromBody] CreateEvtOrderDto input)
        {
            try
            {
                var result = await _evtOrderServices.Add(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// cập nhật sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] UpdateEvtOrderDto input)
        {
            try
            {
                _evtOrderServices.Update(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// hủy sổ lệnh
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        [HttpPut("cancel")]
        public APIResponse Cancel(List<int> orderIds)
        {
            try
            {
                _evtOrderServices.Cancel(orderIds);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// gia han thoi gian
        /// <see cref="EvtUpdateReason"/>
        /// </summary>
        /// <param name="input"></param>
        [HttpPut("extended-time")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ExtendedKeepTime([FromBody] EvtOrderExtendedTimeDto input)
        {
            try
            {
                _evtOrderServices.ExtendedTime(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// khoa lenh
        /// </summary>
        /// <param name="input"></param>
        [HttpPut("lock")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse OrderLock([FromBody] EvtOrderLockDto input)
        {
            try
            {
                _evtOrderValidServices.OrderLock(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// khoa ve trong lenh
        /// </summary>
        /// <param name="input"></param>
        [HttpPut("ticket-lock")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse OrderTickLock([FromBody] EvtOrderTickLockDto input)
        {
            try
            {
                _evtOrderValidServices.OrderTickLock(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// mo khoa ve trong lenh
        /// </summary>
        /// <param name="orderTickId"></param>
        /// <returns></returns>
        [HttpPut("ticket-unlock")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse OrderTickUnlock(int orderTickId)
        {
            try
            {
                _evtOrderValidServices.OrderTickChangeStatus(orderTickId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xac nhan ve da tham gia ( goi lan 1 la checkin, lan 2 la checkout)
        /// </summary>
        /// <param name="orderTickId"></param>
        /// <returns></returns>
        [HttpPut("ticket-confirm-participation")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse OrderTickConfirmParticipation(int orderTickId)
        {
            try
            {
                _evtOrderValidServices.OrderTickChangeStatus(orderTickId, EvtOrderTicketStatus.DA_THAM_GIA);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// doi ma gioi thieu
        /// </summary>
        /// <param name="input"></param>
        [HttpPut("change-referral-code")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeReferralCode([FromBody] UpdateReferralCode input)
        {
            try
            {
                _evtOrderValidServices.ChangeReferralCode(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// phê duyệt số lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("approve")]
        public async Task<APIResponse> Aprove(int orderId)
        {
            try
            {
                await _evtOrderServices.Approve(orderId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa sổ lệnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> Delete(int id)
        {
            try
            {
                await _evtOrderServices.Delete(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách số lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtOrderDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterEvtOrderDto input)
        {
            try
            {
                var result = _evtOrderServices.FindAll(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sach xu ly
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-processing")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtOrderDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllProcessing([FromQuery] FilterEvtOrderDto input)
        {
            try
            {
                input.Status = EvtOrderStatus.CHO_XU_LY;
                var result = _evtOrderServices.FindAll(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách ve ban hop le
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-valid-orders")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtOrderValidDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllOrderValid([FromQuery] FilterEvtOrderValidDto input)
        {
            try
            {
                var result = _evtOrderValidServices.FindAll(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// mở khóa lệnh
        /// </summary>
        /// <param name="id">Id order</param>
        /// <returns></returns>
        [HttpPut("unlock-order/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UnLockOrder(int id)
        {
            try
            {
                _evtOrderValidServices.OrderUnLock(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gửi thông báo đăng ký vé thành công
        /// </summary>
        /// <param name="id">Id order</param>
        /// <returns></returns>
        [HttpPut("send-notify/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> SendNotifyRegisterSuccess(int id)
        {
            try
            {
                await _eventNotificationServices.SendNotifyRegisterTicket(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sach quan ly tham gia
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-order-ticket")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtOrderTicketDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllOrderTicket([FromQuery] FilterEvtOrderTicketDto input)
        {
            try
            {
                var result = _evtOrderValidServices.FindAllOrderTicket(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách thông tin vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-order-ticket-info")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtOrderTicketInfo>>), (int)HttpStatusCode.OK)]
        public APIResponse GetOrderTicketInfoById([FromQuery] FilterEvtOrderTicketInfoDto input)
        {
            try
            {
                var result = _evtOrderServices.GetOrderTicketInfoById(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// chi tiết sổ lệnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<EvtOrderDto>), (int)HttpStatusCode.OK)]
        public APIResponse GetById(int id)
        {
            try
            {
                var result = _evtOrderServices.GetById(id);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sách lịch sử theo bảng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-history")]
        [ProducesResponseType(typeof(APIResponse<EvtHistoryUpdateDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllByTable([FromQuery] FilterEvtHistoryUpdateDto input)
        {
            try
            {
                var result = _evtOrderServices.FindAllByTable(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái giao nhận vé sự kiện, Trạng thái giao nhận vé sự kiện: (1: Chờ xử lý, 2: Đang giao, 3: Hoàn thành)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="deliveryStatus"></param>
        /// <returns></returns>
        [HttpPut("change-delivery-status")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeDeliveryStatus(int orderId, int deliveryStatus)
        {
            try
            {
                _evtOrderServices.ChangeDeliveryStatus(orderId, deliveryStatus);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái giao hoa don, Trạng thái giao nhận hoa don: (1: Chờ xử lý, 2: Đang giao, 3: Hoàn thành)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="deliveryStatus"></param>
        /// <returns></returns>
        [HttpPut("change-delivery-invoice-status")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeDeliveryInvoiceStatus(int orderId, int deliveryStatus)
        {
            try
            {
                _evtOrderServices.ChangeDeliveryStatus(orderId, deliveryStatus, EvtOrderDeliveryTypes.INVOICE);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách hợp đồng giao nhận vé sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-delivery")]
        [ProducesResponseType(typeof(APIResponse<EvtOrderDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllDelivery([FromQuery] FilterEvtOrderDeliveryTicketDto input)
        {
            try
            {
                var result = _evtOrderServices.FindAllDelivery(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách giao nhận hoa don
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-delivery-invoice")]
        [ProducesResponseType(typeof(APIResponse<EvtOrderDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllDeliveryInvoice([FromQuery] FilterEvtOrderDeliveryInvoiceDto input)
        {
            try
            {
                var result = _evtOrderServices.FindAllDeliveryInvoice(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Yêu cầu nhận hoa don
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("invoice-request")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> InvoiceRequest(int orderId)
        {
            try
            {
                await _evtOrderServices.InvoiceRequest(orderId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Yêu cầu nhận vé bản cứng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("receive-hard-ticket")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> ReceiveHardTicket(int orderId)
        {
            try
            {
                await _evtOrderServices.ReceiveHardTicket(orderId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tải file giao nhận vé
        /// </summary>
        /// <param name="orderId">ID hợp đồng</param>
        /// <returns></returns>
        [HttpGet("delivery-ticket/export-file/{orderId}")]
        public async Task<IActionResult> ExportDeliveryTicketTemplate(int orderId)
        {
            try
            {
                var result = await _evtDeliveryTicketTemplateService.FillDeliveryTicket(orderId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}
