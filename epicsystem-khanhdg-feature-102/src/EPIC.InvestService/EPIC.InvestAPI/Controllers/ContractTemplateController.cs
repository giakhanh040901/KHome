using EPIC.DataAccess.Models;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.ContractTemplate;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
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

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [ApiController]
    [Route("api/invest/contract-template")]
    public class ContractTemplateController : BaseController
    {
        private readonly IContractTemplateServices _contractTemplateServices;

        public ContractTemplateController(ILogger<ContractTemplateController> logger, IContractTemplateServices contractTemplateServices)
        {
            _logger = logger;
            _contractTemplateServices = contractTemplateServices;
        }

        [HttpGet]
        [Route("find")]
        [PermissionFilter(Permissions.InvestPPDT_MauHopDong_DanhSach)]

        //public APIResponse GetAllContractTemplates(int pageNumber, int? pageSize, string keyword, int distributionId, string type)
        //{
        //    try
        //    {
        //        var result = _contractTemplateServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), distributionId, type);
        //        return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        [HttpGet]
        [Route("find/{id}")]
        public APIResponse GetContractTemplate(int id)
        {
            try
            {
                var result = _contractTemplateServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        [Route("find-by-order")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewContractTemplateByOrder>>), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach, Permissions.GarnerHDPP_XLRutTien_ThongTinChiTiet, Permissions.GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet, Permissions.GarnerHDPP_LSTL_ThongTinChiTiet)]
        public APIResponse GetAllContractTemplatesByOrder(int pageNumber, int? pageSize, string keyword, int orderId)
        {
            try
            {
                var result = _contractTemplateServices.FindAllByOrderCheckDisplayType(pageSize ?? 100, pageNumber, keyword?.Trim(), orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost]
        [Route("add")]
        [PermissionFilter(Permissions.InvestPPDT_MauHopDong_ThemMoi)]
        public APIResponse AddContractTemplate([FromForm] CreateContractTemplateDto body)
        {
            try
            {
                _contractTemplateServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update")]
        [HttpPut]
        [PermissionFilter(Permissions.InvestPPDT_MauHopDong_CapNhat)]
        public APIResponse UpdateContractTemplate([FromForm] UpdateContractTemplateDto body)
        {
            try
            {
                var result = _contractTemplateServices.Update(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái Active/Deactive của mẫu hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.InvestPPDT_MauHopDong_CapNhat)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                _contractTemplateServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [PermissionFilter(Permissions.InvestPPDT_MauHopDong_Xoa)]

        public APIResponse DeleteContractTemplate(int id)
        {
            try
            {
                _contractTemplateServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách mẫu hợp đồng 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ContractTemplateDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllContractTemplate([FromQuery] ContractTemplateTempFilterDto input)
        {
            try
            {
                var result = _contractTemplateServices.FindAllContractTemplate(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        [Route("find-all-active/{distributionId}")]
        [ProducesResponseType(typeof(APIResponse<List<ContractTemplateDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllContractTemplateActive(int distributionId)
        {
            try
            {
                var result = _contractTemplateServices.FindAllContractTemplateActive(distributionId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        ///// <summary>
        ///// Thay đổi trạng thái hợp đồng
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpPut("change-status")]
        //public APIResponse ChangeStatus(int id)
        //{
        //    try
        //    {
        //        var result = _contractTemplateServices.ChangeStatus(id);
        //        return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}
    }
}
