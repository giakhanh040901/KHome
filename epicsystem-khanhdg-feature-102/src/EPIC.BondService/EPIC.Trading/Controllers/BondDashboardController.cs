using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.BondDashboard;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.DepositProvider;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Controllers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/dashboard")]
    [ApiController]
    public class BondDashboardController : BaseController
    {
        private readonly IBondDashboardService _dashboardServices;

        public BondDashboardController(IBondDashboardService dashboardServices)
        {
            _dashboardServices = dashboardServices;
        }

        /// <summary>
        /// Lấy thông số dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public APIResponse GetDashboard([FromQuery] GetBondDashboardDto dto)
        {
            try
            {
                var result = _dashboardServices.GetBondDashboard(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        } 
    }
}
