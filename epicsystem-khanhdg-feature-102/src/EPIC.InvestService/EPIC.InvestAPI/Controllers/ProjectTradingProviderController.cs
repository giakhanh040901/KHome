using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.ProjectTradingProvider;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/project-trading-provider")]
    [ApiController]
    public class ProjectTradingProviderController : BaseController
    {
        private readonly IProjectTradingProviderServices _projectTradingProviderServices;

        public ProjectTradingProviderController(ILogger<ProjectTradingProviderController> logger, IProjectTradingProviderServices projectTradingProviderServices)
        {
            _logger = logger;
            _projectTradingProviderServices = projectTradingProviderServices;
        }

        /// <summary>
        /// Thêm đại lý vào dự án
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse UpdateProjectTrading(int projectId, List<CreateProjectTradingProviderDto> input)
        {
            try
            {
                _projectTradingProviderServices.UpdateProjectTrading(projectId, input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin đại lý thuộc dự án
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.InvestSPDT_ThongTinChung)]
        public APIResponse FindAll(int projectId)
        {
            try
            {
                var result =_projectTradingProviderServices.FindAll(projectId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
