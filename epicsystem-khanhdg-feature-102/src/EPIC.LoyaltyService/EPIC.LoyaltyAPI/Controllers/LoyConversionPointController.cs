using EPIC.DataAccess.Models;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.ConversionPoint;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.LoyaltyAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/loyalty/conversion-point")]
    [ApiController]
    public class LoyConversionPointController : BaseController
    {
        private readonly ILoyConversionPointServices _loyConversionPointServices;

        public LoyConversionPointController(ILogger<HisAccumulatePointController> logger,
            ILoyConversionPointServices loyConversionPointServices)
        {
            _logger = logger;
            _loyConversionPointServices = loyConversionPointServices;
        }

        /// <summary>
        /// Thêm yêu cầu chuyển đổi
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse AddConversionPoint([FromBody] AddLoyConversionPointDto dto)
        {
            try
            {
                var result = _loyConversionPointServices.Add(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật yêu cầu duyển đổi
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse UpdateConversionPoint([FromBody] UpdateLoyConversionPointDto dto)
        {
            try
            {
                _loyConversionPointServices.Update(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái tiếp nhận yêu cầu
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update-status-pending")]
        public async Task<APIResponse> ChangeStatusPendingConversionPoint([FromBody] UpdateLoyConversionPointStatusDto dto)
        {
            try
            {
                await _loyConversionPointServices.ChangeStatusConversionPoint(dto, LoyConversionPointStatus.PENDING);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái đang giao
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update-status-delivery")]
        public async Task<APIResponse> ChangeStatusDeliveryConversionPoint([FromBody] UpdateLoyConversionPointStatusDto dto)
        {
            try
            {
                await _loyConversionPointServices.ChangeStatusConversionPoint(dto, LoyConversionPointStatus.DELIVERY);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái hoàn thành
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update-status-finished")]
        public async Task<APIResponse> ChangeStatusFinishedConversionPoint([FromBody] UpdateLoyConversionPointStatusDto dto)
        {
            try
            {
                await _loyConversionPointServices.ChangeStatusConversionPoint(dto, LoyConversionPointStatus.FINISHED);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái tiếp nhận yêu cầu
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update-status-cancel")]
        public APIResponse ChangeStatusCancel([FromBody] UpdateLoyConversionPointStatusDto dto)
        {
            try
            {
                _loyConversionPointServices.ChangeStatusCancel(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách chuyển đổi
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<FindAllLoyConversionPointDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllConversionPoint([FromQuery] FilterLoyConversionPointDto dto)
        {
            try
            {
                var result = _loyConversionPointServices.FindAll(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(APIResponse<LoyConversionPointDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindByIdConversionPoint(int id)
        {
            try
            {
                var result = _loyConversionPointServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
