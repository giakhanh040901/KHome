using EPIC.MSB.Dto.Notification;
using EPIC.PaymentDomain.Interfaces;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.PaymentAPI.Controllers
{
    /// <summary>
    /// Notification của msb
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/payment/msb")]
    [ApiController]
    public class MsbPaymentController : BaseController
    {
        private readonly IMsbPaymentServices _msbPaymentService;

        public MsbPaymentController(ILogger<MsbPaymentController> logger, IMsbPaymentServices msbPaymentService)
        {
            _logger = logger;
            _msbPaymentService = msbPaymentService;
        }

        /// <summary>
        /// Nhận notification
        /// </summary>
        [AllowAnonymous]
        [HttpPost("receive-notification")]
        public async Task<APIResponse> ReceiveNotification([FromBody] ReceiveNotificationDto input)
        {
            try
            {
                await _msbPaymentService.ReceiveNotificationAsync(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Nhận notification chi hộ
        /// </summary>
        [AllowAnonymous]
        [HttpPost("receive-notification-payment")]
        public async Task<APIResponse> ReceiveNotificationPayment([FromBody] ReceiveNotificationPaymentDto input)
        {
            try
            {
                await _msbPaymentService.ReceiveNotificationPayment(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [AuthorizeAdminUserTypeFilter]
        [HttpGet("receive-notification/find-all")]
        public APIResponse FindAll([FromQuery] MsbFilterPaymentDto input)
        {
            try
            {
                var result = _msbPaymentService.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách notification thu hộ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("collection-payment/find-all")]
        [PermissionFilter(Permissions.Garner_TruyVan_ThuTien_NganHang, Permissions.Invest_TruyVan_ThuTien_NganHang)]
        public APIResponse FindAllCollectionPayment([FromQuery] MsbCollectionPaymentFilterDto input)
        {
            try
            {
                var result = _msbPaymentService.FindAllCollectionPayment(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
