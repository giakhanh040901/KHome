using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstDistributionContractTemplate;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System;
using EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/open-sell-contract-template")]
    [ApiController]
    public class RstOpenSellContractTemplateController : BaseController
    {
        private readonly IRstOpenSellContractTemplateServices _rstOpenSellContractTemplateServices;
        public RstOpenSellContractTemplateController(ILogger<RstOpenSellContractTemplateController> logger,
            IRstOpenSellContractTemplateServices rstOpenSellContractTemplateServices)
        {
            _logger = logger;
            _rstOpenSellContractTemplateServices = rstOpenSellContractTemplateServices;
        }

        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateMoBan_MauBieu_ThemMoi)]
        public APIResponse Add([FromBody]CreateRstOpenSellContractTemplateDto input)
        {
            try
            {
                var result = _rstOpenSellContractTemplateServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// cập nhật mẫu hợp đồng
        /// </summary>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateMoBan_MauBieu_ChinhSua)]
        public APIResponse Update([FromBody] UpdateRstOpenSellContractTemplateDto input)
        {
            try
            {
                _rstOpenSellContractTemplateServices.Update(input);
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
        [HttpDelete("delete/{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstOpenSellContractTemplateServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái mẫu hợp đồng
        /// </summary>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateMoBan_MauBieu_DoiTrangThai)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                _rstOpenSellContractTemplateServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem mẫu hợp đồng
        /// </summary>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstOpenSellContractTemplateServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách mẫu hợp đồng
        /// </summary>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateMoBan_MauBieu_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<RstOpenSellContractTemplateDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterRstOpenSellContractTemplateDto input)
        {
            try
            {
                var result = _rstOpenSellContractTemplateServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
