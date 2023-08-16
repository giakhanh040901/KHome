using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerInterestPayment;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System;
using EPIC.GarnerEntities.Dto.GarnerDashboard;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/dashboard")]
    [ApiController]
    public class GarnerDashboardController : BaseController
    {
        private readonly IGarnerDashboardServices _garnerDashboardServices;

        public GarnerDashboardController(ILogger<GarnerDashboardController> logger,
            IGarnerDashboardServices garnerDashboardServices)
        {
            _logger = logger;
            _garnerDashboardServices = garnerDashboardServices;
        }

        /// <summary>
        /// Dashboard tích lũy
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(APIResponse<GarnerDashboardDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] GarnerDashboardFindDto dto)
        {
            try
            {
                var result = _garnerDashboardServices.Dashboard(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy list sp theo đại lý (picklist)
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet("products")]
        [ProducesResponseType(typeof(APIResponse<GarnerDashboardProductPickListDto>), (int)HttpStatusCode.OK)]
        public APIResponse GetProductPickList(int? tradingProviderId)
        {
            try
            {
                var result = _garnerDashboardServices.GetPicklistProductByTrading(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
