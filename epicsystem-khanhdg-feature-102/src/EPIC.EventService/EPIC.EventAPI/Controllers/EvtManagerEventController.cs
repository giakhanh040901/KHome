using EPIC.EventDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.EventEntites.Dto.EvtEventDetail;
using System.Collections.Generic;
using EPIC.EventEntites.Dto.EvtHistoryUpdate;
using System.Threading.Tasks;
using EPIC.EventEntites.Dto.EvtTicketTemplate;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Net.MimeTypes;
using System.ComponentModel.DataAnnotations;
using EPIC.EventEntites.Dto.EvtDeliveryTicketTemplate;

namespace EPIC.EventAPI.Controllers
{
    /// <summary>
    /// Controller Xử lý xử kiện
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/event/manager-event")]
    [ApiController]
    public class EvtManagerEventController : BaseController
    {
        private readonly IEvtManagerEventService _evtEventServices;
        private readonly IEvtEventDetailServices _evtEventDetailServices;
        private readonly IEvtTicketTemplateService _evtTicketTemplateService;
        private readonly IEvtOrderTicketFillService _evtOrderTicketFillService;
        private readonly IEvtDeliveryTicketTemplateService _evtDeliveryTicketTemplateService;
        public EvtManagerEventController(
            ILogger<EvtManagerEventController> logger, 
            IEvtManagerEventService evtEventService,
            IEvtEventDetailServices evtEventDetailServices,
            IEvtTicketTemplateService evtTicketTemplateService,
            IEvtOrderTicketFillService evtOrderTicketFillService,
            IEvtDeliveryTicketTemplateService evtDeliveryTicketTemplateService)
        {
            _logger = logger;
            _evtEventServices = evtEventService;
            _evtEventDetailServices = evtEventDetailServices;
            _evtTicketTemplateService = evtTicketTemplateService;
            _evtOrderTicketFillService = evtOrderTicketFillService;
            _evtDeliveryTicketTemplateService = evtDeliveryTicketTemplateService;
        }

        #region Sự kiện
        /// <summary>
        /// Thêm sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateEvtEventDto input)
        {
            try
            {
               var result = _evtEventServices.Add(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] UpdateEvtEventDto input)
        {
            try
            {
                var result = _evtEventServices.Update(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Delete(int id)
        {
            try
            {
                _evtEventServices.Delete(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtEventDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterEvtEventDto input)
        {
            try
            {
                var result = _evtEventServices.FindAll(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuyển sự kiện sang trạng thái mở bán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("open-event/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse OpenEvent(int id)
        {
            try
            {
                _evtEventServices.UpdateStatus(id, EventStatus.DANG_MO_BAN );
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuyển sự kiến trang trại thái hủy sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("cancel-event")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse CancelEvent(CreateHistoryUpdateDto input)
        {
            try
            {
                _evtEventServices.CancelEvent(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuyển sự kiến trang trại thái hủy sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("pause-event")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> PauseEvent(CreateHistoryUpdateDto input)
        {
            try
            {
                await _evtEventServices.PauseEvent(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuyển strạng thái showApp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-show-app/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeShowApp(int id)
        {
            try
            {
                _evtEventServices.ChangeIsShowApp(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật nội dung tổng quan
        /// </summary>
        [HttpPut("update-overview-content")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateEventOverviewContent([FromBody] UpdateEventOverviewContentDto input)
        {
            try
            {
                _evtEventServices.UpdateEventOverviewContent(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtEventDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _evtEventServices.FindById(id);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get sự kiện ở trạng thái mở bán (get cho đặt lệnh)
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-active")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtEventDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindEventActive()
        {
            try
            {
                var result = _evtEventServices.FindEventActive();
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuyển strạng thái isCheck
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-check/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeIsCheck(int id)
        {
            try
            {
                _evtEventServices.ChangeIsCheck(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách event theo trading (cả bán hộ)
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-event-sell-behalf")]
        public APIResponse FindAllDistributionTrading()
        {
            try
            {
                var result = _evtEventServices.FindEventSellBehalfByTrading();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Khung giờ

        /// <summary>
        /// Thêm khung giờ sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("event-detail/add")]
        [ProducesResponseType(typeof(APIResponse<EvtEventDetailDto>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateEvtEventDetailDto input)
        {
            try
            {
                var result = _evtEventDetailServices.AddEventDetail(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật khung giờ sự kịện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("event-detail/update")]
        [ProducesResponseType(typeof(APIResponse<EvtEventDetailDto>), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] UpdateEvtEventDetailDto input)
        {
            try
            {
                var result = _evtEventDetailServices.UpdateEventDetail(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa khung giờ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("event-detail/delete/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse DeleteEventDetail(int id)
        {
            try
            {
                _evtEventDetailServices.DeleteEventDetail(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách chi tiết sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("event-detail/find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtEventDetailDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterEvtEventDetailDto input)
        {
            try
            {
                var result = _evtEventDetailServices.FindAllEventDetail(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuyển trạng thái chi tiết sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("event-detail/change-status/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeShowAppEventDetail(int id)
        {
            try
            {
                _evtEventDetailServices.ChangeStatusEventDetail(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get khung giờ sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("event-detail/find-by-id/{id}")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtEventDetailDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindDetailById(int id)
        {
            try
            {
                var result = _evtEventDetailServices.FindDetailById(id);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get khung giờ sự kiện và các vé ở trạng thái kích hoạt (get cho đặt lệnh)
        /// </summary>
        /// <param name="id">Id sự kiện</param>
        /// <returns></returns>
        [HttpGet("event-detail/find-active/{id}")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<EvtEventDetailDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindDetailActiveById(int id)
        {
            try
            {
                var result = _evtEventDetailServices.FindDetailActiveById(id);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region mẫu vé
        /// <summary>
        /// Thêm mẫu vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("ticket-template")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse AddTicketTemplate([FromBody] CreateEvtTicketTemplateDto input)
        {
            try
            {
                var result = _evtTicketTemplateService.Add(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// sửa mẫu vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("ticket-template")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateTicketTemplate([FromBody] UpdateEvtTicketTemplateDto input)
        {
            try
            {
                var result = _evtTicketTemplateService.Update(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sách mẫu vé của từng event
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("ticket-template")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ResponseEvtTicketTemplateDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllTicketTemplate([FromQuery] FilterEvtTicketTemplateDto input)
        {
            try
            {
                var result = _evtTicketTemplateService.FindAll(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// kích hoạt và hủy mẫu vé
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("ticket-template/change-status")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeStatusTicketTemplate(int id)
        {
            try
            {
                _evtTicketTemplateService.ChangeStatus(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tải file mẫu vé sự kiện
        /// </summary>
        /// <param name="ticketTemplateId">ID mẫu vé</param>
        /// <returns></returns>
        [HttpGet("file-ticket-template-pdf/{ticketTemplateId}")]
        public async Task<IActionResult> ExportTicketTemplatePdf(int ticketTemplateId)
        {
            try
            {
                var result = await _evtOrderTicketFillService.FillTemplateTicketPdf(ticketTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
        /// <summary>
        /// Tải file mẫu vé sự kiện
        /// </summary>
        /// <param name="ticketTemplateId">ID mẫu vé</param>
        /// <returns></returns>
        [HttpGet("file-ticket-template-word/{ticketTemplateId}")]
        public IActionResult ExportTicketTemplateWord(int ticketTemplateId)
        {
            try
            {
                var result = _evtOrderTicketFillService.FillTemplateTicketWord(ticketTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
        #endregion

        #region Mẫu giao nhận vé
        /// <summary>
        /// Thêm mẫu giao nhận vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("delivery-ticket-template/add")]
        [ProducesResponseType(typeof(APIResponse<EvtDeliveryTicketTemplateDto>), (int)HttpStatusCode.OK)]
        public APIResponse AddDeliveryTicketTemplate([FromBody] CreateDeliveryTicketTemplateDto input)
        {
            try
            {
                var result = _evtDeliveryTicketTemplateService.Add(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// sửa mẫu giao nhận vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("delivery-ticket-template/update")]
        [ProducesResponseType(typeof(APIResponse<EvtDeliveryTicketTemplateDto>), (int)HttpStatusCode.OK)]
        public APIResponse UpdateDeliveryTicketTemplate([FromBody] UpdateDeliveryTicketTemplateDto input)
        {
            try
            {
                var result = _evtDeliveryTicketTemplateService.Update(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sách mẫu giao nhận vé của từng event
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("delivery-ticket-template/find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<EvtDeliveryTicketTemplateDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllDeliveryTicketTemplate([FromQuery] FilterDeliveryTicketTemplateDto input)
        {
            try
            {
                var result = _evtDeliveryTicketTemplateService.FindAll(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// kích hoạt và hủy mẫu vé
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delivery-ticket-template/change-status/{id}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeStatusDeliveryTicketTemplate(int id)
        {
            try
            {
                _evtDeliveryTicketTemplateService.ChangeStatus(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Mẫu giao nhận vé theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("delivery-ticket-template/find/{id}")]
        [ProducesResponseType(typeof(APIResponse<EvtDeliveryTicketTemplateDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindDeliveryTicketTemplate(int id)
        {
            try
            {
                var result = _evtDeliveryTicketTemplateService.FindById(id);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tải file mẫu giao nhận vé (PDF)
        /// </summary>
        /// <param name="deliveryTicketTemplateId">ID mẫu giao nhận vé</param>
        /// <returns></returns>
        [HttpGet("delivery-ticket-template/file-pdf/{deliveryTicketTemplateId}")]
        public async Task<IActionResult> ExportDeliveryTicketTemplatePdf(int deliveryTicketTemplateId)
        {
            try
            {
                var result = await _evtDeliveryTicketTemplateService.FillDeliveryTemplateTicketPdf(deliveryTicketTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Tải file mẫu giao nhận vé (WORD)
        /// </summary>
        /// <param name="deliveryTicketTemplateId">ID mẫu giao nhận vé</param>
        /// <returns></returns>
        [HttpGet("delivery-ticket-template/file-word/{deliveryTicketTemplateId}")]
        public IActionResult ExportDeliveryTicketTemplateWord(int deliveryTicketTemplateId)
        {
            try
            {
                var result = _evtDeliveryTicketTemplateService.FillDeliveryTemplateTicketWord(deliveryTicketTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
        #endregion

    }
}
