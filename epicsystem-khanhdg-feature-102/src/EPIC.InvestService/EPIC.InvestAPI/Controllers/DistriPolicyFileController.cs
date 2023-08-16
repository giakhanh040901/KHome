using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.DistriPolicyFile;
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
    [Route("api/invest/distri-policy-file")]
    [ApiController]
    public class DistriPolicyFileController : BaseController
    {
        private IDistriPolicyFileServices _distriPolicyFileServices;

        public DistriPolicyFileController(ILogger<DistriPolicyFileController> logger, IDistriPolicyFileServices distriPolicyFileServices)
        {
            _logger = logger;
            _distriPolicyFileServices = distriPolicyFileServices;
        }

        /// <summary>
        /// Thêm file chính sách
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        [PermissionFilter(Permissions.InvestPPDT_FileChinhSach_ThemMoi)]
        public APIResponse AddDistriPolicyFile([FromBody] CreateDistriPolicyFileDto body)
        {
            try
            {
                var result = _distriPolicyFileServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);

            }
        }

        /// <summary>
        /// Lấy danh sách file chính sách
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Route("fileAll/find")]
        [HttpGet]
        [PermissionFilter(Permissions.InvestPPDT_FileChinhSach_DanhSach)]
        public APIResponse GetAllDistriPolicyFiles(int distributionId, int pageSize, int pageNumber, string keyword)
        {
            try
            {
                var result = _distriPolicyFileServices.FindAllDistriPolicyFile(distributionId, pageSize, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem thông tin chi tiết file chính sách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("find/{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.InvestPPDT_FileChinhSach_DanhSach)]
        public APIResponse GetDistriPolicyFileById(int id)
        {
            try
            {
                var result = _distriPolicyFileServices.FindDistriPolicyFileById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá file chính sách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.InvestPPDT_FileChinhSach_Xoa)]
        public APIResponse DeleteDistriPolicyFile(int id)
        {
            try
            {
                var result = _distriPolicyFileServices.DeleteDistriPolicyFile(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sửa file chính sách
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("update/{id}")]
        [HttpPut]
        [PermissionFilter(Permissions.InvestPPDT_FileChinhSach_CapNhat)]
        public APIResponse DistriPolicyFileUpdate(int id, [FromBody] UpdateDistriPolicyFileDto body)
        {
            try
            {
                var result = _distriPolicyFileServices.DistriPolicyFileUpdate(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
