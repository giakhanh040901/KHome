using EPIC.CoreDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CoreAPI.Controllers.AppController
{
    [Authorize]
    [Route("api/core/core-product-news")]
    [ApiController]
    public class AppCoreProductNewsController : BaseController
    {
        private readonly ICoreProductNewsServices _coreProductNewsServices;
        public AppCoreProductNewsController(ILogger<AppCoreProductNewsController> logger, ICoreProductNewsServices coreProdutcNewsServices)
        {
            _logger = logger;
            _coreProductNewsServices = coreProdutcNewsServices;
        }

        /// <summary>
        /// tìm tất cả tin tức
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpGet("find")]
        public APIResponse AppFindAll(int pageNumber, int? pageSize, string status, int location)
        {
            try
            {
                var result = _coreProductNewsServices.AppFindAll(pageSize ?? 100, pageNumber, status, location);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
