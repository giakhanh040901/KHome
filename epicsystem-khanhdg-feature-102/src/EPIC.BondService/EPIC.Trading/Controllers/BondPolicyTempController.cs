using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.ProductBondPolicyTemp;
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
using System.Threading.Tasks;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/policy-temp")]
    [ApiController]
    public class BondPolicyTempController : BaseController
    {
        private readonly IBondPolicyTempService _productBondPolicyTempServices;
        public BondPolicyTempController(ILogger<BondPolicyTempController> logger, IBondPolicyTempService productBondPolicyTempServices)
        {   
            _logger = logger;
            _productBondPolicyTempServices = productBondPolicyTempServices;
        }

        /// <summary>
        /// Tạo mới chính sách và các kỳ hạn của chính sách
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.BondCaiDat_CSM_ThemChinhSach)]
        public APIResponse Add([FromBody] CreateProductBondPolicyTempDto body)
        {
            try
            {
                _productBondPolicyTempServices.Add(body);
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
        [PermissionFilter(Permissions.BondCaiDat_CSM_ThemKyHan)]
        public APIResponse AddBondPolicyDetailTemplate([FromBody] ProductBondPolicyDetailTempDto body)
        {
            try
            {
                _productBondPolicyTempServices.AddBondPolicyDetailTemp(body);
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
        [PermissionFilter(Permissions.BondCaiDat_CSM_CapNhatChinhSach)]
        public APIResponse Update(int id,[FromBody] UpdateProductBondPolicyTempDto body)
        {
            try
            {
                var result = _productBondPolicyTempServices.UpdateProductBondPolicyTemp(id,body);
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
        [PermissionFilter(Permissions.BondCaiDat_CSM_CapNhatKyHan)]
        public APIResponse UpdatePolicyDetailTemp(int id, [FromBody] UpdateProductBondPolicyDetailTempDto body)
        {
            try
            {
                var result = _productBondPolicyTempServices.UpdateProductBondPolicyDetailTemp(id, body);
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
        [PermissionFilter(Permissions.BondCaiDat_CSM_KichHoatOrHuy)]
        public APIResponse ChangeStatusProductBondPolicyTemp(int id)
        {
            try
            {
                var result = _productBondPolicyTempServices.ChangeStatusProductBondPolicyTemp(id);
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
        [PermissionFilter(Permissions.BondCaiDat_CSM_KyHan_KichHoatOrHuy)]
        public APIResponse ChangeStatusProductBondPolicyDetailTemp(int id)
        {
            try
            {
                var result = _productBondPolicyTempServices.ChangeStatusProductBondPolicyDetailTemp(id);
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
        [PermissionFilter(Permissions.BondCaiDat_CSM_XoaChinhSach)]
        public APIResponse DeleteProductBondPolicyTemp(int id)
        {
            try
            {
                var result = _productBondPolicyTempServices.DeleteProductBondPolicyTemp(id);
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
        [PermissionFilter(Permissions.BondCaiDat_CSM_XoaKyHan)]
        public APIResponse DeleteProductBondPolicyDetailTemp(int id)
        {
            try
            {
                var result = _productBondPolicyTempServices.DeleteProductBondPolicyDetailTemp(id);
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
        [PermissionFilter(Permissions.BondCaiDat_CSM_DanhSach)]
        public APIResponse FindAll(int? pageSize, int pageNumber, bool? isNoPaging, string keyword, string status, decimal? classify)
        {
            try
            {
                var result = _productBondPolicyTempServices.FindAll(pageSize ?? 100, pageNumber, isNoPaging ?? true, keyword?.Trim(), status?.Trim(), classify);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-by-id")]
        [PermissionFilter(Permissions.BondCaiDat_CSM_ThemKyHan)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _productBondPolicyTempServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
