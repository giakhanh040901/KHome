using EPIC.DataAccess.Models;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.DistributionFile;
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
    [Route("api/invest/distribution-file")]
    [ApiController]
    public class DistributionFileController : BaseController
    {
        private readonly IDistributionFileService _distributionFileService;
        public DistributionFileController(ILogger<DistributionFileController> logger, IDistributionFileService distributionFileService)
        {
            _logger = logger;
            _distributionFileService = distributionFileService;
        }

        /// <summary>
        /// hiển thị tất cả dữ liệu trong bảng file phân phối đầu tư
        /// </summary>
        /// <returns></returns>
        [HttpGet("find")]
        [PermissionFilter(Permissions.InvestPPDT_HopDongPhanPhoi_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<PagingResult<DistributionFileDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll(int distributionId,int pageNumber, int? pageSize)
        {
            try
            {
                var result = _distributionFileService.FindAll(distributionId, pageSize ?? 100, pageNumber);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết file phân phối đầu tư id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [PermissionFilter(Permissions.InvestPPDT_HopDongPhanPhoi_DanhSach)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _distributionFileService.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm file phân phối đầu tư
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.InvestPPDT_HopDongPhanPhoi_ThemMoi)]
        public APIResponse AddDistributionFile([FromBody] CreateDistributionFileDto body)
        {
            try
            {
                var result = _distributionFileService.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhập file phân phối đầu tư
        /// </summary>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.InvestPPDT_HopDongPhanPhoi_CapNhat)]
        public APIResponse UpdateDistributionFile([FromBody] UpdateDistributionFileDto body)
        {
            try
            {
                _distributionFileService.Update(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Xem chi tiết file phân phối đầu tư id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        [PermissionFilter(Permissions.InvestPPDT_HopDongPhanPhoi_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _distributionFileService.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
