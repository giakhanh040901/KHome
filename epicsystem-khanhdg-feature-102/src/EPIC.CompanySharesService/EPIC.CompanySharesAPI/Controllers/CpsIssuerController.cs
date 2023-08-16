using EPIC.CompanySharesDomain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using EPIC.Utils.Controllers;
using EPIC.Utils;
using EPIC.CompanySharesEntities.Dto.Issuer;

namespace EPIC.CompanySharesAPI.Controllers
{
    [Authorize]
    [Route("api/company-shares/issuer")]
    [ApiController]
    public class CpsIssuerController : BaseController
    {
        private readonly ICpsIssuerService _cpsIssuerService;
        public CpsIssuerController(ILogger<CpsIssuerController> logger, ICpsIssuerService cpsIssuerService)
        {
            _logger = logger;
            _cpsIssuerService = cpsIssuerService;
        }
        /// <summary>
        /// Thêm BondIssuer
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse Add([FromBody] CreateCpsIssuerDto body)
        {
            try
            {
                var result = _cpsIssuerService.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Xóa BondIssuer ở trạng thái tạm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        //[PermissionFilter(Permissions.CompanyShares_TCPH_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _cpsIssuerService.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Sửa BondIssuer theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        //[PermissionFilter(Permissions.CompanyShares_TCPH_CapNhat)]
        public APIResponse Update(int id, [FromBody] UpdateCpsIssuerDto body)
        {
            try
            {
                var result = _cpsIssuerService.Update(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Tìm BondIssuer theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[PermissionFilter(Permissions.CompanyShares_TCPH_ThongTinChiTiet)]

        public APIResponse Get(int id)
        {
            try
            {
                var result = _cpsIssuerService.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm tất cả BondIssuer 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find")]
        //[PermissionFilter(Permissions.CompanyShares_TCPH_DanhSach)]
        public APIResponse GetAll([FromQuery] CpsIssuerFilterDto input)
        {
            try
            {
                var result = _cpsIssuerService.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
