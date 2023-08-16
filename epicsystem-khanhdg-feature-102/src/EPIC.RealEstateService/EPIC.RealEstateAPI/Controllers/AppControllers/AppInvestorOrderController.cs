using EPIC.RealEstateDomain.Interfaces;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using EPIC.Utils.ConstantVariables.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstCart;
using EPIC.RealEstateEntities.Dto.RstOrder;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.RealEstateEntities.Dto.RstRating;

namespace EPIC.RealEstateAPI.Controllers.AppControllers
{
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/real-estate/investor-order")]
    [ApiController]
    public class AppInvestorOrderController : BaseController
    {
        private readonly IRstProjectServices _rstProjectServices;
        private readonly IRstCartServices _rstCartServices;
        private readonly IRstOrderServices _rstOrderServices;
        private readonly IRstRatingServices _rstRatingServices;

        public AppInvestorOrderController(ILogger<AppInvestorOrderController> logger,
            IRstProjectServices rstProjectServices,
            IRstOrderServices rstOrderServices,
            IRstCartServices rstCartServices,
            IRstRatingServices rstRatingServices)
        {
            _logger = logger;
            _rstProjectServices = rstProjectServices;
            _rstCartServices = rstCartServices;
            _rstOrderServices = rstOrderServices;
            _rstRatingServices = rstRatingServices;
        }
        #region Giỏ hàng
        /// <summary>
        /// Giỏ hàng của investor
        /// </summary>
        /// <returns></returns>
        [HttpGet("cart")]
        [ProducesResponseType(typeof(APIResponse<List<AppRstCartDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppCartGetAll()
        {
            try
            {
                var result = _rstCartServices.GetAllCart();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm item trong giỏ hàng
        /// </summary>
        [HttpPost("cart/add")]
        public APIResponse AppCartItemAdd(CreateRstCartDto input)
        {
            try
            {
                _rstCartServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa item trong giỏ hàng
        /// </summary>
        [HttpDelete("cart/delete/{id}")]
        public APIResponse AppCartItemDelete(int id)
        {
            try
            {
                _rstCartServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Thêm giao dịch
        /// <summary>
        /// Kiểm tra sản phẩm của mở bán trước khi đặt lệnh APP
        /// </summary>
        /// <returns></returns>
        [HttpPost("check-open-sell-detail")]
        public APIResponse CheckOpenSellDetailBeforeOrder(int openSellDetailId)
        {
            try
            {
                _rstOrderServices.CheckOpenSellDetailBeforeOrder(openSellDetailId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Kiểm tra giao dịch
        /// </summary>
        /// <returns></returns>
        [HttpPost("order-check")]
        public APIResponse AppOrderCheck([FromForm] AppCreateRstOrderCheckDto input)
        {
            try
            {
                _rstOrderServices.CheckOrder(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm giao dịch
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse<AppRstOrderDataSuccessDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> InvestorOrderAdd([FromForm] AppCreateRstOrderDto input)
        {
            try
            {
                var result = await _rstOrderServices.InvestorOrderAdd(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sale Đặt lệnh hộ investor
        /// </summary>
        [HttpPost("sale/add")]
        public async Task<APIResponse> SaleInvestorOrderAdd([FromForm]AppSaleCreateRstOrderDto input)
        {
            try
            {
                var result = await _rstOrderServices.SaleInvestorOrderAdd(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// App hủy lệnh trọng trạng thái chờ thanh toán
        /// </summary>
        [HttpDelete("delete/{orderId}")]
        public APIResponse AppDeleteOrder(int orderId)
        {
            try
            {
                _rstOrderServices.AppDeleteOrder(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gia hạn hợp đồng trên App (gia hạn thời gian giữ chỗ)
        /// Khi hợp đồng hết thời gian giữ chỗ mà Căn hộ vẫn đang mở bán Status = 1
        /// </summary>
        [HttpPut("extended-keep-time/{orderId}")]
        public APIResponse AppExtendedKeepTime(int orderId)
        {
            try
            {
                _rstOrderServices.AppExtendedKeepTime(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion

        #region Quản lý hợp đồng
        /// <summary>
        /// Tìm kiếm sale theo mã gioi thieu cua sale và căn đang chọn
        /// </summary>
        [HttpGet("order/find-referral-code")]
        [ProducesResponseType(typeof(APIResponse<AppRstOrderDetailDto>), (int)HttpStatusCode.OK)]
        public APIResponse AppSaleOrderFindReferralCode(string referralCode, int openSellDetailId)
        {
            try
            {
                var result = _rstOrderServices.AppSaleOrderFindReferralCode(referralCode, openSellDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy lịch sử đang sở hữu của nhà đầu tư cá nhân
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-investing")]
        [ProducesResponseType(typeof(APIResponse<List<AppRstOrderDto>>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> AppInvestorOrderInvesting([FromQuery] AppRstOrderFilterDto input)
        {
            try
            {
                var result = await _rstOrderServices.AppGetAllOrder(input, RstAppOrderGroupStatus.DANG_SO_HUU);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy lịch sử đang giao dịch (Chờ duyệt hợp đồng, đang giao dịch đặt cọc...)
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-trading")]
        [ProducesResponseType(typeof(APIResponse<List<AppRstOrderDto>>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> AppInvestorOrderTrading([FromQuery] AppRstOrderFilterDto input)
        {
            try
            {
                var result = await _rstOrderServices.AppGetAllOrder(input, RstAppOrderGroupStatus.DANG_GIAO_DICH);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy lịch sử sổ lệnh đang trong thời gian giao dịch chờ thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-order")]
        [ProducesResponseType(typeof(APIResponse<List<AppRstOrderDto>>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> AppInvestorOrder([FromQuery] AppRstOrderFilterDto input)
        {
            try
            {
                var result = await _rstOrderServices.AppGetAllOrder(input, RstAppOrderGroupStatus.SO_LENH);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// App xem chi tiết thông tin lệnh
        /// </summary>
        [HttpGet("find-by-id/{orderId}")]
        [ProducesResponseType(typeof(APIResponse<AppRstOrderDetailDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> AppOrderDetail(int orderId)
        {
            try
            {
                var result = await _rstOrderServices.AppOrderDetail(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Đánh giá
        /// <summary>
        /// Thêm đánh giá
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("rating/add")]
        public APIResponse AddRating(CreateRstRatingDto input)
        {
            try
            {
                _rstRatingServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách ratting của dự án
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("rating/view-ratting")]
        public APIResponse ViewRstRating(int projectId)
        {
            try
            {
                var result = _rstRatingServices.ViewRstRating(projectId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm hợp đồng mới nhất để đánh giá
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("rating/find-last-order")]
        public APIResponse FindLastOrder()
        {
            try
            {
                var result = _rstRatingServices.FindLastOrder();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
    }
}
