using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.RenewalsRequest;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/renewals-request")]
    [ApiController]
    public class InvestRenewalsRequestController : BaseController
    {
        private readonly IInvestRenewalsRequestServices _renewalsRequestServices;

        public InvestRenewalsRequestController(ILogger<InvestApproveController> logger, IInvestRenewalsRequestServices renewalsRequestServices)
        {
            _logger = logger;
            _renewalsRequestServices = renewalsRequestServices;
        }

        [HttpGet("find-all")] 
        [PermissionFilter(Permissions.InvestHDPP_HopDong_YeuCauTaiTuc)]
        public APIResponse Find([FromQuery] FilterInvRenewalsRequestDto dto)
        {
            try
            {
                var result = _renewalsRequestServices.Find(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("request")]
        [PermissionFilter(Permissions.InvestHDPP_HopDong_YeuCauTaiTuc)]
        public async Task<APIResponse> CreateRenewalsRequestCms([FromBody] CreateRenewalsRequestDto body)
        {
            try
            {
                var result = await _renewalsRequestServices.CreateRenewalsRequestCms(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("approve")]
        [PermissionFilter(Permissions.InvestPDYCTT_PheDuyetOrHuy)]
        public APIResponse ApproveRequest([FromBody] InvestApproveDto body)
        {
            try
            {
                _renewalsRequestServices.ApproveRequest(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("cancel")]
        [PermissionFilter(Permissions.InvestPDYCTT_PheDuyetOrHuy)]
        public APIResponse CancelRequest([FromBody] InvestCancelDto body)
        {
            try
            {
                _renewalsRequestServices.CancelRequest(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
