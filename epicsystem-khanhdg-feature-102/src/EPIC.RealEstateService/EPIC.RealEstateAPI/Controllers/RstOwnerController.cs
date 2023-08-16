using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstOwner;
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

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/owner")]
    [ApiController]
    public class RstOwnerController : BaseController
    {
        private readonly IRstOwnerServices _realEstateOwnerServices;
        public RstOwnerController(ILogger<RstOwnerController> logger,
            IRstOwnerServices realEstateOwnerServices)
        {
            _logger = logger;
            _realEstateOwnerServices = realEstateOwnerServices;
        }

        /// <summary>
        /// Tìm danh sách chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateChuDT_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstOwnerDto input)
        {
            try
            {
                var result = _realEstateOwnerServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách chủ đầu tư theo đối tác
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-by-partner")]
        public APIResponse GetAllOwnerByPartner()
        {
            try
            {
                var result = _realEstateOwnerServices.GetAllOwnerByPartner();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách chủ đầu tư theo đại lý
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-by-trading")]
        public APIResponse GetAllOwnerByTrading()
        {
            try
            {
                var result = _realEstateOwnerServices.GetAllOwnerByTrading();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Thêm chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateChuDT_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstOwnerDto input)
        {
            try
            {
                _realEstateOwnerServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateChuDT_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstOwnerDto input)
        {
            try
            {
                _realEstateOwnerServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.RealStateChuDT_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _realEstateOwnerServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái chủ đầu tư
        /// </summary>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [PermissionFilter(Permissions.RealStateChuDT_KichHoatOrHuy)]
        public APIResponse ChangeStatus(int id, string status)
        {
            try
            {
                var result = _realEstateOwnerServices.ChangeStatus(id, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _realEstateOwnerServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật nội dung miêu tả chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-description-content")]
        public APIResponse UpdateProductItemMaterialContent([FromBody] UpdateRstOwnerDescriptionDto input)
        {
            try
            {
                _realEstateOwnerServices.UpdateOwnerDescriptionContent(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
