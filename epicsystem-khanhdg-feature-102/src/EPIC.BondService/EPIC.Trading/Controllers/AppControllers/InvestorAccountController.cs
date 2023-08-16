using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.BondInvestorAccount;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.BondAPI.Controllers.InvestorControllers
{
    [Authorize]
    //[AuthorizeUserTypeFilter(UserData.INVESTOR)]
    [Route("api/bond/investor-account")]
    [ApiController]
    public class InvestorAccountController : BaseController
    {
        private readonly IInvestorAccountService _userServices;
        public InvestorAccountController(ILogger<InvestorAccountController> logger,
            IInvestorAccountService userServices)
        {
            _logger = logger;
            _userServices = userServices;
        }

        /// <summary>
        /// Lấy toàn bộ tài khoản loại của investor (Chưa lọc theo Trading Provider)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("find")]
        public APIResponse GetAllAccountInvestor([FromQuery] FindBondInvestorAccountDto dto)
        {
            try
            {
                var investorAccount = _userServices.GetByType(dto);
                return new APIResponse(Utils.StatusCode.Success, investorAccount, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
