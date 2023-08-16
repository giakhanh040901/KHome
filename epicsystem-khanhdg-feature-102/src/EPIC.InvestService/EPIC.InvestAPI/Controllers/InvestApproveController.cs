using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/approve")]
    [ApiController]
    public class InvestApproveController : BaseController
    {
        private readonly IInvestApproveServices _approveServices;

        public InvestApproveController(ILogger<InvestApproveController> logger, IInvestApproveServices approveServices)
        {
            _logger = logger;
            _approveServices = approveServices;
        }

        /// <summary>
        /// Get All Bảng Core Approve
        /// </summary>
        [HttpGet]
        [Route("all")]
        [PermissionFilter(Permissions.InvestPDPPDT_DanhSach, Permissions.InvestPDSPDT_DanhSach, Permissions.InvestPDYCTT_DanhSach)]
        public APIResponse GetAll(int pageSize, int pageNumber, string keyword, int? status, int? userApproveId, int? userRequestId, int? dataType, int? actionType, DateTime? requestDate, DateTime? approveDate)
        {
            try
            {
                var result = _approveServices.Find(new InvestApproveGetDto
                {
                    Keyword = keyword,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Status = status,
                    UserApproveId = userApproveId,
                    UserRequestId = userRequestId,
                    ActionType = actionType,
                    DataType = dataType,
                    RequestDate = requestDate,
                    ApproveDate = approveDate,
                });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
