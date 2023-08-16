using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace EPIC.GarnerAPI.Controllers.AppController
{
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/garner/investor-product")]
    [ApiController]
    public class AppGarnerProductController : BaseController
    {
        private readonly IGarnerDistributionServices _garnerDistributionServices;

        public AppGarnerProductController(ILogger<AppGarnerProductController> logger,
            IGarnerDistributionServices garnerDistributionServices)
        {
            _logger = logger;
            _garnerDistributionServices = garnerDistributionServices;
        }

        /// <summary>
        /// danh sách sản phẩm
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("distribution-filter")]
        [ProducesResponseType(typeof(APIResponse<List<AppGarnerDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppFilterDistribution(string keyword)
        {
            try
            {
                var result = _garnerDistributionServices.AppFilterDistribution(keyword?.Trim(), null);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách sản phẩm cho investor
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("find")]
        [ProducesResponseType(typeof(APIResponse<List<AppGarnerDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppDistributionGetAll(string keyword, bool isSaleView)
        {
            try
            {
                var result = _garnerDistributionServices.AppDistributionGetAll(keyword?.Trim(), isSaleView);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách kỳ hạn theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="totalValue"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("find-list-policy-detail")]
        [ProducesResponseType(typeof(APIResponse<List<AppGarnerPolicyDetailDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppListPolicyDetail(int policyId, decimal totalValue)
        {
            try
            {
                var result = _garnerDistributionServices.AppListPolicyDetail(policyId, totalValue);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tổng quan sản phẩm tích lũy
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("product-overview/{policyId}")]
        [ProducesResponseType(typeof(APIResponse<AppDistributionOverviewDto>), (int)HttpStatusCode.OK)]
        public APIResponse DistributionOverview(int policyId)
        {
            try
            {
                var result = _garnerDistributionServices.DistributionOverview(policyId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
