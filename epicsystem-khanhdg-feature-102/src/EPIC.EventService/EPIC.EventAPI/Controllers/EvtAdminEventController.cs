using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtAdminEvent;
using EPIC.EventEntites.Dto.EvtConfigContractCode;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace EPIC.EventAPI.Controllers
{
    /// <summary>
    /// Controller Xử lý cấu trúc mã hợp đồng
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/event/admin-event")]
    [ApiController]
    public class EvtAdminEventController : BaseController
    {
        private readonly IEvtAdminEventService _evtAdminEventService;

        public EvtAdminEventController(ILogger<EvtAdminEventController> logger, IEvtAdminEventService evtAdminEventService)
        {
            _logger = logger;
            _evtAdminEventService = evtAdminEventService;
        }

        /// <summary>
        /// Thêm admin của sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Add(CreateEvtAdminEventDto input)
        {
            try
            {
                _evtAdminEventService.Add(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm admin của sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Delete(int id)
        {
            try
            {
                _evtAdminEventService.Delete(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm admin của sự kiện
        /// </summary>
        /// <param name="eventId">Id sự kiện</param>
        /// <returns></returns>
        [HttpGet("find-all/{eventId}")]
        [ProducesResponseType(typeof(APIResponse<EvtAdminEventDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll(int eventId)
        {
            try
            {
                var result = _evtAdminEventService.FindAll(eventId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
