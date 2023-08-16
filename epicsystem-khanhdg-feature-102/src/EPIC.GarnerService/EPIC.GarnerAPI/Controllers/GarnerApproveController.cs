using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/approve")]
    [ApiController]
    public class GarnerApproveController : BaseController
    {
        private readonly IGarnerApproveServices _garnerApproveServices;

        public GarnerApproveController(ILogger<GarnerApproveController> logger,
            IGarnerApproveServices garnerApproveServices)
        {
            _logger = logger;
            _garnerApproveServices = garnerApproveServices;
        }

        /// <summary>
        /// lấy danh sách duyệt
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.GarnerPDSPTL_DanhSach, Permissions.GarnerPDPPDT_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterGarnerApproveDto input)
        {
            try
            {
                var result = _garnerApproveServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
