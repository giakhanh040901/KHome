using EPIC.FileEntities.Settings;
using EPIC.InvestDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.InvestAPI.Controllers
{ 
    [ApiController]
    [Route("api/invest/receive-contract")]
    public class ReceiveContractController : BaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly IConfiguration _configuration;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly NotificationServices _sendEmailServices;

        public ReceiveContractController(ILogger<ReceiveContractController> logger,
            IOrderServices orderServices,
            IConfiguration configuration,
            IOptions<FileConfig> fileConfig,
            NotificationServices sendEmailServices)
        {
            _logger = logger;
            _orderServices = orderServices;
            _configuration = configuration;
            _fileConfig = fileConfig;
            _sendEmailServices = sendEmailServices;
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
        public async Task<APIResponse> ChangeDeliveryStatusRecevired(string deliveryCode, string otp)
        {
            try
            {
                var result = await _orderServices.ChangeDeliveryStatusRecevired(deliveryCode, otp);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
                _orderServices.VerifyPhone(deliveryCode, phone, tradingProviderId);
                await _sendEmailServices.SendEmailOtpByPhone(phone, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        [Route("get-phone-by-delivery-code/{deliveryCode}")]
        [AllowAnonymous]
        public APIResponse GetPhoneByDeliveryCode(string deliveryCode)
        {
            try
            {
                var result = _orderServices.GetPhoneByDeliveryCode(deliveryCode);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
