using EPIC.PaymentDomain.Implements;
using EPIC.PaymentDomain.Interfaces;
using EPIC.PaymentEntities.Dto;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.PaymentAPI.Controllers
{
    /// <summary>
    /// xem thông báo thanh toán từ ngân hàng
    /// </summary>
    [Authorize]
    [Route("api/payment/pvcb")]
    [ApiController]
    public class PvcbPaymentController : BaseController
    {
        private readonly IPvcbPaymentServices _pvcbCallbackServices;

        public PvcbPaymentController(ILogger<PvcbPaymentController> logger, IPvcbPaymentServices callbackServices)
        {
            _logger = logger;
            _pvcbCallbackServices = callbackServices;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll(int? pageSize, int pageNumber, string keyword, string status)
        {
            try
            {
                var result = _pvcbCallbackServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), status?.Trim());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return OkExceptionStatusCode(ex);
            }
        }
    }
}
