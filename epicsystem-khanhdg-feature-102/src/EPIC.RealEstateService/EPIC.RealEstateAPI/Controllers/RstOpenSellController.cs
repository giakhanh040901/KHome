using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.RealEstateEntities.Dto.RstDistribution;
using EPIC.RealEstateEntities.Dto.RstDistributionProductItem;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System;
using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.RealEstateEntities.Dto.RstOpenSellDetail;
using EPIC.RealEstateEntities.Dto.RstOpenSellBank;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.RealEstateEntities.Dto.RstProductItem;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/open-sell")]
    [ApiController]
    public class RstOpenSellController : BaseController
    {
        private readonly IRstOpenSellServices _rstOpenSellServices;
        public RstOpenSellController(ILogger<RstOpenSellController> logger,
            IRstOpenSellServices rstOpenSellServices)
        {
            _logger = logger;
            _rstOpenSellServices = rstOpenSellServices;
        }

        #region Mo ban
        /// <summary>
        /// Danh sách mở bán của đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateMoBan_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<RstOpenSellDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterRstOpenSellDto input)
        {
            try
            {
                var result = _rstOpenSellServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách tài khoản ngân hàng có thể cài cho mở bán
        /// </summary>
        [HttpGet("bank-account-can-distribution/{projectId}")]
        [ProducesResponseType(typeof(APIResponse<List<BankAccountDtoForOpenSell>>), (int)HttpStatusCode.OK)]
        public APIResponse BankAccountCanDistributionOpenSell(int projectId, int? bankType)
        {
            try
            {
                var result = _rstOpenSellServices.BankAccountCanDistributionOpenSell(projectId, bankType);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiet mo ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(APIResponse<RstOpenSellDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id) 
        {
            try
            {
                var result = _rstOpenSellServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm Mở bán
        /// </summary>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateMoBan_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstOpenSellDto input)
        {
            try
            {
                var result = _rstOpenSellServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật Mở bán
        /// </summary>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateMoBan_ThongTinChung_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstOpenSellDto input)
        {
            try
            {
                _rstOpenSellServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạm dừng Mở bán (Tạm dừng - Đang bán)
        /// </summary>
        [HttpPut("pause/{id}")]
        public APIResponse PauseOpenSell(int id)
        {
            try
            {
                _rstOpenSellServices.PauseOpenSell(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Bật tắt show App của mở bán
        /// </summary>
        [HttpPut("show-app/{id}")]
        [PermissionFilter(Permissions.RealStateMoBan_DoiShowApp)]
        public APIResponse IsShowAppOpenSell(int id)
        {
            try
            {
                _rstOpenSellServices.IsShowAppOpenSell(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Dừng mở bán (dừng mở bán không được mở lại)
        /// </summary>
        [HttpPut("stop/{id}")]
        [PermissionFilter(Permissions.RealStateMoBan_DungBan)]
        public APIResponse StopOpenSell(int id)
        {
            try
            {
                _rstOpenSellServices.StopOpenSell(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa mở bán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        [PermissionFilter(Permissions.RealStateMoBan_Xoa)]
        public APIResponse DeleteOpenSell(int id)
        {
            try
            {
                _rstOpenSellServices.DeleteOpenSell(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái nổi bật: Y/N
        /// </summary>
        [HttpPut("outstanding/{id}")]
        [PermissionFilter(Permissions.RealStateMoBan_DoiNoiBat)]
        public APIResponse ChangeOutstanding(int id)
        {
            try
            {
                var result = _rstOpenSellServices.ChangeOutstanding(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Sản phẩm của Mở bán
        /// <summary>
        /// Thêm sản phẩm
        /// </summary>
        [HttpPost("detail/add")]
        [PermissionFilter(Permissions.RealStateMoBan_DSSP_Them)]
        public APIResponse AddOpenSellDetail([FromBody] CreateRstOpenSellDetailDto input)
        {
            try
            {
                _rstOpenSellServices.AddOpenSellDetail(input);
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
        [HttpPut("detail/delete")]
        [PermissionFilter(Permissions.RealStateMoBan_DSSP_Xoa)]
        public APIResponse DeleteOpenSellDetail(List<int> openSellDetailIds)
        {
            try
            {
                _rstOpenSellServices.DeleteOpenSellDetail(openSellDetailIds);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Bật tắt show App của sảm phẩm mở bán
        /// </summary>
        [HttpPut("detail/show-app/{id}")]
        [PermissionFilter(Permissions.RealStateMoBan_DSSP_DoiShowApp)]
        public APIResponse IsShowAppOpenSellDetail(int id)
        {
            try
            {
                _rstOpenSellServices.IsShowAppOpenSellDetail(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết căn hộ của sản phẩm
        /// </summary>
        [HttpGet("detail/find-product-item/{openSellDetailId}")]
        public APIResponse ProductItemByOpenSellDetail(int openSellDetailId)
        {
            try
            {
                var result = _rstOpenSellServices.ProductItemByOpenSellDetail(openSellDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Ẩn giá căn
        /// </summary>
        [HttpPut("detail/hide-price")]
        [PermissionFilter(Permissions.RealStateMoBan_DSSP_DoiShowPrice)]
        public APIResponse HideOpenSellDetail(RstOpenSellDetailHidePriceDto input)
        {
            try
            {
                _rstOpenSellServices.HideOpenSellDetail(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hiện giá căn
        /// </summary>
        [HttpPut("detail/show-price/{openSellDetailId}")]
        [PermissionFilter(Permissions.RealStateMoBan_DSSP_DoiShowPrice)]
        public APIResponse ShowOpenSellDetail(int openSellDetailId)
        {
            try
            {
                _rstOpenSellServices.ShowOpenSellDetail(openSellDetailId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Khóa căn hộ ở sản phẩm mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("detail/lock")]
        public APIResponse LockOpenSellDetail(LockRstOpenSellDetailDto input)
        {
            try
            {
                _rstOpenSellServices.LockOpenSellDetail(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Danh sách căn của mở bán
        /// </summary>
        [HttpGet("detail")]
        [ProducesResponseType(typeof(APIResponse<List<RstOpenSellDetailDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllItemByDistribution([FromQuery] FilterRstOpenSellDetailDto input)
        {
            try
            {
                var result = _rstOpenSellServices.FindAllOpenSellDetail(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông tin hợp đồng mới nhất được tạo trong mở bán
        /// </summary>
        [HttpGet("order-latest/{openSellId}")]
        [ProducesResponseType(typeof(APIResponse<InfoOrderNewInProjectDto>), (int)HttpStatusCode.OK)]
        public APIResponse InfoOrderNewInOpenSell(int openSellId)
        {
            try
            {
                var result = _rstOpenSellServices.InfoOrderNewInOpenSell(openSellId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách mở bán của đại lý có thể đặt lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("get-all-for-order")]
        [ProducesResponseType(typeof(APIResponse<List<RstOpenSellDetailDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllOpenSellDetailForOrder([FromQuery] FilterRstOpenSellDetailForOrder input)
        {
            try
            {
                var result = _rstOpenSellServices.GetAllOpenSellDetailForOrder(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách list sản phẩm của đại lý bán hộ cho đại lý khác
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-list-ban-ho")]
        public APIResponse FindListOpenSellBanHo()
        {
            try
            {
                var result = _rstOpenSellServices.FindOpenSellBanHoByTrading();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion

        #region Duyệt Mở bán
        /// <summary>
        /// Yêu cầu trình duyệt Mở bán
        /// </summary>
        [HttpPut("request")]
        [PermissionFilter(Permissions.RealStateMoBan_ThongTinChung_TrinhDuyet)]
        public APIResponse ProjectRequest([FromBody] RstRequestDto input)
        {
            try
            {
                _rstOpenSellServices.Request(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt mở bán
        /// </summary>
        [HttpPut("approve")]
        [PermissionFilter(Permissions.RealStateMoBan_ThongTinChung_PheDuyet)]
        public APIResponse ProjectApprove([FromBody] RstApproveDto input)
        {
            try
            {
                _rstOpenSellServices.Approve(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt
        /// </summary>
        [HttpPut("cancel")]
        [PermissionFilter(Permissions.RealStateMoBan_ThongTinChung_PheDuyet)]
        public APIResponse ProjectCancel([FromBody] RstCancelDto input)
        {
            try
            {
                _rstOpenSellServices.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
    }
}
