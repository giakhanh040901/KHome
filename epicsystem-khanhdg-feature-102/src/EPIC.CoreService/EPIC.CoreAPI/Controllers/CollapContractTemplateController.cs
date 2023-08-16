using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.CollabContractTemp;
using EPIC.Entities.Dto.CoreCollabContractTemp;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CoreAPI.Controllers
{
    /// <summary>
    /// Hợp đồng cộng tác
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/collap-contract")]
    [ApiController]
    public class CollapContractTemplateController : BaseController
    {
        private readonly ICollabContractServices _collabContractServices;

        public CollapContractTemplateController(ILogger<CollapContractTemplateController> logger, ICollabContractServices collabContractServices)
        {
            _logger = logger;
            _collabContractServices = collabContractServices;
        }

        /// <summary>
        /// tìm tất cả hợp đồng cộng tác
        /// </summary>
        /// <returns></returns>
        [HttpGet("find")]
        [PermissionFilter(Permissions.CoreHDCT_Template_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterCollabContractTempDto input)
        {
            try
            {
                var result = _collabContractServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        [HttpGet]
        [Route("find-by-sale")]
        [PermissionFilter(Permissions.CoreSaleActive_HDCT_DanhSach)]
        public APIResponse GetAllCollabContractTemplatesBySale(int pageNumber, int? pageSize, int saleId)
        {
            try
            {
                var result = _collabContractServices.FindAllBySale(pageSize ?? 100, pageNumber, saleId, null);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tìm kiếm hợp đồng cộng tác id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _collabContractServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thêm mới hợp đồng cộng tác
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.CoreHDCT_Template_ThemMoi)]
        public APIResponse Add([FromBody] CreateCollabContractTempDto body)
        {
            try
            {
                var result = _collabContractServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// cập nhật lại hợp đồng cộng tác
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.CoreHDCT_Template_CapNhat)]
        public APIResponse Update([FromBody] UpdateCollabContractTempDto body)
        {
            try
            {
                var result = _collabContractServices.Update(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa hợp đồng cộng tác theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.CoreHDCT_Template_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _collabContractServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
