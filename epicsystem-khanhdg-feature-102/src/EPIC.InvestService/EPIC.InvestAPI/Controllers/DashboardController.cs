using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using EPIC.Utils.Controllers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.Dashboard;
using EPIC.WebAPIBase.FIlters;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/dashboard")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IDashboardServices _dashboardServices;

        public DashboardController(IDashboardServices dashboardServices)
        {
            _dashboardServices = dashboardServices;
        }

        /// <summary>
        /// Lấy thông số dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// InvestPageDashboard
        [HttpGet]
        [Authorize]
        [PermissionFilter(Permissions.InvestPageDashboard)]
        public APIResponse GetDashboard([FromQuery] GetInvDashboardDto dto)
        {
            try
            {
                var result = _dashboardServices.GetInvDashboard(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sản phẩm của đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet("distribution")]
        [Authorize]
        public APIResponse GetPicklistDistributionByTrading(int? tradingProviderId)
        {
            try
            {
                var result = _dashboardServices.GetPicklistDistributionByTrading(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
