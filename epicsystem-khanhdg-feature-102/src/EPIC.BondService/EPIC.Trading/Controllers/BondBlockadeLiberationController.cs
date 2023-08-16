using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.BlockadeLiberation;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/blockade-liberation")]
    [ApiController]
    public class BondBlockadeLiberationController : BaseController
    {
        private readonly IBondBlockadeLiberationService _blockadeLiberationServices;
        private readonly IConfiguration _configuration;

        public BondBlockadeLiberationController(
            ILogger<BondBlockadeLiberationController> logger,
            IBondBlockadeLiberationService blockadeLiberationServices,
            IConfiguration configuration)
        {
            _logger = logger;
            _blockadeLiberationServices = blockadeLiberationServices;
            _configuration = configuration;
        }

        /// <summary>
        /// Tìm kiếm phong toả giải toả
        /// </summary>
        /// <returns></returns>
        [Route("find")]
        [HttpGet]
        [PermissionFilter(Permissions.BondHDPP_PTGT_DanhSach)]

        public APIResponse FindAllBlockadeLiberation(int? pageSize, int pageNumber, string keyword, int? status, int? type)
        {
            try
            {
                var result = _blockadeLiberationServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), status, type);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm phong toả giải toả theo id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("find/{id}")]
        [HttpGet]
        // khong thay su dung (8/9/22)
        public APIResponse GetBlockadeLiberation(int id)
        {
            try
            {
                var result = _blockadeLiberationServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo phong toả
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        [PermissionFilter(Permissions.BondHDPP_HopDong_PhongToaHopDong)]
        public APIResponse AddBlockadeLiberation([FromBody] CreateBlockadeLiberationDto body)
        {
            try
            {
                var result = _blockadeLiberationServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật phong toả giải toả
        /// </summary>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        [PermissionFilter(Permissions.BondHDPP_PTGT_GiaiToaHopDong)]
        public APIResponse UpdateOrder([FromBody] UpdateBlockadeLiberationDto body, int id)
        {
            try
            {
                var result = _blockadeLiberationServices.Update(body, id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
