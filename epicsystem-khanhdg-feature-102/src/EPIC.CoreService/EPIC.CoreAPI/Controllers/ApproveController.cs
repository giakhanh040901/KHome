using EPIC.CoreDomain.Interfaces;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.CoreAPI.Controllers
{
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/approve")]
    [ApiController]
    public class ApproveController : BaseController
    {
        private readonly IApproveServices _approveServices;

        public ApproveController(IApproveServices approveServices)
        {
            _approveServices = approveServices;
        }

        /// <summary>
        /// Find all approve
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        [PermissionFilter(Permissions.CoreKHCN_NDTCN, Permissions.CoreQLPD_KHCN_DanhSach, Permissions.CoreQLPD_KHDN_DanhSach, Permissions.CoreQLPD_NDTCN_DanhSach, Permissions.CoreQLPD_Sale_DanhSach, Permissions.BondQLPD_PDYCTT_DanhSach)]
        public APIResponse GetAll([FromQuery] GetApproveListDto dto)
        {
            try
            {
                var result = _approveServices.Find(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
