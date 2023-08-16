using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
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

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/product-trading-provider")]
    [ApiController]
    public class GarnerProductTradingProviderController : BaseController
    {
        private readonly IGarnerProductTradingProviderServices _garnerProductTradingProviderServices;

        public GarnerProductTradingProviderController(ILogger<GarnerProductTradingProviderController> logger,
            IGarnerProductTradingProviderServices garnerProductTradingProviderServices)
        {
            _logger = logger;
            _garnerProductTradingProviderServices = garnerProductTradingProviderServices;
        }

        /// <summary>
        /// lấy danh sách đại lý tham gia theo sản phẩm tích lũy
        /// </summary>
        /// <param name="productId">Id sản phẩm tích lũy</param>
        /// <returns></returns>
        [HttpGet("find-all-by-product")]
        [PermissionFilter(Permissions.GarnerSPDT_ChiTiet)]
        public APIResponse FindAllByProduct(int productId)
        {
            try
            {
                var result = _garnerProductTradingProviderServices.FindAllByProduct(productId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết đại lý của sản phẩm tích lũy
        /// </summary>
        /// <param name="id">id của bảng</param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _garnerProductTradingProviderServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm đại lý vào sản phẩm tích lũy
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.GarnerSPDT_DLPP_ThemMoi)]

        public APIResponse Add([FromBody] CreateGarnerProductTradingProviderDto input)
        {
            try
            {
                _garnerProductTradingProviderServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin của đại lý trong sản phẩm tích lũy
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.GarnerSPDT_DLPP_CapNhat)]

        public APIResponse Update([FromBody] UpdateGarnerProductTradingProviderDto input)
        {
            try
            {
                _garnerProductTradingProviderServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách ngân hàng theo DLSC đang đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet("list-bank")]
        [ProducesResponseType(typeof(APIResponse<List<BusinessCustomerBankDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindBankByTrading(int? distributionId = null, int? type = null)
        {
            try
            {
                var result = _garnerProductTradingProviderServices.FindBankByTrading(distributionId, type);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
