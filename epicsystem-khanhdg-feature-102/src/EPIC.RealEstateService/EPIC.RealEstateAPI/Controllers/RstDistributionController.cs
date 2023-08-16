using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicyTemp;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System;
using EPIC.RealEstateEntities.Dto.RstDistribution;
using EPIC.RealEstateEntities.Dto.RstDistributionProductItem;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/distribution")]
    [ApiController]
    public class RstDistributionController : BaseController
    {
        private readonly IRstDistributionServices _rstDistributionServices;
        public RstDistributionController(ILogger<RstDistributionController> logger,
            IRstDistributionServices rstDistributionServices)
        {
            _logger = logger;
            _rstDistributionServices = rstDistributionServices;
        }

        #region Phân phối
        /// <summary>
        /// Danh sách phân phối cho đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<RstDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterRstDistributionDto input)
        {
            try
            {
                var result = _rstDistributionServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiet phan phoi
        /// </summary>
        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(APIResponse<List<RstDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstDistributionServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm phân phối
        /// </summary>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstDistributionDto input)
        {
            try
            {
                var result = _rstDistributionServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật phân phối
        /// </summary>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateMenuPhanPhoi_ThongTinChung_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstDistributionDto input)
        {
            try
            {
                _rstDistributionServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạm dừng phân phối (Đang phân phối - Tạm dừng)
        /// </summary>
        [HttpPut("pause/{id}")]
        public APIResponse PauseDistribution(int id)
        {
            try
            {
                _rstDistributionServices.PauseDistribution(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa phân phối
        /// </summary>
        [HttpPut("delete/{id}")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_Xoa)]
        public APIResponse DeleteDistribution(int id)
        {
            try
            {
                _rstDistributionServices.DeleteDistribution(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion

        #region Sản phẩm của phân phối
        /// <summary>
        /// Thêm sản phẩm
        /// </summary>
        [HttpPost("distribution-product-item/add")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_DSSP_ThemMoi)]
        public APIResponse AddDistributionProductItem([FromBody] CreateRstDistributionProductItemDto input)
        {
            try
            {
                _rstDistributionServices.AddDistributionProductItem(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        [HttpPut("distribution-product-item/delete")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_DSSP_Xoa)]
        public APIResponse DeleteDistributionProductItem([FromBody] DeleteRstDistributionProductDto input)
        {
            try
            {
                _rstDistributionServices.DeleteDistributionProductItem(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết sản phẩm
        /// </summary>
        [HttpGet("distribution-product-item/find/{distributionProductItemId}")]
        public APIResponse FindDistributionItemById(int distributionProductItemId)
        {
            try
            {
                var result = _rstDistributionServices.FindDistributionItemById(distributionProductItemId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái khóa căn
        /// </summary>
        [HttpPut("distribution-product-item/lock")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_DSSP_DoiTrangThai)]
        public APIResponse LockDistributionProductItem(LockRstDistributionProductItemDto input)
        {
            try
            {
                _rstDistributionServices.LockDistributionProductItem(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách căn của phân phối 
        /// </summary>
        [HttpGet("find-all-item")]
        [PermissionFilter(Permissions.RealStatePhanPhoi_DSSP_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<RstDistributionProductItemDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllItemByDistribution(int distributionId, [FromQuery] FilterRstDistributionProductItemDto input)
        {
            try
            {
                var result = _rstDistributionServices.FindAllItemByDistribution(distributionId, input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
        #region Duyệt phân phối
        /// <summary>
        /// Yêu cầu trình duyệt dự án
        /// </summary>
        [HttpPut("request")]
        [PermissionFilter(Permissions.RealStateMenuPhanPhoi_ThongTinChung_TrinhDuyet)]
        public APIResponse ProjectRequest([FromBody] RstRequestDto input)
        {
            try
            {
                _rstDistributionServices.Request(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt dự án
        /// </summary>
        [HttpPut("approve")]
        [PermissionFilter(Permissions.RealStateMenuPhanPhoi_ThongTinChung_PheDuyet)]
        public APIResponse ProjectApprove([FromBody] RstApproveDto input)
        {
            try
            {
                _rstDistributionServices.Approve(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt dự án
        /// </summary>
        [HttpPut("cancel")]
        [PermissionFilter(Permissions.RealStateMenuPhanPhoi_ThongTinChung_PheDuyet)]
        public APIResponse ProjectCancel([FromBody] RstCancelDto input)
        {
            try
            {
                _rstDistributionServices.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        /// <summary>
        /// Danh sách sản phẩm được phân phối theo dự án cho đại lý
        /// </summary>
        [HttpGet("distribution-item-by-trading")]
        [ProducesResponseType(typeof(APIResponse<List<RstDistributionProductItemDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllProductItemByTrading([FromQuery] FilterRstDistributionProductItemByTradingDto input)
        {
            try
            {
                var result = _rstDistributionServices.GetAllProductItemByTrading(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách sản phẩm được phân phối theo dự án cho đại lý
        /// </summary>
        [HttpGet("distribution-by-trading")]
        [ProducesResponseType(typeof(APIResponse<List<RstDistributionByTradingDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllByTrading()
        {
            try
            {
                var result = _rstDistributionServices.GetAllByTrading();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
