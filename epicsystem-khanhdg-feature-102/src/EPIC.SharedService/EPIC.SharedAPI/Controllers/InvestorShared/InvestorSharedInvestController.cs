using EPIC.Shared.Filter;
using EPIC.SharedDomain.Implements;
using EPIC.SharedDomain.Interfaces;
using EPIC.SharedEntities.Dto.InvestorTelesale;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.SharedAPI.Controllers.InvestorShared
{
    /// <summary>
    /// Chia sẻ thông tin khách hàng đầu tư
    /// </summary>
    [WhiteListIpFilter(WhiteListIpTypes.ThongTinKhachDauTuTelesale)]
    [Route("api/shared/investor-shared/invest")]
    [ApiController]
    public class InvestorSharedInvestController : BaseController
    {
        private readonly IInvestorSharedTelesaleServices _investorSharedServices;

        public InvestorSharedInvestController(ILogger<InvestorSharedInvestController> logger,
            IInvestorSharedTelesaleServices investorSharedServices)
        {
            _logger = logger;
            _investorSharedServices = investorSharedServices;
        }

        /// <summary>
        /// Tìm thông tin các khoản đầu tư của khách hàng theo số giấy tờ
        /// </summary>
        /// <param name="idNo"></param>
        /// <returns></returns>
        [HttpGet("find/{idNo}")]
        public APIResponse FindInvestInfo(string idNo)
        {
            try
            {
                var result = _investorSharedServices.FindInvestInfo(idNo);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm danh sách hợp đồng đang đầu tư của invest + garner theo số giấy tờ của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-active-info")]
        public APIResponse FindActivesInfo([FromQuery] FilterInvestorTelesaleDto input)
        {
            try
            {
                var result = _investorSharedServices.FindActiveInfo(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
