using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.RenewalsRequest;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/renewals-request")]
    [ApiController]
    public class BondRenewalsRequestController : BaseController
    {
        private readonly IBondRenewalsRequestService _renewalsRequestServices;

        public BondRenewalsRequestController(ILogger<BondRenewalsRequestController> logger, IBondRenewalsRequestService renewalsRequestServices)
        {
            _logger = logger;
            _renewalsRequestServices = renewalsRequestServices;
        }

        [HttpGet("find-all")]
        public APIResponse Find([FromQuery] FilterRenewalsRequestDto body)
        {
            try
            {
                var result = _renewalsRequestServices.Find(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("request")]
        public APIResponse AddRequest([FromBody] CreateRenewalsRequestDto body)
        {
            try
            {
                var result = _renewalsRequestServices.AddRequest(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("approve")]
        [PermissionFilter(Permissions.BondQLPD_PDYCTT_PheDuyetOrHuy)]
        public APIResponse ApproveRequest([FromBody] ApproveStatusDto body)
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
        [PermissionFilter(Permissions.BondQLPD_PDYCTT_PheDuyetOrHuy)]
        public APIResponse CancelRequest([FromBody] CancelStatusDto body)
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
