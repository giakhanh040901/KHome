using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.PartnerMsbPrefixAccount;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CoreAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/partner/msb-prefix-account")]
    [ApiController]
    public class PartnerMsbPrefixAccountController : BaseController
    {
        private readonly IPartnerMsbPrefixAccountServices _partnerMsbPrefixAccountServices;
        public PartnerMsbPrefixAccountController(ILogger<PartnerMsbPrefixAccountController> logger,
            IPartnerMsbPrefixAccountServices partnerMsbPrefixAccountServices)
        {
            _logger = logger;
            _partnerMsbPrefixAccountServices = partnerMsbPrefixAccountServices;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse FindAll([FromQuery] FilterPartnerMsbPrefixAccountDto input)
        {
            try
            {
                var result = _partnerMsbPrefixAccountServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _partnerMsbPrefixAccountServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin bank theo bank account id của đối tác
        /// </summary>
        /// <param name="partnerBankAccId"></param>
        /// <returns></returns>
        [HttpGet("find-by-partner-bank-id/{partnerBankAccId}")]
        public APIResponse FindByPartnerBankId(int partnerBankAccId)
        {
            try
            {
                var result = _partnerMsbPrefixAccountServices.FindByPartnerBankId(partnerBankAccId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse Add([FromBody] CreatePartnerMsbPrefixAccountDto input)
        {
            try
            {
                var result = _partnerMsbPrefixAccountServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse Update([FromBody] UpdatePartnerMsbPrefixAccountDto input)
        {
            try
            {
                _partnerMsbPrefixAccountServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                _partnerMsbPrefixAccountServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
