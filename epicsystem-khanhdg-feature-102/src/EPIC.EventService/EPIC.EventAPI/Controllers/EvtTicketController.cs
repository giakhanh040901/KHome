using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using EPIC.DataAccess.Models;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.EventEntites.Dto.EvtTicketMedia;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.EventAPI.Controllers
{
    /// <summary>
    /// Controller Xử lý loại vé của các khung giờ trong xự kiện
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/event/ticket")]
    [ApiController]
    public class EvtTicketController : BaseController
    {
        private readonly IEvtTicketService _evtTicketServices;
        private readonly IEvtOrderTicketFillService _evtOrderTicketFillServices;

        public EvtTicketController(ILogger<EvtTicketController> logger, IEvtTicketService evtTicketServices, IEvtOrderTicketFillService evtOrderTicketFillServices)
        {
            _logger = logger;
            _evtTicketServices = evtTicketServices;
            _evtOrderTicketFillServices = evtOrderTicketFillServices;
        }

        /// <summary>
        /// Thêm loại vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse<EvtTicketDto>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateEvtTicketDto input)
        {
            try
            {
                var result = _evtTicketServices.Add(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật loại vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(APIResponse<EvtTicketDto>), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] UpdateEvtTicketDto input)
        {
            try
            {
                var result = _evtTicketServices.Update(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa loại vé
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                _evtTicketServices.Delete(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách vé trong 1 khung giờ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtTicketDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterEvtTicketDto input)
        {
            try
            {
                var result = _evtTicketServices.FindAll(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Nhân bản vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("replicate-ticket")]
        [ProducesResponseType(typeof(APIResponse<EvtTicketDto>), (int)HttpStatusCode.OK)]
        public APIResponse ReplicateTicket([FromBody] CreateListRepicateTicketDto input)
        {
            try
            {
                var result = _evtTicketServices.ReplicateTicket(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái của vé
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update-status/{id}")]
        public APIResponse UpdateStatus(int id)
        {
            try
            {
                _evtTicketServices.UpdateStatus(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa ảnh của vé
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("image/delete/{id}")]
        public APIResponse DeleteTicketImage(int id)
        {
            try
            {
                _evtTicketServices.DeleteTicketImage(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái show appcủa vé
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-show-app/{id}")]
        public APIResponse ChangeShowApp(int id)
        {
            try
            {
                _evtTicketServices.ChangeShowApp(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin vé theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(APIResponse<EvtTicketDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _evtTicketServices.FindById(id);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sửa ảnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-image")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateImageUrl(UpdateTicketImageDto input)
        {
            try
            {
                _evtTicketServices.UpdateTicketImage(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Fill lại vé cho Order
        /// </summary>
        /// <param name="orderId">Id order</param>
        /// <returns></returns>
        [HttpPut("fill-order-ticket/{orderId}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> FillOrderTicket(int orderId)
        {
            try
            {
                await _evtOrderTicketFillServices.FillOrderTicket(orderId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Fill lại cho từng vé
        /// </summary>
        /// <param name="id">Id vé</param>
        /// <returns></returns>
        [HttpPut("fill-ticket/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> FillTicket(int id)
        {
            try
            {
                await _evtOrderTicketFillServices.FillTicket(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
