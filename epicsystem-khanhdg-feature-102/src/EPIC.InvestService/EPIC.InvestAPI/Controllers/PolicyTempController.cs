using EPIC.DataAccess.Models;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.PolicyTemp;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/policy-temp")]
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
        [PermissionFilter(Permissions.InvestCSM_ThemMoi)]
        public APIResponse Add([FromBody] CreatePolicyTempDto body)
        {
            try
            {
                var result = _policyTempServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
        [PermissionFilter(Permissions.InvestCSM_KyHan_ThemMoi)]
        public APIResponse AddBondPolicyDetailTemplate([FromBody] PolicyDetailTempDto body)
        {
            try
            {
                _policyTempServices.AddBondPolicyDetailTemp(body);
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
        [PermissionFilter(Permissions.InvestCSM_CapNhat)]
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
        [PermissionFilter(Permissions.InvestCSM_KyHan_CapNhat)]
        public APIResponse UpdatePolicyDetailTemp(int id, [FromBody] UpdatePolicyDetailTempDto body)
        {
            try
            {
                var result = _policyTempServices.UpdateProductBondPolicyDetailTemp(id, body);
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
        [PermissionFilter(Permissions.InvestCSM_KichHoatOrHuy)]
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
        [PermissionFilter(Permissions.InvestCSM_KyHan_KichHoatOrHuy)]
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
        [PermissionFilter(Permissions.InvestCSM_Xoa)]
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
        [PermissionFilter(Permissions.InvestCSM_KyHan_Xoa)]
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
        /// <returns></returns>
        [HttpGet("find")]
        [PermissionFilter(Permissions.InvestCSM_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterPolicyTempDto input)
        {
            try
            {
                var result = _policyTempServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-by-id")]
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

        /// <summary>
        /// Tìm kiếm chính sách (không phân trang)
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-no-permission")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewPolicyTemp>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllPolicyTempNoPermission(string status)
        {
            try
            {
                var result = _policyTempServices.FindAllPolicyTempNoPermission(status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
