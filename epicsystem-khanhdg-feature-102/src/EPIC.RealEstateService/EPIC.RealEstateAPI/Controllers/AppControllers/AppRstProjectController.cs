using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using EPIC.DataAccess.Base;
using System.Linq;
using EPIC.RealEstateEntities.Dto.RstProjectFavourite;

namespace EPIC.RealEstateAPI.Controllers.AppControllers
{
    [Route("api/real-estate/investor-project")]
    [ApiController]
    public class AppRstProjectController : BaseController
    {
        private readonly IRstProjectServices _rstProjectServices;
        private readonly IRstProjectFavouriteServices _rstProjectFavouriteServices;

        public AppRstProjectController(ILogger<AppRstProjectController> logger,
            IRstProjectServices rstProjectServices,
            IRstProjectFavouriteServices rstProjectFavouriteServices)
        {
            _logger = logger;
            _rstProjectServices = rstProjectServices;
            _rstProjectFavouriteServices = rstProjectFavouriteServices;
        }

        /// <summary>
        /// Danh sách dự án cho investor
        /// </summary>
        /// <returns></returns>
        [HttpGet("find")]
        [ProducesResponseType(typeof(APIResponse<AppViewListProjectDto>), (int)HttpStatusCode.OK)]
        public APIResponse AppPojectGetAll([FromQuery] AppFindProjectDto dto)
        {
            try
            {
                var data = _rstProjectServices.AppFindProjects(dto);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy các tham số để tạo bộ lọc cho app lọc dự án
        /// </summary>
        /// <returns></returns>
        [HttpGet("init-filter-find-project")]
        [ProducesResponseType(typeof(APIResponse<AppGetParamsFindProjectDto>), (int)HttpStatusCode.OK)]
        public APIResponse AppInitFilterFindProject(bool isSaleView)
        {
            try
            {
                var data = _rstProjectServices.AppGetParamsFindProjects(isSaleView);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tổng quan dự án
        /// </summary>
        [ProducesResponseType(typeof(APIResponse<AppDetailProjectDto>), (int)HttpStatusCode.OK)]
        [HttpGet("project-detail/{openSellId}")]
        public APIResponse AppProjectDetail(int openSellId)
        {
            try
            {
                var data = _rstProjectServices.AppGetDetailProject(openSellId);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy list media cho dự án
        /// </summary>
        [ProducesResponseType(typeof(APIResponse<List<AppViewProjectMediaDto>>), (int)HttpStatusCode.OK)]
        [HttpGet("media/{projectId}")]
        public APIResponse AppProjectMedia(int projectId)
        {
            try
            {
                var data = _rstProjectServices.AppGetAllMedia(projectId);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm dự án yêu thích
        /// </summary>
        [ProducesResponseType(typeof(APIResponse<List<RstProjectFavouriteDto>>), (int)HttpStatusCode.OK)]
        [HttpPost("project-favourite/add")]
        public APIResponse AppAddProjectFavourite(CreateRstProjectFavouriteDto input)
        {
            try
            {
                var data = _rstProjectFavouriteServices.AppAddProjectFavourite(input);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa dự án yêu thích
        /// </summary>
        [HttpDelete("project-favourite/delete/{openSellId}")]
        public APIResponse AppDeleteProjectFavourite(int openSellId)
        {
            try
            {
                _rstProjectFavouriteServices.AppDeleteProjectFavourite(openSellId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
