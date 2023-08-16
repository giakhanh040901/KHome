using EPIC.CoreDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EPIC.CoreAPI.Controllers.AppController
{
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/core/investor/asset-manager")]
    [ApiController]
    public class AppAssetManagerController : BaseController
    {
        private readonly IAssetManagerServices _managerCashFlowServices;

        public AppAssetManagerController(IAssetManagerServices managerCashFlowServices)
        {
            _managerCashFlowServices = managerCashFlowServices;
        }

        /// <summary>
        /// Thông tin quản lý tài sản của khách hàng màn tổng quan
        /// </summary>
        /// <returns></returns>
        [HttpGet("find")]
        public APIResponse AssetManagerInvestor()
        {
            try
            {
                var result = _managerCashFlowServices.AssetManagerInvestor();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông tin tài sản đầu tư - màn đầu tư
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-invest")]
        public APIResponse AssetManagerInvest()
        {
            try
            {
                var result = _managerCashFlowServices.InvestManager();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
