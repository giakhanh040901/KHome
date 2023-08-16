using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using System.Net;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.DataAccess.Models;
using System.Collections.Generic;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using System.ComponentModel.DataAnnotations;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.LoyaltyAPI.Controllers
{
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Authorize]
    [Route("api/loyalty/app/voucher")]
    [ApiController]
    public class AppLoyVoucherController : BaseController
    {
        private readonly ILoyVoucherServices _loyVoucherServices;

        public AppLoyVoucherController(ILogger<AppLoyVoucherController> logger,
            ILoyVoucherServices loyVoucherServices)
        {
            _logger = logger;
            _loyVoucherServices = loyVoucherServices;
        }

        /// <summary>
        /// Tab ưu đãi (Lấy list voucher theo khách đăng nhập)
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="useType">Loại voucher (TD: Tiêu dùng; MS: Mua sắm; AT: Ẩm thực; DV: Dịch vụ)</param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(APIResponse<AppViewVoucherByInvestorDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindVoucher(int? tradingProviderId, string useType)
        {
            try
            {
                var result = _loyVoucherServices.AppFindByInvestor(tradingProviderId, useType);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách 6 voucher nổi bật của đại lý
        /// </summary>
        [HttpGet("hot")]
        [ProducesResponseType(typeof(APIResponse<AppViewVoucherByInvestorDto>), (int)HttpStatusCode.OK)]
        public APIResponse AppFindVoucherIsHot(int tradingProviderId)
        {
            try
            {
                var result = _loyVoucherServices.AppFindVoucherIsHot(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Loại voucher đại lý có
        /// </summary>
        [HttpGet("use-type")]
        public APIResponse AppGetVoucherUseTypeOfTrading(int tradingProviderId)
        {
            try
            {
                var result = _loyVoucherServices.AppGetVoucherUseTypeOfTrading(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// App Lấy thông tin voucher
        /// </summary>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        [HttpGet("{voucherId}")]
        [ProducesResponseType(typeof(APIResponse<ViewVoucherDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById([FromRoute] int voucherId)
        {
            try
            {
                var result = _loyVoucherServices.FindById(voucherId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// App xem tab lịch sử voucher theo investor
        /// </summary>
        /// <returns></returns>
        [HttpGet("history")]
        [ProducesResponseType(typeof(APIResponse<AppViewVoucherExpiredByInvestorDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindVoucherExpired()
        {
            try
            {
                var result = _loyVoucherServices.AppFindExpiredByInvestor();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
