using EPIC.DataAccess.Models;
using EPIC.EventDomain.Implements;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEventDescriptionMedia;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace EPIC.EventAPI.Controllers
{
    /// <summary>
    /// Controller xử lý hình ảnh mô tả sự kiện
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/event/description-media")]
    [ApiController]
    public class EvtEventDescriptionMediaController : BaseController
    {
        private readonly IEvtEventDescriptionMediaServices _evtEventDescriptionMediaServices;

        public EvtEventDescriptionMediaController(ILogger<EvtEventDescriptionMediaController> logger, IEvtEventDescriptionMediaServices evtEventDescriptionMediaServices)
        {
            _logger = logger;
            _evtEventDescriptionMediaServices = evtEventDescriptionMediaServices;
        }

        /// <summary>
        /// Thêm hình ảnh mô tả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse<EvtEventDescriptionMediaDto>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateEvtEventDescriptionDto input)
        {
            try
            {
                var result = _evtEventDescriptionMediaServices.Add(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hình ảnh mô tả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(APIResponse<EvtEventDescriptionMediaDto>), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] UpdateEvtEventDescriptionMediaDto input)
        {
            try
            {
                _evtEventDescriptionMediaServices.Update(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa hình ảnh mô tả
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                _evtEventDescriptionMediaServices.Delete(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách hình ảnh mô tả theo eventId
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet("find-by-event-id/{eventId}")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtEventDescriptionMediaDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindByEventId(int eventId)
        {
            try
            {
                var result = _evtEventDescriptionMediaServices.FindByEventId(eventId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
