using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.Shared.Filter;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/approve")]
    [ApiController]
    public class RstApproveController : BaseController
    {
        private readonly IRstApproveServices _rstApproveServices;

        public RstApproveController(ILogger<RstApproveController> logger,
            IRstApproveServices rstApproveServices)
        {
            _logger = logger;
            _rstApproveServices = rstApproveServices;
        }

        /// <summary>
        /// lấy danh sách duyệt
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStatePDDA_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstApproveDto input)
        {
            try
            {
                var result = _rstApproveServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
