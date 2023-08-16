using EPIC.DataAccess.Models;
using EPIC.EventDomain.Implements;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEventMedia;
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

namespace EPIC.EventAPI.Controllers
{
    /// <summary>
    /// Controller Xử lý hình ảnh sự kiện
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/event/event-media")]
    [ApiController]
    public class EvtEventMediaController : BaseController
    {
        private readonly IEvtEventMediaServices _evtEventMediaServices;

        public EvtEventMediaController(ILogger<EvtEventMediaController> logger,
            IEvtEventMediaServices evtEventMediaServices)
        {
            _logger = logger;
            _evtEventMediaServices = evtEventMediaServices;
        }

        /// <summary>
        /// Tìm danh sách hình ảnh sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewEvtEventMediaDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterEvtEventMediaDto input)
        {
            try
            {
                var result = _evtEventMediaServices.FindAll(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm hình ảnh sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse<List<ViewEvtEventMediaDto>>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateEvtEventMediasDto input)
        {
            try
            {
                var result = _evtEventMediaServices.Add(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hình ảnh sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(APIResponse<ViewEvtEventMediaDto>), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] UpdateEvtEventMediaDto input)
        {
            try
            {
                var result = _evtEventMediaServices.Update(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá hình ảnh sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Delete(int id)
        {
            try
            {
                _evtEventMediaServices.Delete(id);
                return new APIResponse(null);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái hình ảnh sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeStatus(int id, string status)
        {
            try
            {
                _evtEventMediaServices.ChangeStatus(id, status);
                return new APIResponse(null);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// update thứ tự hình ảnh sự kiện
        /// </summary>
        /// <returns></returns>
        [HttpPut("sort-order")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateSortOrder(EvtEventMediaSortDto input)
        {
            try
            {
                _evtEventMediaServices.UpdateSortOrder(input);
                return new APIResponse(null);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm hình ảnh sự kiện theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(APIResponse<ViewEvtEventMediaDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _evtEventMediaServices.FindById(id);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách dự án orderBy Location
        /// </summary>
        /// <returns></returns>
        [HttpGet("find/{eventId}")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<ViewEvtEventMediaDto>>), (int)HttpStatusCode.OK)]
        public APIResponse Find(int eventId, string location, string status)
        {
            try
            {
                var result = _evtEventMediaServices.Find(eventId, location, status);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
