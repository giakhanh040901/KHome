using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstContractTemplateTemp;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/contract-template-temp")]
    [ApiController]
    public class RstContractTemplateTempController : BaseController
    {
        private readonly IRstContractTemplateTempServices _realEstateContractTemplateTempServices;
        public RstContractTemplateTempController(ILogger<RstContractTemplateTempController> logger,
            IRstContractTemplateTempServices realEstateContractTemplateTempServices)
        {
            _logger = logger;
            _realEstateContractTemplateTempServices = realEstateContractTemplateTempServices;
        }

        /// <summary>
        /// Tìm thông tin mẫu hợp đồng mẫu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        [ProducesResponseType(typeof(APIResponse<RstContractTemplateTempDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _realEstateContractTemplateTempServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateMauHDDL_ThemMoi, Permissions.RealStateMauHDCDT_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<CreateRstContractTemplateTempDto>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateRstContractTemplateTempDto input)
        {
            try
            {
                _realEstateContractTemplateTempServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateMauHDCDT_CapNhat, Permissions.RealStateMauHDDL_CapNhat)]
        [ProducesResponseType(typeof(APIResponse<UpdateRstContractTemplateTempDto>), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] UpdateRstContractTemplateTempDto input)
        {
            try
            {
                _realEstateContractTemplateTempServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateMauHDCDT_Xoa, Permissions.RealStateMauHDDL_Xoa)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Delete(int id)
        {
            try
            {
                _realEstateContractTemplateTempServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách Hợp đồng mẫu có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateMauHDCDT_DanhSach, Permissions.RealStateMauHDDL_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstContractTemplateTempDto input)
        {
            try
            {
                var result = _realEstateContractTemplateTempServices.FindAllContractTemplateTemp(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách Hợp đồng mẫu cho mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("get-all-contract")]
        public APIResponse GetAll([FromQuery] FilterRstContractTemplateTempDto input)
        {
            try
            {
                var result = _realEstateContractTemplateTempServices.GetAllContractTemplateTemp(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách Hợp đồng mẫu không phân trang 
        /// </summary>
        /// <param name="contractSource"></param>
        /// <returns></returns>
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetAllContractTemplateTemp(int? contractSource)
        {
            try
            {
                var result = _realEstateContractTemplateTempServices.GetAllContractTemplateTemp(contractSource);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateMauHDDL_DoiTrangThai, Permissions.RealStateMauHDCDT_DoiTrangThai)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                var result = _realEstateContractTemplateTempServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
