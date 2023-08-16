using EPIC.CoreDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.CoreAPI.Controllers
{
    [Route("api/core/bank")]
    [ApiController]
    //[Authorize]
    public class BankController : BaseController
    {
        private readonly ICoreBankServices _bankServices;
        public BankController(ICoreBankServices bankServices)
        {
            _bankServices = bankServices;
        }

        /// <summary>
        /// Lấy list bank cho dropdown
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public APIResponse GetListBank(string keyword)
        {
            try
            {
                var result = _bankServices.GetListBank(keyword);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
