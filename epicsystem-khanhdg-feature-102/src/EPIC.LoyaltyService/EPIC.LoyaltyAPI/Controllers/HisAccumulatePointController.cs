using EPIC.Utils.Controllers;
using EPIC.Utils.Filter;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using System.Net;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.DataAccess.Models;
using System.Threading.Tasks;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using System.Collections.Generic;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.WebAPIBase.FIlters;
using DocumentFormat.OpenXml.Office2010.Excel;
using Humanizer;
using EPIC.Entities.DataEntities;

namespace EPIC.LoyaltyAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/loyalty/accumulate-point")]
    [ApiController]
    public class HisAccumulatePointController : BaseController
    {
        private readonly IHisAccumulatePointServices _hisAccumulatePointServices;

        public HisAccumulatePointController(ILogger<HisAccumulatePointController> logger,
            IHisAccumulatePointServices hisAccumulatePointServices)
        {
            _logger = logger;
            _hisAccumulatePointServices = hisAccumulatePointServices;
        }

        /// <summary>
        /// Thêm tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<APIResponse> AddAccumulatePoint([FromBody] AddAccumulatePointDto dto)
        {
            try
            {
                await _hisAccumulatePointServices.Add(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Import excel các lệnh tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("import-excel")]
        public async Task<APIResponse> AddAccumulatePoint([FromForm] ImportExceAccumulatePointlDto dto)
        {
            try
            {
                await _hisAccumulatePointServices.ImportExcel(dto);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("")]
        public APIResponse UpdateAccumulatePoint([FromBody] UpdateAccumulatePointDto dto)
        {
            try
            {
                _hisAccumulatePointServices.Update(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái yêu cầu đổi điểm sang Tiếp nhận (màn hình Quản lý yêu cầu)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("exchanged-point-status/pending")]
        public APIResponse UpdateStatusPending(UpdateHisAccumulateStatusDto dto)
        {
            try
            {
                _hisAccumulatePointServices.UpdateExchangedPointStatus(dto, LoyExchangePointStatus.PENDING);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái yêu cầu đổi điểm sang Đang giao (màn hình Quản lý yêu cầu)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("exchanged-point-status/delivery")]
        public APIResponse UpdateStatusDelivery(UpdateHisAccumulateStatusDto dto)
        {
            try
            {
                _hisAccumulatePointServices.UpdateExchangedPointStatus(dto, LoyExchangePointStatus.DELIVERY);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái yêu cầu đổi điểm sang Hoàn thành (màn hình Quản lý yêu cầu)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("exchanged-point-status/finished")]
        public APIResponse UpdateStatusFinished(UpdateHisAccumulateStatusDto dto)
        {
            try
            {
                _hisAccumulatePointServices.UpdateExchangedPointStatus(dto, LoyExchangePointStatus.FINISHED);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái yêu cầu đổi điểm sang Hủy (màn hình Quản lý yêu cầu)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("exchanged-point-status/cancel")]
        public APIResponse UpdateStatusCanceled(UpdateHisAccumulateStatusDto dto)
        {
            try
            {
                _hisAccumulatePointServices.UpdateExchangedPointStatus(dto, LoyExchangePointStatus.CANCELED);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy lệnh tích điểm/tiêu điểm (màn hình Quản lý tích điểm)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("status/cancel")]
        public APIResponse UpdateStatusCanceled(UpdateStatusCancelDto dto)
        {
            try
            {
                _hisAccumulatePointServices.Cancel(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa tích điểm/tiêu điểm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                _hisAccumulatePointServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("find")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewFindAllHisAccumulatePointDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FindHisAccumulatePointDto dto)
        {
            try
            {
                var result = _hisAccumulatePointServices.FindAll(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm phân trang yêu cầu đổi điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("find/request-consume")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewFindAllHisAccumulatePointDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllRequestConsume([FromQuery] FindRequestConsumePointDto dto)
        {
            try
            {
                var result = _hisAccumulatePointServices.FindAll(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm lịch sử yêu cầu tiêu điểm/tích điểm theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("investor/{investorId}/find")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewFindAllHisAccumulatePointDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllRequestConsume(int investorId, [FromQuery] FindAccumulatePointByInvestorId dto)
        {
            try
            {
                var result = _hisAccumulatePointServices.FindByInvestorId(investorId, dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Lấy tích điểm/tiêu điểm theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<ViewHisAccumulatePointDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById([FromRoute] int id)
        {
            try
            {
                var result = _hisAccumulatePointServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy list lý do tích điểm/tiêu điểm
        /// </summary>
        /// <returns></returns>
        [HttpGet("reasons")]
        [ProducesResponseType(typeof(APIResponse<List<ViewListReasonsDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetReasonsByPointType()
        {
            try
            {
                var result = _hisAccumulatePointServices.GetAccumulateReason(null);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get list voucher đang kích hoạt và chưa cấp phát cho ai
        /// </summary>
        /// <returns></returns>
        [HttpGet("free-voucher")]
        [ProducesResponseType(typeof(APIResponse<List<ViewListVoucherDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetFreeVoucher()
        {
            try
            {
                var result = _hisAccumulatePointServices.FindFreeVoucher();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
