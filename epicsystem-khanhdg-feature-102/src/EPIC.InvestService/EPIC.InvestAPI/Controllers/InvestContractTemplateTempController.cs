using EPIC.DataAccess.Models;
using EPIC.EntitiesBase.Dto;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ContractTemplateTemp;
using EPIC.InvestEntities.Dto.Order;
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
using System.Collections.Generic;
using System.Net;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/contract-template-temp")]
    [ApiController]
    public class InvestContractTemplateTempController : BaseController
    {
        private readonly IInvestContractTemplateTempServices _investContractTemplateTempServices;
        public InvestContractTemplateTempController(ILogger<InvestContractTemplateTempController> logger,
            IInvestContractTemplateTempServices investContractTemplateTempServices)
        {
            _logger = logger;
            _investContractTemplateTempServices = investContractTemplateTempServices;
        }

        /// <summary>
        /// Tìm thông tin mẫu hợp đồng mẫu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        [ProducesResponseType(typeof(APIResponse<InvestContractTemplateTempDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.InvestMauHD_DanhSach)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _investContractTemplateTempServices.FindById(id);
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
        [ProducesResponseType(typeof(APIResponse<CreateInvestContractTemplateTempDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.InvestMauHD_ThemMoi)]
        public APIResponse Add([FromBody] CreateInvestContractTemplateTempDto input)
        {
            try
            {
                var result = _investContractTemplateTempServices.Add(input);
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
        [ProducesResponseType(typeof(APIResponse<UpdateInvestContractTemplateTempDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.InvestMauHD_CapNhat)]
        public APIResponse Update([FromBody] UpdateInvestContractTemplateTempDto input)
        {
            try
            {
                _investContractTemplateTempServices.Update(input);
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
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.InvestMauHD_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _investContractTemplateTempServices.Delete(id);
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
        [PermissionFilter(Permissions.InvestMauHD_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<PagingResult<InvestContractTemplateTemp>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterInvestContractTemplateTempDto input)
        {
            try
            {
                var result = _investContractTemplateTempServices.FindAllContractTemplateTemp(input);
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
        [PermissionFilter(Permissions.InvestPPDT_MauHopDong_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<InvestContractTemplateTempDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllContractTemplateTemp(int? contractSource)
        {
            try
            {
                var result = _investContractTemplateTempServices.GetAllContractTemplateTemp(contractSource);
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
        [PermissionFilter(Permissions.InvestMauHD_CapNhat)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                var result = _investContractTemplateTempServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
