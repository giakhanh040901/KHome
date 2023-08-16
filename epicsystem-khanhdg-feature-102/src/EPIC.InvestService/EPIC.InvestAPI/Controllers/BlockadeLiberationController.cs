using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.BlockadeLiberation;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/blockade-liberation")]
    [ApiController]
    public class BlockadeLiberationController : BaseController
    {
        private readonly IBlockadeLiberationServices _blockadeLiberationServices;
        private readonly IConfiguration _configuration;

        public BlockadeLiberationController(
            ILogger<BlockadeLiberationController> logger,
            IBlockadeLiberationServices blockadeLiberationServices,
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
        [PermissionFilter(Permissions.InvestHopDong_PhongToaGiaiToa_DanhSach)]

        public APIResponse FindAllBlockadeLiberation([FromQuery] FilterBlockadeLiberationDto input)
        {
            try
            {
                var result = _blockadeLiberationServices.FindAll(input);
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
        [PermissionFilter(Permissions.InvestHDPP_HopDong_PhongToaHopDong)]
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
        [PermissionFilter(Permissions.InvestHopDong_PhongToaGiaiToa_GiaiToaHD)]
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
