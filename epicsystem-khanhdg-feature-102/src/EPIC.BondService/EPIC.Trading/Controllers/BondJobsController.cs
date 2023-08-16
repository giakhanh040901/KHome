using EPIC.BondDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.BondAPI.Controllers
{
    /// <summary>
    /// Api chạy job quét qua toàn bộ bond xem có gì cần xử lý
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/jobs")]
    [ApiController]
    public class BondJobsController : BaseController
    {
        private readonly IBondJobsService _bondJobsServices;
        public BondJobsController(ILogger<BondJobsController> logger, IBondJobsService bondJobsServices)
        {
            _logger = logger;
            _bondJobsServices = bondJobsServices;
        }

        /// <summary>
        /// dò theo sổ lệnh
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpPost("order/{tradingProviderId}")]
        public APIResponse GetContractType(int tradingProviderId)
        {
            try
            {
                _bondJobsServices.ScanOrder(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
