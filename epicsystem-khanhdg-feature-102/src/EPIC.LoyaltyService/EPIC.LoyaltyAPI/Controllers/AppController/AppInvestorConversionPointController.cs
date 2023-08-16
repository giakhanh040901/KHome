using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.ConversionPoint;
using EPIC.LoyaltyEntities.Dto.LoyPointInvestor;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.LoyaltyAPI.Controllers.AppController
{
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Authorize]
    [Route("api/loyalty/app/investor-conversion-point")]
    [ApiController]
    public class AppInvestorConversionPointController : BaseController
    {
        private readonly ILoyConversionPointServices _loyConversionPointServices;

        public AppInvestorConversionPointController(ILogger<AppInvestorConversionPointController> logger,
            ILoyConversionPointServices loyConversionPointServices)
        {
            _logger = logger;
            _loyConversionPointServices = loyConversionPointServices;
        }

        /// <summary>
        /// Khách yêu cầu quy đổi voucher
        /// </summary>
        [HttpPost("add")]
        public async Task<APIResponse> AppConversionPointVoucher([FromBody] AppCreateConversionPointDto dto)
        {
            try
            {
                await _loyConversionPointServices.AppConversionPointVoucher(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách Ưu đãi voucher có thể sử dụng
        /// </summary>
        /// <returns></returns>
        [HttpGet("can-use")]
        [ProducesResponseType(typeof(APIResponse<List<AppLoyConversionPointByInvestorDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppInvestorVoucherCanUse(int tradingProviderId)
        {
            try
            {
                var result = _loyConversionPointServices.AppInvestorVoucherCanUse(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách Ưu đãi voucher có thể sử dụng Lấy 4 voucher mới nhất theo đại lý
        /// </summary>
        /// <returns></returns>
        [HttpGet("new-can-use")]
        [ProducesResponseType(typeof(APIResponse<List<AppLoyConversionPointByInvestorDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppInvestorVoucherNewCanUse(int tradingProviderId)
        {
            try
            {
                var result = _loyConversionPointServices.AppInvestorVoucherNewCanUse(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách Giao dịch voucher đã đổi
        /// </summary>
        /// <returns></returns>
        [HttpGet("transaction")]
        [ProducesResponseType(typeof(APIResponse<List<AppLoyConversionPointByInvestorDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppInvestorVoucherIsConversionPoint(int tradingProviderId)
        {
            try
            {
                var result = _loyConversionPointServices.AppInvestorVoucherIsConversionPoint(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách Voucher đã hết hạn sử dụng
        /// </summary>
        /// <returns></returns>
        [HttpGet("exprired")]
        [ProducesResponseType(typeof(APIResponse<List<AppLoyConversionPointByInvestorDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppInvestorVoucherIsExpired(int tradingProviderId)
        {
            try
            {
                var result = _loyConversionPointServices.AppInvestorVoucherIsExpired(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết voucher của nhà đầu tư
        /// </summary>
        /// <param name="conversionPointDetailId"></param>
        /// <returns></returns>
        [HttpGet("get-info/{conversionPointDetailId}")]
        [ProducesResponseType(typeof(APIResponse<AppLoyConversionPointByInvestorInfoDto>), (int)HttpStatusCode.OK)]
        public APIResponse AppInvestorVoucherInfo(int conversionPointDetailId)
        {
            try
            {
                var result = _loyConversionPointServices.AppInvestorVoucherInfo(conversionPointDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem lịch sử điểm thưởng của nhà đầu tư
        /// </summary>
        [HttpGet("history")]
        [ProducesResponseType(typeof(APIResponse<AppHistoryConversionPointDto>), (int)HttpStatusCode.OK)]
        public APIResponse AppHistoryConversionPoint(int tradingProviderId)
        {
            try
            {
                var result = _loyConversionPointServices.AppHistoryConversionPoint(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
