using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerEntities.Dto.GarnerProduct;
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
    [Route("api/garner/product")]
    [ApiController]
    public class GarnerProductController : BaseController
    {
        private readonly IGarnerProductServices _garnerProductServices;

        public GarnerProductController(ILogger<GarnerProductController> logger,
            IGarnerProductServices garnerProductServices)
        {
            _logger = logger;
            _garnerProductServices = garnerProductServices;
        }

        /// <summary>
        /// lấy danh sách dự án
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.GarnerSPDT_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterGarnerProductDto input)
        {
            try
            {
                var result = _garnerProductServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// lấy danh sách tổ chức phát hành không trùng nhau
        /// </summary>
        /// <returns></returns>
        [HttpGet("distinct-issuer")]
        public APIResponse DistinctIssuers([FromQuery] FilterGarnerProductDto input)
        {
            try
            {
                var result = _garnerProductServices.DistinctIssuer(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// lấy list Product File theo productId
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all-product-file/{productId}")]
        public APIResponse FindAllListByProduct(int productId, int? documentType)
        {
            try
            {
                var result = _garnerProductServices.FindAllListByProduct(productId, documentType);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thay đổi trạng thái status đóng và hoạt động 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPut("change-status/{productId}")]
        [PermissionFilter(Permissions.GarnerSPDT_DongOrMo)]
        public APIResponse ChangeStatus(int productId)
        {
            try
            {
                _garnerProductServices.ChangeStatus(productId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// update ProductFile
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-product-file")]
        public APIResponse UpdateProductFile([FromBody] CreateGarnerProductFileDto input)
        {
            try
            {
                _garnerProductServices.UpdateProductFile(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thêm mới product file
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-product-file/{productId}")]
        public APIResponse AddProductFile(int productId, CreateGarnerProductFileDto input)
        {
            try
            {
                _garnerProductServices.AddProductFile(productId, input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa product file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-product-file/{id}")]
        public APIResponse DeletedProductFile(int id)
        {
            try
            {
                _garnerProductServices.DeletedProductFile(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm thông tin dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        [PermissionFilter(Permissions.GarnerSPDT_EpicXacMinh)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _garnerProductServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm tích lũy theo đại lý
        /// </summary>
        /// <returns></returns>
        [HttpGet("list-product-by-trading")]
        [ProducesResponseType(typeof(APIResponse<List<GarnerProductByTradingProviderDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetListProductByTradingProvider()
        {
            try
            {
                var result = _garnerProductServices.GetListProductByTradingProvider();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.GarnerSPDT_ThemMoi)]
        public APIResponse Add([FromBody] CreateGarnerProductDto input)
        {
            try
            {
                 _garnerProductServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse Update([FromBody] UpdateGarnerProductDto input)
        {
            try
            {
                 _garnerProductServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa sản phẩm tích lũy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="summary"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public APIResponse Delete(int id, string summary)
        {
            try
            {
                _garnerProductServices.Delete(id, summary?.Trim());
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo yêu cầu trình duyệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("request")]
        [PermissionFilter(Permissions.GarnerSPDT_TrinhDuyet)]
        public APIResponse RequestProduct(CreateGarnerRequestDto input)
        {
            try
            {
                _garnerProductServices.Request(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("approve")]
        [PermissionFilter(Permissions.GarnerPDSPTL_PheDuyetOrHuy)]
        public APIResponse ApproveProduct(GarnerApproveDto input)
        {
            try
            {
                _garnerProductServices.Approve(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("cancel")]
        [PermissionFilter(Permissions.GarnerPDSPTL_PheDuyetOrHuy)]
        public APIResponse CancelProduct(GarnerCancelDto input)
        {
            try
            {
                _garnerProductServices.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Check sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("check")]
        [PermissionFilter(Permissions.GarnerSPDT_EpicXacMinh)]
        public APIResponse CheckProduct(GarnerCheckDto input)
        {
            try
            {
                _garnerProductServices.Check(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm lịch sử update sản phẩm phân phối đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-history-update-by-id")]
        public APIResponse FindHistoryUpdateById(int id)
        {
            try
            {
                var result = _garnerProductServices.FindHistoryUpdateById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
