using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstOrderPayment;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/order-payment")]
    [ApiController]
    public class RstOrderPaymentController : BaseController
    {
        private readonly IRstOrderPaymentServices _rstOrderPaymentServices;
        public RstOrderPaymentController(ILogger<RstOrderPaymentController> logger,
            IRstOrderPaymentServices rstOrderPaymentServices)
        {
            _logger = logger;
            _rstOrderPaymentServices = rstOrderPaymentServices;
        }

        /// <summary>
        /// lấy danh sách thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_ThongTinThanhToan_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstOrderPaymentDto input)
        {
            try
            {
                var result = _rstOrderPaymentServices.FindAll(input);
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
                var result = _rstOrderPaymentServices.FindById(id);
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
        [PermissionFilter(Permissions.RealStateMenuSoLenh_ThongTinThanhToan_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstOrderPaymentDto input)
        {
            try
            {
                var result = _rstOrderPaymentServices.Add(input);
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
        [PermissionFilter(Permissions.RealStateMenuSoLenh_ThongTinThanhToan_ChinhSua)]
        public APIResponse Update([FromBody] UpdateRstOrderPaymentDto input)
        {
            try
            {
                _rstOrderPaymentServices.Update(input);
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
        [PermissionFilter(Permissions.RealStateMenuSoLenh_ThongTinThanhToan_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstOrderPaymentServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt thanh toán
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        [HttpPut("approve/{orderPaymentId}")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_ThongTinThanhToan_PheDuyetOrHuy)]
        [WhiteListIpFilter(WhiteListIpTypes.RstDuyetVaoTien)]
        public async Task<APIResponse> Approve(int orderPaymentId)
        {
            try
            {
                await _rstOrderPaymentServices.ApproveOrCancel(orderPaymentId, OrderPaymentStatus.DA_THANH_TOAN);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt thanh toán
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        [HttpPut("cancel/{orderPaymentId}")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_ThongTinThanhToan_PheDuyetOrHuy)]
        public async Task<APIResponse> Cancel(int orderPaymentId)
        {
            try
            {
                await _rstOrderPaymentServices.ApproveOrCancel(orderPaymentId, OrderPaymentStatus.HUY_THANH_TOAN);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
