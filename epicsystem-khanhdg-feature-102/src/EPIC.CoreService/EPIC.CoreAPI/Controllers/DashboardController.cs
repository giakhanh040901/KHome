using EPIC.CoreDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstDashboard;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using EPIC.CoreEntities.Dto.Dashboard;

namespace EPIC.CoreAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/dashboard")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IDashboardServices _dashboardServices;

        public DashboardController(ILogger<DashboardController> logger,
            IDashboardServices dashboardServices)
        {
            _logger = logger;
            _dashboardServices = dashboardServices;
        }

        /// <summary>
        /// dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse Dasboard([FromQuery] FilterCoreDashboardDto dto)
        {
            try
            {
                var result = _dashboardServices.Dashboard(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách khách hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("find-all-customer")]
        public APIResponse FindAllCustomer([FromQuery] FilterCoreDashboardDto dto)
        {
            try
            {
                var result = _dashboardServices.CustomerList(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
