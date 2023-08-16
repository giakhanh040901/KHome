using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerDashboard;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using EPIC.RealEstateEntities.Dto.RstDashboard;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/dashboard")]
    [ApiController]
    public class RstDashboardController : BaseController
    {
        private readonly IRstDashboardServices _rstDashboardServices;

        public RstDashboardController(ILogger<RstDashboardController> logger,
            IRstDashboardServices rstDashboardServices)
        {
            _logger = logger;
            _rstDashboardServices = rstDashboardServices;
        }

        /// <summary>
        /// dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStatePageDashboard)]
        [ProducesResponseType(typeof(APIResponse<RstDashboardDto>), (int)HttpStatusCode.OK)]
        public APIResponse Dasboard([FromQuery] GetRstDashboardDto dto)
        {
            try
            {
                var result = _rstDashboardServices.Dasboard(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// dashboard
        /// </summary>
        /// <returns></returns>
        [HttpGet("list-project")]
        [PermissionFilter(Permissions.RealStatePageDashboard)]
        [ProducesResponseType(typeof(APIResponse<RstListProjectDashboardDto>), (int)HttpStatusCode.OK)]
        public APIResponse Dasboard()
        {
            try
            {
                var result = _rstDashboardServices.GetListProject();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
