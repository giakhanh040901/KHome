using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.Dto.CompanySharesInfoTradingProvider;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EPIC.CompanySharesAPI.Controllers
{
    [Authorize]
    [Route("api/company-shares/cps-trading-provider")]
    [ApiController]
    public class CompanySharesInfoTradingProviderController : BaseController
    {
        private readonly ICpsInfoTradingProviderServices _cpsInfoTradingProviderServices;

        public CompanySharesInfoTradingProviderController(ILogger<CompanySharesInfoTradingProviderController> logger, 
            ICpsInfoTradingProviderServices cpsInfoTradingProviderServices)
        {
            _logger = logger;
            _cpsInfoTradingProviderServices = cpsInfoTradingProviderServices;
        }

        /// <summary>
        /// Thêm đại lý vào cổ phần
        /// </summary>
        /// <param name="cpsInfoId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse UpdateProjectTrading(int cpsInfoId, List<CreateCpsInfoTradingProviderDto> input)
        {
            try
            {
                _cpsInfoTradingProviderServices.UpdateProjectTrading(cpsInfoId, input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin đại lý thuộc cổ phần
        /// </summary>
        /// <param name="cpsInfoId"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse FindAll(int cpsInfoId)
        {
            try
            {
                var result = _cpsInfoTradingProviderServices.FindAll(cpsInfoId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
