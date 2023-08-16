using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProducItemDesignDiagramFile;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProductItemMaterialFile;
using EPIC.RealEstateEntities.Dto.RstProductItemMedia;
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

namespace EPIC.RealEstateAPI.Controllers.AppControllers
{
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/real-estate/investor-product-item")]
    [ApiController]
    public class AppRstProductItemController : BaseController
    {
        private readonly IRstProductItemServices _rstProductItemServices;
        private readonly IRstProjectStructureServices _rstProjectStructureServices;
        private readonly IRstCountServices _rstCountServices;
        private readonly IRstProductItemMaterialFileService _rstProductItemMaterialFileService;
        private readonly IRstProductItemDesignDiagramFileService _rstProductItemDesignDiagramFileService;

        public AppRstProductItemController(ILogger<AppRstProductItemController> logger,
            IRstProductItemServices rstProductItemServices,
            IRstProjectStructureServices rstProjectStructureServices,
            IRstCountServices rstCountServices,
            IRstProductItemMaterialFileService rstProductItemMaterialFileService,
            IRstProductItemDesignDiagramFileService rstProductItemDesignDiagramFileService
            )
        {
            _logger = logger;
            _rstProductItemServices = rstProductItemServices;
            _rstProjectStructureServices = rstProjectStructureServices;
            _rstCountServices = rstCountServices;
            _rstProductItemDesignDiagramFileService = rstProductItemDesignDiagramFileService;
            _rstProductItemMaterialFileService = rstProductItemMaterialFileService;
        }

        /// <summary>
        /// Danh sách sản phẩm của mở bán cho investor
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(APIResponse<List<AppGetAllProductItemDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppPojectGetAll([FromQuery] AppFilterProductItemDto input)
        {
            try
            {
                var data = _rstProductItemServices.AppGetAllProjectItem(input);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Các tham số chuẩn bị cho lọc sản phẩm của mở bán
        /// </summary>
        [ProducesResponseType(typeof(APIResponse<AppGetParamsFindProductItemDto>), (int)HttpStatusCode.OK)]
        [HttpGet("get-params-find/{openSellId}")]
        public APIResponse AppGetParamsFindProductItem(int openSellId)
        {
            try
            {
                var result = _rstProductItemServices.AppGetParamsFindProductItem(openSellId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đếm tổng các sản phẩm của mở bán trong dự án mà đại lý đang mở bán
        /// </summary>
        [ProducesResponseType(typeof(APIResponse<AppCountProductItemSignalRDto>), (int)HttpStatusCode.OK)]
        [HttpGet("count-product-item/{openSellId}")]
        public APIResponse CountProductItemByTrading(int openSellId)
        {
            try
            {
                var result = _rstCountServices.AppCountProductItemByTrading(openSellId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông tin chi tiết của sản phẩm mở bán
        /// </summary>
        [ProducesResponseType(typeof(APIResponse<AppRstProductItemDetailDto>), (int)HttpStatusCode.OK)]
        [HttpGet("detail/{openSellDetailId}")]
        public APIResponse AppProductItemDetail(int openSellDetailId)
        {
            try
            {
                var result = _rstProductItemServices.AppProductItemDetail(openSellDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông tin chi tiết của sản phẩm mở bán
        /// </summary>
        [ProducesResponseType(typeof(APIResponse<List<AppRstProductItemMediaDto>>), (int)HttpStatusCode.OK)]
        [HttpGet("detail/media/{openSellDetailId}")]
        public APIResponse AppProductItemDetailMedia(int openSellDetailId)
        {
            try
            {
                var result = _rstProductItemServices.AppProductItemDetailMedia(openSellDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm các thông tin sản phẩm tương tự trong cùng dự án
        /// </summary>
        [ProducesResponseType(typeof(APIResponse<AppRstProductItemSimilarDto>), (int)HttpStatusCode.OK)]
        [HttpGet("detail/similar/{openSellDetailId}")]
        public APIResponse OpenSellDetailSimilar(int openSellDetailId)
        {
            try
            {
                var result = _rstProductItemServices.OpenSellDetailSimilar(openSellDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// get danh sách file vật liệu
        /// </summary>
        /// <param name="productItemId">Id căn hộ</param>
        /// <returns></returns>
        [HttpGet("get-material-file/{productItemId}")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<RstProductItemMaterialFileDto>>), (int)HttpStatusCode.OK)]

        public APIResponse GetAllMaterialFile(int productItemId)
        {
            try
            {
                var result = _rstProductItemMaterialFileService.FindAll(productItemId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// get danh sách file sơ đồ thiết kế
        /// </summary>
        /// <param name="productItemId">Id căn hộ</param>
        /// <returns></returns>
        [HttpGet("get-design-diagram-file/{productItemId}")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<RstProductItemDesignDiagramFileDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllDesignDiagramFile(int productItemId)
        {
            try
            {
                var result = _rstProductItemDesignDiagramFileService.GetAll(productItemId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
