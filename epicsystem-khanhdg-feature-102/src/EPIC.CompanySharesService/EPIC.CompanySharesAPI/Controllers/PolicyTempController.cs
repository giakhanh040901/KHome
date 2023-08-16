using EPIC.Entities.Dto.ProductBondPolicyTemp;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.Dto.PolicyTemp;

namespace EPIC.CompanySharesAPI.Controllers
{
    [Authorize]
    [Route("api/company-shares/policy-temp")]
    [ApiController]
    public class PolicyTempController : BaseController
    {
        private readonly IPolicyTempServices _policyTempServices;
        public PolicyTempController(ILogger<PolicyTempController> logger, IPolicyTempServices policyTempServices)
        {
            _logger = logger;
            _policyTempServices = policyTempServices;
        }

        /// <summary>
        /// Tạo mới chính sách và các kỳ hạn của chính sách
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        //[PermissionFilter(Permissions.BondCaiDat_CSM_ThemChinhSach)]
        public APIResponse Add([FromBody] CreatePolicyTempDto body)
        {
            try
            {
                _policyTempServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo mới chính sách và các kỳ hạn của chính sách
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add-policy-detail-temp")]
        //[PermissionFilter(Permissions.BondCaiDat_CSM_ThemKyHan)]
        public APIResponse AddPolicyDetailTemplate([FromBody] PolicyDetailTempDto body)
        {
            try
            {
                _policyTempServices.AddPolicyDetailTemp(body);
                return new APIResponse(Utils.StatusCode.Success, body, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Update chính sách
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update")]
        //[PermissionFilter(Permissions.BondCaiDat_CSM_CapNhatChinhSach)]
        public APIResponse Update(int id, [FromBody] UpdatePolicyTempDto body)
        {
            try
            {
                var result = _policyTempServices.UpdatePolicyTemp(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update kỳ hạn
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update-policy-detail-temp")]
        //[PermissionFilter(Permissions.BondCaiDat_CSM_CapNhatKyHan)]
        public APIResponse UpdatePolicyDetailTemp(int id, [FromBody] UpdatePolicyDetailTempDto body)
        {
            try
            {
                var result = _policyTempServices.UpdatePolicyDetailTemp(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái chính sách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status")]
        //[PermissionFilter(Permissions.BondCaiDat_CSM_KichHoatOrHuy)]
        public APIResponse ChangeStatusPolicyTemp(int id)
        {
            try
            {
                var result = _policyTempServices.ChangeStatusPolicyTemp(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái Kỳ hạn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status-policy-detail-temp")]
        //[PermissionFilter(Permissions.BondCaiDat_CSM_KyHan_KichHoatOrHuy)]
        public APIResponse ChangeStatusPolicyDetailTemp(int id)
        {
            try
            {
                var result = _policyTempServices.ChangeStatusPolicyDetailTemp(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa chính sách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        //[PermissionFilter(Permissions.BondCaiDat_CSM_XoaChinhSach)]
        public APIResponse DeletePolicyTemp(int id)
        {
            try
            {
                var result = _policyTempServices.DeletePolicyTemp(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa kỳ hạn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-policy-detail-temp")]
        //[PermissionFilter(Permissions.BondCaiDat_CSM_XoaKyHan)]
        public APIResponse DeletePolicyDetailTemp(int id)
        {
            try
            {
                var result = _policyTempServices.DeletePolicyDetailTemp(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm chính sách (Trả về list kèm theo kỳ hạn)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="isNoPaging"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="classify"></param>
        /// <returns></returns>
        [HttpGet("find")]
        //[PermissionFilter(Permissions.BondCaiDat_CSM_DanhSach)]
        public APIResponse FindAll(int? pageSize, int pageNumber, bool? isNoPaging, string keyword, string status, decimal? classify)
        {
            try
            {
                var result = _policyTempServices.FindAll(pageSize ?? 100, pageNumber, isNoPaging ?? true, keyword?.Trim(), status?.Trim(), classify);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-by-id")]
        //[PermissionFilter(Permissions.BondCaiDat_CSM_ThemKyHan)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _policyTempServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
