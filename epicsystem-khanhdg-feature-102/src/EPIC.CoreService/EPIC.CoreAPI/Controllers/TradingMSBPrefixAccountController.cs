using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.TradingMSBPrefixAccount;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CoreAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/trading-provider/msb-prefix-account")]
    [ApiController]
    public class TradingMsbPrefixAccountController : BaseController
    {
        private readonly ITradingMsbPrefixAccountServices _tradingMsbPrefixAccountServices;

        public TradingMsbPrefixAccountController(ILogger<TradingMsbPrefixAccountController> logger,
            ITradingMsbPrefixAccountServices tradingMSBPrefixAccountServices)
        {
            _logger = logger;
            _tradingMsbPrefixAccountServices = tradingMSBPrefixAccountServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse FindAll([FromQuery] FilterTradingMsbPrefixAccountDto input)
        {
            try
            {
                var result = _tradingMsbPrefixAccountServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _tradingMsbPrefixAccountServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin bank theo bank account id của đại lý
        /// </summary>
        /// <param name="tradingBankAccId"></param>
        /// <returns></returns>
        [HttpGet("find-by-trading-bank-id/{tradingBankAccId}")]
        public APIResponse FindByTradingBankId(int tradingBankAccId)
        {
            try
            {
                var result = _tradingMsbPrefixAccountServices.FindByTradingBankId(tradingBankAccId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse Add([FromBody] CreateTradingMsbPrefixAccountDto input)
        {
            try
            {
                _tradingMsbPrefixAccountServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse Update([FromBody] UpdateTradingMsbPrefixAccountDto input)
        {
            try
            {
                _tradingMsbPrefixAccountServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa sản phẩm tích lũy
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                _tradingMsbPrefixAccountServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
