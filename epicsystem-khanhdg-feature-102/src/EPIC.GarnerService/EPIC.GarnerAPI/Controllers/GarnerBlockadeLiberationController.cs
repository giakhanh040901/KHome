using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerBlockadeLiberation;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/blockade-liberation")]
    [ApiController]
    public class GarnerBlockadeLiberationController : BaseController
    {
        private readonly IGarnerBlockadeLiberationServices _garnerBlockadeLiberationServices;
        public GarnerBlockadeLiberationController(ILogger<GarnerBlockadeLiberationController> logger,
            IGarnerBlockadeLiberationServices garnerBlockadeLiberationServices)
        {
            _logger = logger;
            _garnerBlockadeLiberationServices = garnerBlockadeLiberationServices;
        }

        /// <summary>
        /// Tìm kiếm phong toả giải toả không phân trang
        /// </summary>
        /// <returns></returns>
        [Route("find")]
        [HttpGet]
        [PermissionFilter(Permissions.GarnerHDPP_HopDong_PhongToaHopDong)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse FindAll()
        {
            try
            {
                var result = _garnerBlockadeLiberationServices.FindAll();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm phong toả giải toả có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.GarnerHopDong_PhongToaGiaiToa_DanhSach)]
        public APIResponse FindAllBlockadeLiberation([FromQuery] FilterGarnerOrderDto input)
        {
            try
            {
                var result = _garnerBlockadeLiberationServices.FindAllBlockadeLiberation(input, new int[] { OrderStatus.PHONG_TOA, OrderStatus.GIAI_TOA });
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
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(APIResponse<GarnerBlockadeLiberationDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.GarnerHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _garnerBlockadeLiberationServices.FindById(id);
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
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        [PermissionFilter(Permissions.GarnerHDPP_HopDong_PhongToaHopDong)]
        public APIResponse AddBlockadeLiberation([FromBody] CreateGarnerBlockadeLiberationDto input)
        {
            try
            {
                var result = _garnerBlockadeLiberationServices.Add(input);
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
        [PermissionFilter(Permissions.GarnerHopDong_PhongToaGiaiToa_GiaiToaHD)]
        public APIResponse UpdateOrder([FromBody] UpdateGarnerBlockadeLiberationDto input)
        {
            try
            {
                var result = _garnerBlockadeLiberationServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
