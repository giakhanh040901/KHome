using EPIC.BondDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.BondAPI.Controllers 
{

    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/receive-contract")]
    public class BondReceiveContractController : BaseController
    {
        private IBondOrderService _bondOrderServices;
        private NotificationServices _sendEmailServices;

        public BondReceiveContractController(ILogger<BondReceiveContractController> logger, IBondOrderService bondOrderServices, NotificationServices sendEmailServices)
        {
            _logger = logger;
            _bondOrderServices = bondOrderServices;
            _sendEmailServices = sendEmailServices;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("get-phone-by-delivery-code/{deliveryCode}")]
        public APIResponse GetPhoneByDeliveryCode(string deliveryCode)
        {
            try
            {
                var result = _bondOrderServices.GetPhoneByDeliveryCode(deliveryCode);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Verify phone
        /// </summary>
        /// <param name="deliveryCode"></param>
        /// <param name="phone"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [Route("verify-phone")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<APIResponse> VerifyPhone(string deliveryCode, string phone, int tradingProviderId)
        {
            try
            {
                _bondOrderServices.VerifyPhone(deliveryCode, phone, tradingProviderId);
                await _sendEmailServices.SendEmailOtpByPhone(phone, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Đổi trạng thái đang giao sang đã nhận CMS
        /// </summary>
        /// <param name="deliveryCode"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        [Route("delivery-status-recevired/{deliveryCode}/{otp}")]
        [HttpPut]
        [AllowAnonymous]
        public APIResponse ChangeDeliveryStatusRecevired(string deliveryCode, string otp)
        {
            try
            {
                var result = _bondOrderServices.ChangeDeliveryStatusRecevired(deliveryCode, otp);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
