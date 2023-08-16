using EPIC.EventDomain.Implements;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.EventAPI.Controllers.AppControllers
{
    /// <summary>
    /// Màn đặt vé
    /// </summary>
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/event/app/order")]
    [ApiController]
    public class AppEvtOrderController : BaseController
    {
        private readonly IEvtAppOrderService _evtAppOrderServices;

        public AppEvtOrderController(ILogger<AppEvtOrderController> logger,
                        IEvtAppOrderService evtAppOrderServices)
        {
            _logger = logger;
            _evtAppOrderServices = evtAppOrderServices;
        }

        /// <summary>
        /// danh sách chi tiết sự kiện lúc đặt vé
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="isSaleView"></param>
        /// <returns></returns>
        [HttpGet("find-event-detail")]
        [ProducesResponseType(typeof(APIResponse<List<AppEventDetailDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindEventMediaByEventId(int eventId, bool isSaleView)
        {
            try
            {
                var result = _evtAppOrderServices.FindEventDetailsById(eventId, isSaleView);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thêm sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> Add([FromBody] AppCreateEvtOrderDto input)
        {
            try
            {
                var result = await _evtAppOrderServices.Add(input, true, null);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("sale/add")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse SaleAdd([FromBody] AppSaleCreateEvtOrderDto input)
        {
            try
            {
                _evtAppOrderServices.Add(input, false, input.InvestorId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy đặt vé khi lệnh ở trạng thái chờ thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("cancel/{orderId}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse CancelOrder(int orderId)
        {
            try
            {
                _evtAppOrderServices.CancelOrder(orderId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// app yeu cau nhan hoa don, ve cung
        /// </summary>
        /// <param name="input"></param>
        [HttpPut("invoice-ticket-request")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> InvoiceTicketRequest([FromBody] InvoiceTicketRequestDto input)
        {
            try
            {
                await _evtAppOrderServices.InvoiceTicketRequest(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
