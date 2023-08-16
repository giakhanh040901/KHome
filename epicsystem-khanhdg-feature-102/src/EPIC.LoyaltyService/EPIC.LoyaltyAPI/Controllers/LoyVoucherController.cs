using EPIC.DataAccess.Models;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.Utils.Net.MimeTypes;
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
    [Route("api/loyalty/voucher")]
    [ApiController]
    public class LoyVoucherController : BaseController
    {
        private readonly ILoyVoucherServices _loyVoucherServices;

        public LoyVoucherController(ILogger<LoyVoucherController> logger,
            ILoyVoucherServices loyVoucherServices)
        {
            _logger = logger;
            _loyVoucherServices = loyVoucherServices;
        }

        /// <summary>
        /// Thêm mới voucher
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(APIResponse<ViewVoucherDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> AddVoucher([FromBody] AddVoucherDto dto)
        {
            try
            {
                var result = await _loyVoucherServices.Add(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm voucher bằng excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<APIResponse> AddVoucher([FromForm] ImportExcelVoucherDto dto)
        {
            try
            {
                await _loyVoucherServices.ImportExcelVoucher(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Giao voucher cho khách
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("investor")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<APIResponse> UpdateStatus([FromBody] ApplyVoucherToInvestorDto dto)
        {
            try
            {
                await _loyVoucherServices.ApplyVoucherToInvestor(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái của voucher với khách
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public APIResponse UpdateStatus([FromBody] UpdateVoucherStatusDto dto)
        {
            try
            {
                _loyVoucherServices.UpdateStatus(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật voucher
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<APIResponse> UpdateVoucher([FromBody] UpdateVoucherDto dto)
        {
            try
            {
                await _loyVoucherServices.Update(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật show app
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("is-show-app")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public APIResponse UpdateShowApp([FromBody] UpdateShowAppDto dto)
        {
            try
            {
                _loyVoucherServices.UpdateIsShowApp(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật nổi bật
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("is-hot")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public APIResponse UpdateIsHot([FromBody] UpdateIsHotDto dto)
        {
            try
            {
                _loyVoucherServices.UpdateIsHot(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật dùng trong vòng quay may mắn
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("is-in-lucky-draw")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public APIResponse UpdateIsInLuckyDraw([FromBody] UpdateIsUseInLuckyDrawDto dto)
        {
            try
            {
                _loyVoucherServices.UpdateIsUseInLuckyDraw(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Tìm kiếm voucher
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("find")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewListVoucherDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FindAllVoucherDto dto)
        {
            try
            {
                var result = _loyVoucherServices.FindAll(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lịch sử cấp phát
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("conversion-history")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewListVoucherDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllVoucherConversionHistory([FromQuery] FilterVoucherConversionHistoryDto dto)
        {
            try
            {
                var result = _loyVoucherServices.FindAllVoucherConversionHistory(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách voucher để đổi điểm Tab yêu cầu đổi điểm
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-for-conversion-point")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewListVoucherDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllVoucherForConversionPoint(string keyword)
        {
            try
            {
                var result = _loyVoucherServices.GetAllVoucherForConversionPoint(keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy voucher theo id
        /// </summary>
        /// <param name="voucherId"></param>
        /// <param name="voucherInvestorId"></param>
        /// <returns></returns>
        [HttpGet("{voucherId}")]
        [ProducesResponseType(typeof(APIResponse<ViewVoucherDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById([FromRoute] int voucherId)
        {
            try
            {
                var result = _loyVoucherServices.FindById(voucherId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm investor kèm theo thông tin voucher
        /// (Cho màn quản lý khách hàng)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("investors")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewInvestorVoucherDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllInvestor([FromQuery] FindAllInvestorForVoucherDto dto)
        {
            try
            {
                var result = _loyVoucherServices.FindAllInvestorVoucher(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xuất excel theo kq tìm kiếm khcn
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("investors/export-excel")]
        //[ProducesResponseType(typeof(APIResponse<PagingResult<ViewInvestorVoucherDto>>), (int)HttpStatusCode.OK)]
        public IActionResult ExportExcelInvestorVoucher([FromQuery] FindAllInvestorForVoucherDto dto)
        {
            try
            {
                var result = _loyVoucherServices.ExportExcelInvestorVoucher(dto);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "QuanLyKhachHangCaNhan.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Lấy danh sách voucher theo investor id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("investor")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewVoucherByInvestorDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindVoucherByInvestorId([FromQuery] FindVoucherByInvestorIdDto dto)
        {
            try
            {
                var result = _loyVoucherServices.FindByInvestorPaging(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa voucher khi chưa gán cho khách nào
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public APIResponse DeleteVoucher([FromRoute] int id)
        {
            try
            {
                _loyVoucherServices.DeleteById(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
