using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEventHistory;
using EPIC.RealEstateEntities.Dto.RstCart;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.EventAPI.Controllers.AppControllers
{
    /// <summary>
    /// Xử lý màn lịch sử sự kiện app
    /// </summary>
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/event/app/event-history")]
    [ApiController]
    public class AppEvtEventHistoryController : BaseController
    {
        private readonly IEvtAppEventHistoryServices _evtAppEventHistoryServices;

        public AppEvtEventHistoryController(ILogger<AppEvtEventHistoryController> logger,
            IEvtAppEventHistoryServices evtAppEventHistoryServices)
        {
            _logger = logger;
            _evtAppEventHistoryServices = evtAppEventHistoryServices;
        }

        /// <summary>
        /// Danh sách lịch sử
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse FindAll([FromQuery] FilterEvtEventHistoryDto input)
        {
            try
            {
                var result = _evtAppEventHistoryServices.FindAll(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chi tiết lịch sử sự kiện theo orderId
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-detail/{orderId}")]
        public APIResponse FindByOrderId(int orderId)
        {
            try
            {
                var result = _evtAppEventHistoryServices.FindByOrderId(orderId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chi tiết lịch sử sự kiện theo ticketId
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-ticket-detail/{orderDetailId}")]
        public APIResponse FindByOrderDetailId(int orderDetailId)
        {
            try
            {
                var result = _evtAppEventHistoryServices.FindByOrderDetailId(orderDetailId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Màn thanh toán lịch sử sự kiện
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-bank-account/{orderId}")]
        public async Task<APIResponse> FindTradingBankAccountOfEvent(int orderId)
        {
            try
            {
                var result = await _evtAppEventHistoryServices.FindTradingBankAccountOfEvent(orderId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
