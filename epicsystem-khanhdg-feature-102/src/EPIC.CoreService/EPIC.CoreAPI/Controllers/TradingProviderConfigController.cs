using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.TradingProviderConfig;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EPIC.CoreAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/core/trading-provider-config")]
    public class TradingProviderConfigController : BaseController
    {
        private readonly ITradingProviderConfigServices _tradingProviderConfigServices;

        public TradingProviderConfigController(ILogger<TradingProviderConfigController> logger, ITradingProviderConfigServices tradingProviderConfigServices)
        {
            _logger = logger;
            _tradingProviderConfigServices = tradingProviderConfigServices;
        }

        /// <summary>
        /// Thêm config key
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse Add([FromBody] CreateTradingProviderConfigDto input)
        {
            try
            {
                _tradingProviderConfigServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm nhiều config cho đại lý
        /// </summary>
        [HttpPost("add-mutiple")]
        public APIResponse AddMutipleConfig([FromBody] List<CreateTradingProviderConfigDto> input)
        {
            try
            {
                _tradingProviderConfigServices.AddMutipleConfig(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update nhiều config cho đại lý
        /// </summary>
        [HttpPut("add-mutiple")]
        public APIResponse UpdateMutipleConfig([FromBody] List<CreateTradingProviderConfigDto> input)
        {
            try
            {
                _tradingProviderConfigServices.UpdateMutipleConfig(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật config key
        /// </summary>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse Update([FromBody] CreateTradingProviderConfigDto input)
        {
            try
            {
                _tradingProviderConfigServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách config key
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public APIResponse GetAll(string keyword)
        {
            try
            {
                var result = _tradingProviderConfigServices.GetAll(keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa config theo key
        /// </summary>
        /// <returns></returns>
        [HttpPut("delete/{key}")]
        public APIResponse Delete(string key)
        {
            try
            {
                _tradingProviderConfigServices.Delete(key?.Trim());
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
