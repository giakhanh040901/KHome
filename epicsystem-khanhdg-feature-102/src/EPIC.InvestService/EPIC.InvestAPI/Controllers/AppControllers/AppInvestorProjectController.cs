using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EPIC.InvestAPI.Controllers.AppControllers
{
    [Route("api/invest/investor-project")]
    [ApiController]
    public class AppInvestorProjectController : BaseController
    {
        private readonly IDistributionServices _distributionServices;
        private readonly IInvestSharedServices _investSharedServices;
        public AppInvestorProjectController(ILogger<AppInvestorProjectController> logger, 
            IDistributionServices distributionServices,
            IInvestSharedServices investSharedServices)
        {
            _logger = logger;
            _distributionServices = distributionServices;
            _investSharedServices = investSharedServices;
        }

        #region lấy thông tin dự án đầu tư
        /// <summary>
        /// Lấy danh sách dự án đầu tư
        /// Nếu là khách hàng đăng nhập thì sẽ xem được bảng hàng cố định
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find")]
        [ProducesResponseType(typeof(APIResponse<List<ProjectDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllProjectDistribution([FromQuery] AppFilterProjectDistribution input)
        {
            try
            {
                var result = _distributionServices.FindAllProjectDistribution(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông tin chi tiết dự án
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        [HttpGet("find/{distributionId}")]
        public APIResponse FindProjectById([Range(1, int.MaxValue)] int distributionId)
        {
            try
            {
                var result = _distributionServices.FindProjectById(distributionId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy chính sách (sản phẩm), tất cả chính sách còn mở, cho show app
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
        [HttpGet("policy/{distributionId}")]
        [ProducesResponseType(typeof(APIResponse<List<AppInvestPolicyDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllPolicy([Range(1, int.MaxValue)] int distributionId)
        {
            try
            {
                var result = _distributionServices.FindAllListPolicy(distributionId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get kỳ hạn, còn show app của chính sách đang chọn
        /// <param name="policyId"></param>
        /// <param name="totalValue"></param>
        /// </summary>
        [Authorize]
        [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
        [HttpGet("policy-detail/{policyId}")]
        [ProducesResponseType(typeof(APIResponse<List<AppInvestPolicyDetailDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllPolicyDetail([Range(1, int.MaxValue)] int policyId, [Required] [Range(1, double.MaxValue)] decimal? totalValue)
        {
            try
            {
                var result = _distributionServices.FindAllListPolicyDetail(policyId, totalValue ?? 0);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get mô tả dòng tiền
        /// </summary>
        [Authorize]
        [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
        [HttpGet("cash-flow")]
        public APIResponse GetCashFlow([Range(1, double.MaxValue)] decimal totalValue, [Range(1, int.MaxValue)] int policyDetailId)
        {
            try
            {
                var result = _investSharedServices.GetCashFlow(totalValue, policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
    }
}
