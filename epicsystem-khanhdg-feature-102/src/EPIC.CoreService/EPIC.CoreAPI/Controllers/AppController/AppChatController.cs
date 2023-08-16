using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPIC.CoreAPI.Controllers.AppController
{
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/core/chat")]
    [ApiController]
    public class AppChatController : BaseController
    {
        private readonly IRocketChatServices _rocketChatServices;

        public AppChatController(IRocketChatServices rocketChatServices)
        {
            _rocketChatServices = rocketChatServices;
        }

        /// <summary>
        /// Các department của màn liên hệ trên app
        /// </summary>
        /// <returns></returns>
        [HttpGet("departments")]
        public async Task<APIResponse> GetListDepartments()
        {
            try
            {
                var result = await _rocketChatServices.GetListDepartmentForInvestor();
                var status = result != null ? Utils.StatusCode.Success : Utils.StatusCode.Error;
                return new APIResponse(status, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy department theo trading
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet("department/trading/{tradingProviderId}")]
        public async Task<APIResponse> GetDepartmentByTradingId(int tradingProviderId)
        {
            try
            {
                var departmentName = _rocketChatServices.genChannelName(new Entities.DataEntities.TradingProvider { TradingProviderId = tradingProviderId }, null);
                var result = await _rocketChatServices.GetListDepartment(departmentName, true);
                var status = result != null ? Utils.StatusCode.Success : Utils.StatusCode.Error;
                return new APIResponse(status, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
