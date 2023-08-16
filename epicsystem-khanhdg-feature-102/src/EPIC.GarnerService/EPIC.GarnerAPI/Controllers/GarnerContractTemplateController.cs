using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
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

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/contract-template")]
    [ApiController]
    public class GarnerContractTemplateController : BaseController
    {
        private readonly IGarnerContractTemplateServices _garnerContractTemplateServices;
        public GarnerContractTemplateController(ILogger<GarnerContractTemplateController> logger,
            IGarnerContractTemplateServices garnerContractTemplateServices)
        {
            _logger = logger;
            _garnerContractTemplateServices = garnerContractTemplateServices;
        }

        /// <summary>
        /// Tìm thông tin mẫu hợp đồng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        [ProducesResponseType(typeof(APIResponse<GarnerContractTemplateDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _garnerContractTemplateServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.GarnerPPDT_MauHopDong_ThemMoi)]
        public APIResponse Add([FromBody] CreateGarnerContractTemplateDto input)
        {
            try
            {
                 _garnerContractTemplateServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.GarnerPPDT_MauHopDong_CapNhat)]
        public APIResponse Update([FromBody] UpdateGarnerContractTemplateDto input)
        {
            try
            {
                var result = _garnerContractTemplateServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái mẫu hợp đồng
        /// </summary>
        [HttpPut("change-status/{id}")]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                _garnerContractTemplateServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Xóa mẫu hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.GarnerPPDT_MauHopDong_Xoa)]

        public APIResponse Delete(int id)
        {
            try
            {
                _garnerContractTemplateServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách hợp đồng (HSKH) cho sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("find-by-order")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach)]

        public APIResponse GetAllContractTemplatesByOrder(int orderId)
        {
            try
            {
                var result = _garnerContractTemplateServices.FindAllByOrderCheckDisplayType(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách mẫu hợp đồng theo id bán phân phối
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.GarnerPPDT_MauHopDong_DanhSach)]

        public APIResponse FindAllContractTemplate([FromQuery] GarnerContractTemplateFilterDto input)
        {
            try
            {
                var result = _garnerContractTemplateServices.FindAllContractTemplate(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách đang acive
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        [HttpGet("find-all-active/{distributionId}")]
        public APIResponse FindAllContractTemplateActive(int distributionId)
        {
            try
            {
                var result = _garnerContractTemplateServices.FindAllContractTemplateActive(distributionId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
