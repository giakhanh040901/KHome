using EPIC.CoreDomain.Implements;
using EPIC.CoreDomain.Interfaces;
using EPIC.Entities.Dto.BusinessLicenseFile;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CoreAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/business-license-file")]
    [ApiController]
    public class BusinessLicenseFileController : BaseController
    {
        private IBusinessLicenseFileServices _businessLicenseFileServices;

        public BusinessLicenseFileController(ILogger<BusinessLicenseFileController> logger, IBusinessLicenseFileServices businessLicenseFileServices)
        {
            _logger = logger;
            _businessLicenseFileServices = businessLicenseFileServices;
        }

        /// <summary>
        /// Thêm mới giấy phép đăng ký kinh doanh
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.CoreKHDN_DKKD_ThemMoi)]
        public APIResponse Add([FromBody] CreateBusinessLicenseFileDto body)
        {
            try
            {
                var result = _businessLicenseFileServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);

            }
        }

        /// <summary>
        /// Thêm mới giấy phép đăng ký kinh doanh vào bảng tạm
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add-temp")]
        [PermissionFilter(Permissions.CoreKHDN_DKKD_ThemMoi)]
        public APIResponse AddTemp([FromBody] CreateBusinessLicenseFileTempDto body)
        {
            try
            {
                var result = _businessLicenseFileServices.AddTemp(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);

            }
        }

        /// <summary>
        /// Lấy danh sách giấy phép đăng ký kinh doanh theo doanh nghiệp
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <returns></returns>
        [HttpGet("find")]
        [PermissionFilter(Permissions.CoreKHDN_DKKD)]
        public APIResponse FindAll(int businessCustomerId)
        {
            try
            {
                var result = _businessLicenseFileServices.FindAll(businessCustomerId, null);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách giấy phép đăng ký kinh doanh theo doanh nghiệp, là khách hàng tạm
        /// </summary>
        /// <param name="businessCustomerTempId"></param>
        /// <returns></returns>
        [HttpGet("find-temp")]
        public APIResponse FindAllTemp(int businessCustomerTempId)
        {
            try
            {
                var result = _businessLicenseFileServices.FindAll(null, businessCustomerTempId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết giấy phép đăng ký kinh doanh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _businessLicenseFileServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa giấy phép đăng ký kinh doanh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.CoreKHDN_DKKD_XoaFile)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _businessLicenseFileServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật giấy phép đăng ký kinh doanh
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.CoreKHDN_DKKD_CapNhat)]
        public APIResponse Update([FromBody] UpdateBusinessLicenseFileDto body)
        {
            try
            {
                var result = _businessLicenseFileServices.Update(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật giấy phép đăng ký kinh doanh trong bảng tạm
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update-temp")]
        public APIResponse UpdateTemp([FromBody] UpdateBusinessLicenseFileTempDto body)
        {
            try
            {
                var result = _businessLicenseFileServices.UpdateTemp(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
