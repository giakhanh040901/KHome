using EPIC.PaymentDomain.Interfaces;
using EPIC.PaymentEntities.Dto.Pvcb;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.PaymentAPI.Controllers
{
    /// <summary>
    /// Api nhận các callback thanh toán từ ngân hàng pvcombank
    /// </summary>
    [Route("callback/pvcb")]
    [ApiController]
    public class PvcbCallbackController : BaseController
    {
        private readonly IPvcbPaymentServices _callbackServices;

        public PvcbCallbackController(ILogger<MsbPaymentController> logger, IPvcbPaymentServices callbackServices)
        {
            _logger = logger;
            _callbackServices = callbackServices;
        }

        /// <summary>
        /// Nhận callback thanh toán từ pvcombank
        /// </summary>
        /// <param name="input"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("{token}/transfer")]
        [WhiteListIpFilter(WhiteListIpTypes.InvestDuyetHopDong)]
        public IActionResult PvcbCallbackAdd(string token, [FromBody] PvcbCallbackRequestDto input)
        {
            try
            {
                _callbackServices.VerifyToken(input.Data, input.Signature);
                _callbackServices.PvcbCallbackAdd(input.Data, token);
                return Ok();
            }
            catch (Exception ex)
            {
                return OkExceptionStatusCode(ex);
            }
        }
    }
}
