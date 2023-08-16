using EPIC.DataAccess.Models;
using EPIC.EventDomain.Implements;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtOrderPayment;
using EPIC.Notification.Services;
using EPIC.RealEstateEntities.Dto.RstOrderPayment;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.Utils.Validation;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.EventAPI.Controllers
{
    /// <summary>
    /// Thanh toán sổ lệnh
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/event/order-payment")]
    [ApiController]
    public class EvtOrderPaymentController : BaseController
    {
        private readonly IEvtOrderPaymentServices _evtOrderPaymentServices;
        private readonly EventNotificationServices _eventNotificationServices;

        public EvtOrderPaymentController(ILogger<EvtOrderPaymentController> logger,
            IEvtOrderPaymentServices evtOrderPaymentServices,
            EventNotificationServices eventNotificationServices)
        {
            _logger = logger;
            _evtOrderPaymentServices = evtOrderPaymentServices;
            _eventNotificationServices = eventNotificationServices;
        }

        /// <summary>
        /// lấy danh sách thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtOrderPaymentDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterEvtOrderPaymentDto input)
        {
            try
            {
                var result = _evtOrderPaymentServices.FindAll(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm thông tin thanh toán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        [ProducesResponseType(typeof(APIResponse<EvtOrderPaymentDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _evtOrderPaymentServices.FindById(id);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse<EvtOrderPaymentDto>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateEvtOrderPaymentDto input)
        {
            try
            {
                var result = _evtOrderPaymentServices.Add(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] UpdateEvtOrderPaymentDto input)
        {
            try
            {
                _evtOrderPaymentServices.Update(input);
                return new APIResponse(null);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Delete(int id)
        {
            try
            {
                _evtOrderPaymentServices.Delete(id);
                return new APIResponse(null);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt thanh toán, hủy thanh toán
        /// duyệt thanh toán nếu đủ tiền thì update trạng thái order thành chờ xử lý, nếu k đủ tiền thì về chờ thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpPut("approve/{orderPaymentId}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> OrderPaymentApprove(int orderPaymentId,
            [IntegerRange(AllowableValues = new int[] { OrderPaymentStatus.DA_THANH_TOAN, OrderPaymentStatus.HUY_THANH_TOAN })] int status)
        {
            try
            {
                await _evtOrderPaymentServices.ApprovePayment(orderPaymentId, status);
                return new APIResponse(null);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm danh sách tài khoản nhận tiền
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("find-list-bank")]
        [ProducesResponseType(typeof(APIResponse<EvtOrderPaymentDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindListBank(int orderId)
        {
            try
            {
                var result = _evtOrderPaymentServices.FindListBank(orderId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gửi thông báo chuyển tiền thành công khi duyệt thanh toán chuyển tiền
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        [HttpPost("send-notify/{orderPaymentId}")]
        public async Task<APIResponse> ResendNotify(int orderPaymentId)
        {
            try
            {
                await _eventNotificationServices.SendNotifyApprovePaymentOrder(orderPaymentId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
