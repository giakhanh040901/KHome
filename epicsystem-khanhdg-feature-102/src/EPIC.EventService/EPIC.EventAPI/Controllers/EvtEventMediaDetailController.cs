using EPIC.EventDomain.Implements;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEventMedia;
using EPIC.EventEntites.Dto.EvtEventMediaDetail;
using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
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
    /// Controller Xử lý nhóm hình ảnh sự kiện
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/event/event-media-detail")]
    [ApiController]
    public class EvtEventMediaDetailController : BaseController
    {
        private readonly IEvtEventMediaDetailServices _evtEventMediaDetailServices;

        public EvtEventMediaDetailController(ILogger<EvtEventMediaDetailController> logger,
            IEvtEventMediaDetailServices evtEventMediaDetailServices)
        {
            _logger = logger;
            _evtEventMediaDetailServices = evtEventMediaDetailServices;
        }

        /// <summary>
        /// Thêm nhóm hình ảnh sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<EvtEventMediaDetailDto>>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateEvtEventMediaDetailsDto input)
        {
            try
            {
                var result = _evtEventMediaDetailServices.Add(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thêm list media detail vào media
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-list-detail")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<EvtEventMediaDetailDto>>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] AddEvtEventMediaDetailsDto input)
        {
            try
            {
                var result = _evtEventMediaDetailServices.AddListMediaDetail(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật nhóm hình ảnh sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(APIResponse<EvtEventMediaDetailDto>), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] UpdateEvtEventMediaDetailDto input)
        {
            try
            {
                var result = _evtEventMediaDetailServices.Update(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá nhóm hình ảnh sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Delete(int id)
        {
            try
            {
                _evtEventMediaDetailServices.Delete(id);
                return new APIResponse(null);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái nhóm hình ảnh dự án
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
                _evtEventMediaDetailServices.ChangeStatus(id, status);
                return new APIResponse(null);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm nhóm hình ảnh sự kiện theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(APIResponse<EvtEventMediaDetailDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _evtEventMediaDetailServices.FindById(id);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách nhóm hình ảnh sự kiện order by tên nhóm hình ảnh
        /// </summary>
        /// <returns></returns>
        [HttpGet("find/{eventId}")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<EvtEventMediaDto>>), (int)HttpStatusCode.OK)]
        public APIResponse Find(int eventId, string status)
        {
            try
            {
                var result = _evtEventMediaDetailServices.Find(eventId, status);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// update thứ tự nhóm hình ảnh sự kiện
        /// </summary>
        /// <returns></returns>
        [HttpPut("sort-order")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateSortOrder(EvtEventMediaDetailSortDto input)
        {
            try
            {
                _evtEventMediaDetailServices.UpdateSortOrder(input);
                return new APIResponse(null);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
