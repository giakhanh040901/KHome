using EPIC.DataAccess.Models;
using EPIC.EntitiesBase.Dto;
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

namespace EPIC.EventAPI.Controllers.AppControllers
{
    /// <summary>
    /// Màn tìm kiếm event, chi tiết event
    /// </summary>
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/event/app/event")]
    [ApiController]
    public class AppEvtEventController : BaseController
    {
        private readonly IEvtAppEventService _evtAppEventServices;

        public AppEvtEventController(ILogger<AppEvtEventController> logger,
            IEvtAppEventService evtAppEventService
          )
        {
            _logger = logger;
            _evtAppEventServices = evtAppEventService;
        }

        [ProducesResponseType(typeof(APIResponse<List<AppEvtSearchEventDto>>), (int)HttpStatusCode.OK)]
        [HttpGet("search-history")]
        public APIResponse SearchHistoryEvent([FromQuery] PagingRequestBaseDto input)
        {
            try
            {
                var data = _evtAppEventServices.SearchHistoryEvent(input);
                return new APIResponse(data);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thêm lịch sử tìm kiếm
        /// </summary>
        /// <param name="eventId"></param>
        [HttpPost("search-history")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse AddSearchHistoryEvent([FromQuery] int eventId)
        {
            try
            {
                _evtAppEventServices.AddSearchHistoryEvent(eventId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa tất cả lịch sử tìm kiếm của investor
        /// </summary>
        [HttpDelete("search-history")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse DeleteSearchHistoryEvent()
        {
            try
            {
                _evtAppEventServices.DeleteSearchHistoryEvent();
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// chi tiết sự kiện
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="isSaleView"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(APIResponse<AppEventDetailsDto>), (int)HttpStatusCode.OK)]
        [HttpGet("event-details")]
        public APIResponse AppEventDetail(int eventId, bool isSaleView)
        {
            try
            {
                var data = _evtAppEventServices.FindEventDetailsById(eventId, isSaleView);
                return new APIResponse(data);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sự kiện theo tên
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(APIResponse<List<AppEvtSearchEventDto>>), (int)HttpStatusCode.OK)]
        [HttpGet("search")]
        public APIResponse SearchEvent([FromQuery] AppFilterEventDto input)
        {
            try
            {
                var data = _evtAppEventServices.SearchEvent(input);
                return new APIResponse(data);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// event liên quan
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(APIResponse<List<AppRelatedEventsDto>>), (int)HttpStatusCode.OK)]
        [HttpGet("related-events")]
        public APIResponse FindRelatedEventsById([FromQuery] AppFilterEventDto input)
        {
            try
            {
                var data = _evtAppEventServices.FindRelatedEventsById(input);
                return new APIResponse(data);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách sự kiện nổi bật
        /// </summary>
        /// <returns></returns>
        [HttpGet("event-highlight")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(APIResponse<AppEvtEventDto>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllEventHighlight(bool isSaleView)
        {
            try
            {
                var result = _evtAppEventServices.AppFindHighlightEvent(isSaleView);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách sự kiện của hệ thống (nếu lấy pageSize = -1 thì lấy hết sự kiện để dùng cho màn Maps)
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(APIResponse<PagingResult<AppViewEventDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllEvent([FromQuery] AppFilterEvtEventDto input)
        {
            try
            {
                var result = _evtAppEventServices.AppFindEvent(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
