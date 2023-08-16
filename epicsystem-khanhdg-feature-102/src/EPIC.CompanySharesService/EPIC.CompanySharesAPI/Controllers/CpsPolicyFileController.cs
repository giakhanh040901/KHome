using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.Dto.PolicyFile;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CompanySharesAPI.Controllers
{
    [Authorize]
    [Route("api/cps/policy-file")]
    [ApiController]
    public class CpsPolicyFileController : BaseController
    {
        private ICpsPolicyFileServices _cpsPolicyFileServices;

        public CpsPolicyFileController(ILogger<CpsPolicyFileController> logger, ICpsPolicyFileServices policyFileServices)
        {
            _logger = logger;
            _cpsPolicyFileServices = policyFileServices;
        }
        /// <summary>
        /// Thêm file chính sách
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public APIResponse AddPolicyFile([FromBody] CreateCpsPolicyFileDto body)
        {
            try
            {
                var result = _cpsPolicyFileServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);

            }
        }
        /// <summary>
        /// tìm file chính sách phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("fileAll/find")]
        [HttpGet]
        public APIResponse GetAllPolicyFiles([FromQuery] CpsPolicyFileFilterDto input)
        {
            try
            {
                var result = _cpsPolicyFileServices.FindAllPolicyFile(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// tìm file chính sách theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("find/{id}")]
        [HttpGet]
        public APIResponse GetPolicyFileById(int id)
        {
            try
            {
                var result = _cpsPolicyFileServices.FindPolicyFileById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// xoá file chính sách ở trạng thái tạm theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete/{id}")]
        [HttpDelete]
        public APIResponse DeletePolicyFile(int id)
        {
            try
            {
                var result = _cpsPolicyFileServices.DeletePolicyFile(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Cập nhật file chính sách theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("update/{id}")]
        [HttpPut]
        public APIResponse PolicyFileUpdate(int id, [FromBody] UpdateCpsPolicyFileDto body)
        {
            try
            {
                var result = _cpsPolicyFileServices.PolicyFileUpdate(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
