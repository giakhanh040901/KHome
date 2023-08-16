using EPIC.EntitiesBase.Dto;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerProduct;
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
using System.Net;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/contract-template-temp")]
    [ApiController]
    public class GarnerContractTemplateTempController : BaseController
    {
        private readonly IGarnerContractTemplateTempServices _garnerContractTemplateTempServices;
        public GarnerContractTemplateTempController(ILogger<GarnerContractTemplateTempController> logger,
            IGarnerContractTemplateTempServices garnerContractTemplateTempServices)
        {
            _logger = logger;
            _garnerContractTemplateTempServices = garnerContractTemplateTempServices;
        }


        /// <summary>
        /// Tìm thông tin mẫu hợp đồng mẫu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        [ProducesResponseType(typeof(APIResponse<GarnerContractTemplateTempDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _garnerContractTemplateTempServices.FindById(id);
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
        [PermissionFilter(Permissions.GarnerCSM_HopDongMau_ThemMoi)]

        [ProducesResponseType(typeof(APIResponse<CreateGarnerContractTemplateTempDto>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateGarnerContractTemplateTempDto input)
        {
            try
            {
                var result = _garnerContractTemplateTempServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
        [PermissionFilter(Permissions.GarnerCSM_HopDongMau_CapNhat)]
        [ProducesResponseType(typeof(APIResponse<UpdateGarnerContractTemplateTempDto>), (int)HttpStatusCode.OK)]
        public APIResponse Update([FromBody] UpdateGarnerContractTemplateTempDto input)
        {
            try
            {
                _garnerContractTemplateTempServices.Update(input);
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
        [PermissionFilter(Permissions.GarnerCSM_HopDongMau_Xoa)]

        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse Delete(int id)
        {
            try
            {
                _garnerContractTemplateTempServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách Hợp đồng mẫu theo chính sách
        /// </summary>
        /// <param name="policyTempId"></param>
        /// <returns></returns>
        [HttpGet("find-by-policy-temp")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse FindContractByPolicyTempId(int policyTempId)
        {
            try
            {
                var result = _garnerContractTemplateTempServices.FindAll(policyTempId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
        public APIResponse FindAll([FromQuery] FilterGarnerContractTemplateTempDto input)
        {
            try
            {
                var result = _garnerContractTemplateTempServices.FindAllContractTemplateTemp(input);
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
        [ProducesResponseType(typeof(APIResponse<List<GarnerContractTemplateTempDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllContractTemplateTemp(int? contractSource)
        {
            try
            {
                var result = _garnerContractTemplateTempServices.GetAllContractTemplateTemp(contractSource);
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
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                var result = _garnerContractTemplateTempServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
