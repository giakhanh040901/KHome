using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerOrderPayment;
using EPIC.Notification.Services;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.Utils.Validation;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/order-payment")]
    [ApiController]
    public class GarnerOrderPaymentController : BaseController
    {
        private readonly IGarnerOrderPaymentServices _garnerOrderPaymentServices;
        private readonly GarnerNotificationServices _garnerNotificationServices;

        public GarnerOrderPaymentController(ILogger<GarnerOrderPaymentController> logger,
            IGarnerOrderPaymentServices garnerOrderPaymentServices, GarnerNotificationServices garnerNotificationServices)
        {
            _logger = logger;
            _garnerOrderPaymentServices = garnerOrderPaymentServices;
            _garnerNotificationServices = garnerNotificationServices;
        }

        /// <summary>
        /// lấy danh sách thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_DanhSach, Permissions.GarnerHDPP_XLRutTien_ThongTinChiTiet, Permissions.GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet, Permissions.GarnerHDPP_LSTL_ThongTinChiTiet)]
        //[ProducesResponseType(typeof(APIResponse<List<AppGarnerDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterOrderPaymentDto input)
        {
            try
            {
                var result = _garnerOrderPaymentServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm thông tin thanh toán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _garnerOrderPaymentServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<GarnerOrderPaymentDto>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateGarnerOrderPaymentDto input)
        {
            try
            {
                var result = _garnerOrderPaymentServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_CapNhat)]

        public APIResponse Update([FromBody] UpdateGarnerOrderPaymentDto input)
        {
            try
            {
                _garnerOrderPaymentServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_Xoa)]

        public APIResponse Delete(int id)
        {
            try
            {
                _garnerOrderPaymentServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt thanh toán, hủy thanh toán
        /// duyệt thanh toán nếu đủ tiền thì update trạng thái order thành chờ duyệt hợp đồng, nếu k đủ tiền thì về chờ thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpPut("approve/{orderPaymentId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet, Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet)]
        [WhiteListIpFilter(WhiteListIpTypes.GarnerDuyetVaoTien)]
        public async Task<APIResponse> OrderPaymentApprove(long orderPaymentId,
            [IntegerRange(AllowableValues = new int[] { OrderPaymentStatus.DA_THANH_TOAN, OrderPaymentStatus.HUY_THANH_TOAN })] int status)
        {
            try
            {
                await _garnerOrderPaymentServices.ApprovePayment(orderPaymentId, status);
                //gửi email thông báo chuyển tiền thành công, nếu chuyển đủ tiền thì sẽ gửi email đầu tư thành công
                //if (status == OrderPaymentStatus.DA_THANH_TOAN)
                //{
                //    await _sendEmailServices.SendEmailBondApprovePayment(orderPaymentId);
                //}
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gửi lại thông báo chuyển tiền thành công khi duyệt lệnh chuyển tiền
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        [HttpPost("resend-notify/{orderPaymentId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao)]

        public async Task<APIResponse> ResendNotify(long orderPaymentId)
        {
            try
            {
                await _garnerNotificationServices.SendNotifyGarnerApprovePayment(orderPaymentId);

                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
