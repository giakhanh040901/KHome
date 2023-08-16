using EPIC.CompanySharesDomain.Interfaces;
using EPIC.FileEntities.Settings;
using EPIC.Notification.Services;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using EPIC.CompanySharesEntities.Dto.OrderPayment;
using EPIC.Utils.Validation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EPIC.CompanySharesAPI.Controllers
{
    [Authorize]
    [Route("api/company-shares/order-payment")]
    [ApiController]
    public class OrderPaymentController : BaseController
    {
        private readonly ICpsOrderPaymentServices _orderPaymentServices;
        private readonly NotificationServices _sendEmailServices;
        private readonly IConfiguration _configuration;
        private readonly IOptions<FileConfig> _fileConfig;

        public OrderPaymentController(
            ILogger<OrderPaymentController> logger,
            ICpsOrderPaymentServices orderPaymentServices,
            NotificationServices sendEmailServices,
            IConfiguration configuration,
            IOptions<FileConfig> fileConfig)
        {
            _logger = logger;
            _orderPaymentServices = orderPaymentServices;
            _sendEmailServices = sendEmailServices;
            _configuration = configuration;
            _fileConfig = fileConfig;
        }

        /// <summary>
        /// Thêm thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpPost("payment/add")]
        //[PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi)]
        public APIResponse OrderPaymentAdd(CreateOrderPaymentDto input)
        {
            try
            {
                var result = _orderPaymentServices.AddPayment(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
        [HttpPut("payment/approve/{orderPaymentId}")]
        //[PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet, Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet)]
        public async Task<APIResponse> OrderPaymentApprove(int orderPaymentId,
            [IntegerRange(AllowableValues = new int[] { OrderPaymentStatus.DA_THANH_TOAN, OrderPaymentStatus.HUY_THANH_TOAN })] int status)
        {
            try
            {
                var result = _orderPaymentServices.ApprovePayment(orderPaymentId, status);

                //gửi email thông báo chuyển tiền thành công, nếu chuyển đủ tiền thì sẽ gửi email đầu tư thành công
                if (status == OrderPaymentStatus.DA_THANH_TOAN)
                {
                    await _sendEmailServices.SendEmailBondApprovePayment(orderPaymentId);
                }
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        ///  Xóa thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("payment/delete/{id}")]
        [HttpDelete]
        //[PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_Xoa)]
        public APIResponse DeleteOrderPayment(int id)
        {
            try
            {
                var result = _orderPaymentServices.DeleteOrderPayment(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách thanh toán
        /// </summary>
        /// <returns></returns>
        [Route("payment/find")]
        [HttpGet]
        //[PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_DanhSach)]
        public APIResponse FindAllOrderPayment(int orderId, int pageNumber, int? pageSize, string keyword, int? status)
        {
            try
            {
                var result = _orderPaymentServices.FindAll(orderId, pageSize ?? 100, pageNumber, keyword?.Trim(), status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm thanh toán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("payment/find/{id}")]
        [HttpGet]
        public APIResponse GetOrderPayment(int id)
        {
            try
            {
                var result = _orderPaymentServices.FindPaymentById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
