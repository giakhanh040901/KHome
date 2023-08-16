using EPIC.InvestDomain;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.Owner;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/owner")]
    [ApiController]
    public class OwnerController : BaseController
    {
        private IOwnerServices _ownerServices;
        public OwnerController(ILogger<OwnerController> logger, IOwnerServices invOwnerServices)
        {
            _logger = logger;
            _ownerServices = invOwnerServices;
        }

        /// <summary>
        /// Lấy danh sách chủ đầu tư
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("find")]
        [PermissionFilter(Permissions.InvestMenuChuDT)]
        public APIResponse GetAll(int pageNumber, int? pageSize, string keyword, int? status)
        {
            try
            {
                var result = _ownerServices.FindAll(pageSize ?? 100, pageNumber , keyword?.Trim(), status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [PermissionFilter(Permissions.InvestChuDT_ChiTiet)]
        public APIResponse Get(int id)
        {
            try
            {
                var result = _ownerServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mới chủ đầu tư
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.InvestChuDT_ThemMoi)]
        public APIResponse Add([FromBody] CreateOwnerDto body)
        {
            try
            {
                var result = _ownerServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.InvestChuDT_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _ownerServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        [PermissionFilter(Permissions.InvestChuDT_CapNhat)]
        public APIResponse Update(int id, [FromBody] UpdateOwnerDto body)
        {
            try
            {
                var result = _ownerServices.Update(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
