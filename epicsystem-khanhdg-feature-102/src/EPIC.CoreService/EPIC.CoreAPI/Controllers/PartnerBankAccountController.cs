using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.PartnerBankAccount;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CoreAPI.Controllers
{
    [Route("api/core/partner-bank-account")]
    [ApiController]
    public class PartnerBankAccountController : BaseController
    {
        private readonly IPartnerBankAccountServices _partnerBankAccountServices;
        public PartnerBankAccountController(ILogger<PartnerBankAccountController> logger,
            IPartnerBankAccountServices partnerBankAccountServices)
        {
            _logger = logger;
            _partnerBankAccountServices = partnerBankAccountServices;
        }

        /// <summary>
        /// Danh sách tài khoản ngân hàng của đối tác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse FindAll([FromQuery] FilterPartnerBankAccountDto input)
        {
            try
            {
                var result = _partnerBankAccountServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm tài khoản ngân hàng của đối tác theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse FindById(int id, int? partnerId)
        {
            try
            {
                var result = _partnerBankAccountServices.FindById(id, partnerId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái tài khoản đối tác
        /// </summary>
        /// <returns></returns>
        [HttpPut("change-status")]
        public APIResponse ChangeStatus(int id, int? partnerId)
        {
            try
            {
                var result = _partnerBankAccountServices.ChangeStatus(id, partnerId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Set tài khoản ngân hàng của đối tác mặc định
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [HttpPut("set-default")]
        public APIResponse SetDefault(int id, int? partnerId)
        {
            try
            {
                var result = _partnerBankAccountServices.SetDefault(id, partnerId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật tài khoản ngân hàng đối tác 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse SetDefault([FromBody] UpdatePartnerBankAccountDto input)
        {
            try
            {
                var result = _partnerBankAccountServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm tài khoản ngân hàng đối tác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse Add([FromBody] CreatePartnerBankAccountDto input)
        {
            try
            {
                var result = _partnerBankAccountServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpDelete("delete")]
        public APIResponse Delete(int id, int? partnerId)
        {
            try
            {
                _partnerBankAccountServices.Delete(id, partnerId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
