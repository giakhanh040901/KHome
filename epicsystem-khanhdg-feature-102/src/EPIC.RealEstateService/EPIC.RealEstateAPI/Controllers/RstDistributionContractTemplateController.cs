using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstDistributionContractTemplate;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
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

namespace EPIC.RealEstateAPI.Controllers
{

    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/distribution-contract-template")]
    [ApiController]
    public class RstDistributionContractTemplateController : BaseController
    {
        private readonly IRstDistributionContractTemplateServices _rstDistributionContractTemplateServices;
        public RstDistributionContractTemplateController(ILogger<RstDistributionContractTemplateController> logger,
            IRstDistributionContractTemplateServices rstDistributionContractTemplateServices)
        {
            _logger = logger;
            _rstDistributionContractTemplateServices = rstDistributionContractTemplateServices;
        }

        /// <summary>
        /// Thêm biểu mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_MauBieu_ThemMoi)]
        public APIResponse Add(CreateRstDistributionContractTemplateDto input)
        {
            try
            {
                var result = _rstDistributionContractTemplateServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// cập nhật biểu mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_MauBieu_ChinhSua)]
        public APIResponse Update(UpdateRstDistributionContractTemplateDto input)
        {
            try
            {
                var result = _rstDistributionContractTemplateServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Xóa biểu mẫu hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstDistributionContractTemplateServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái biểu mẫu hợp đồng
        /// </summary>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_MauBieu_DoiTrangThai)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                _rstDistributionContractTemplateServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm biểu mẫu hợp đồng
        /// </summary>
        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(APIResponse<RstDistributionContractTemplateDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstDistributionContractTemplateServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm danh sách biểu mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_MauBieu_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<RstDistributionContractTemplateDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterRstDistributionContractTemplateDto input)
        {
            try
            {
                var result = _rstDistributionContractTemplateServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
