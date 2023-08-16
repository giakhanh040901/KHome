using EPIC.EntitiesBase.Dto;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.PaymentDomain.Interfaces;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.PaymentAPI.Controllers.AppController
{
    /// <summary>
    /// Api app msb
    /// </summary>
    [Route("api/payment/msb")]
    [ApiController]
    public class AppMsbController : BaseController
    {
        private readonly IMsbPaymentServices _msbPaymentService;

        public AppMsbController(ILogger<MsbPaymentController> logger, IMsbPaymentServices msbPaymentService)
        {
            _logger = logger;
            _msbPaymentService = msbPaymentService;
        }

        /// <summary>
        /// Truy vấn tài khoản ngân hàng qua msb
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        [HttpGet("inquiry-bank-account")]
        [ProducesResponseType(typeof(APIResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> InquiryBankAccount(int bankId, string bankAccount)
        {
            try
            {
                var result = await _msbPaymentService.InquiryBankAccount(bankId, bankAccount);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Truy vấn investor bank account qua msb, nếu tài khoản lỗi xuất ra file excel
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-investor-bank-account")]
        [ProducesResponseType(typeof(APIResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllInvestorBankAccountList([FromQuery] FilterInvestorBankAccountDto input)
        {
            try
            {
                var result = await _msbPaymentService.GetAllInvestorBankAccountList(input);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "DanhSachInvestorBankAccountLoi.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}
