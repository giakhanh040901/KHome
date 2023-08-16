using EPIC.MSB.Dto.PayMoney;
using EPIC.PaymentDomain.Interfaces;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPIC.PaymentAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/payment/msb/request-pay")]
    [ApiController]
    public class MsbRequestPaymentController : BaseController
    {
        private readonly IMsbRequestPaymentServices _msbRequestPaymentServices;
        private readonly IMsbRequestDetailServices _msbRequestDetailServices;

        public MsbRequestPaymentController(ILogger<MsbRequestPaymentController> logger, IMsbRequestPaymentServices msbRequestPaymentServices, IMsbRequestDetailServices msbRequestDetailServices)
        {
            _logger = logger;
            _msbRequestPaymentServices = msbRequestPaymentServices;
            _msbRequestDetailServices = msbRequestDetailServices;
        }

        /// <summary>
        /// Tìm kiếm tất cả request
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.Garner_TruyVan_ChiTien_NganHang, Permissions.Invest_TruyVan_ChiTien_NganHang)]
        public APIResponse FindAll([FromQuery] FilterMsbRequestDetailDto input)
        {
            try
            {
                var result = _msbRequestPaymentServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy yêu cầu chi tiền 
        /// </summary>
        [HttpPut("cancel-request/{requestPaymentDetailId}")]
        public APIResponse CancelRequestPayment(long requestPaymentDetailId)
        {
            try
            {
                _msbRequestPaymentServices.CancelRequestPayment(requestPaymentDetailId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Truy vấn lô
        /// </summary>
        /// <returns></returns>
        [HttpGet("inquiry-batch/{requestId}")]
        [PermissionFilter(Permissions.Garner_TruyVan_ChiTien_NganHang, Permissions.Invest_TruyVan_ChiTien_NganHang)]
        public async Task<APIResponse> InquiryBatch(long requestId)
        {
            try
            {
                var result = await _msbRequestPaymentServices.InquiryBatch(requestId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("get-otp/{requestId}")]
        public async Task<APIResponse> SendOtp(long requestId, int tradingBankAccId)
        {
            try
            {
                await _msbRequestPaymentServices.SendOtp(requestId, tradingBankAccId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
