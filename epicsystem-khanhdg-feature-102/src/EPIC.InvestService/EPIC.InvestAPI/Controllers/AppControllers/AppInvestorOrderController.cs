using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.InvestRating;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestEntities.Dto.RenewalsRequest;
using EPIC.InvestEntities.Dto.Withdrawal;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Controllers;
using EPIC.Utils.Net.MimeTypes;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.InvestAPI.Controllers.AppControllers
{
    /// <summary>
    /// Nhà đầu tư cá nhân đặt lệnh
    /// </summary>
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/invest/investor-order")]
    [ApiController]
    public class AppInvestorOrderController : BaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly IContractTemplateServices _contractTemplateServices;
        private readonly IContractDataServices _contractDataServices;
        private readonly IConfiguration _configuration;
        private readonly IWithdrawalServices _withdrawalServices;
        private readonly IInvestRenewalsRequestServices _renewalsRequestServices;
        private readonly IInvestRatingServices _investRatingServices;

        public AppInvestorOrderController(
            ILogger<AppInvestorOrderController> logger,
            IOrderServices orderServices,
            IInvestSharedServices investSharedServices,
            IContractTemplateServices contractTemplateServices,
            IWithdrawalServices withdrawalServices,
            IInvestRenewalsRequestServices renewalsRequestServices,
            IContractDataServices contractDataServices,
            IConfiguration configuration,
            IInvestRatingServices investRatingServices)
        {
            _logger = logger;
            _orderServices = orderServices;
            _investSharedServices = investSharedServices;
            _contractTemplateServices = contractTemplateServices;
            _contractDataServices = contractDataServices;
            _configuration = configuration;
            _withdrawalServices = withdrawalServices;
            _renewalsRequestServices = renewalsRequestServices;
            _investRatingServices = investRatingServices;
        }

        #region lưu lệnh trên app
        /// <summary>
        /// Kiểm tra lệnh
        /// </summary>
        /// <returns></returns>
        [Route("order/check")]
        [HttpPost]
        public APIResponse CheckOrder([FromBody] AppCheckOrderDto input)
        {
            try
            {
                _orderServices.CheckOrder(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Check mã OTP, lưu order, đồng thời sinh file đã ký nếu không có file hoặc file không tồn tại vẫn cho lưu lệnh thành công như bình thường
        /// </summary>
        /// <returns></returns>
        [HttpPost("order/add")]
        [ProducesResponseType(typeof(APIResponse<AppOrderDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> OrderAdd([FromBody] AppCreateOrderDto input)
        {
            try
            {
                var result = await _orderServices.OrderInvestorAdd(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sale đặt lệnh hộ investor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("order/sale-add")]
        public async Task<APIResponse> SaleOrderAdd([FromBody] AppSaleInvestorCreateOrderDto input)
        {
            try
            {
                var result = await _orderServices.SaleOrderInvestorAdd(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Check sale trước khi đặt lệnh. Nếu thỏa mãn các điều kiện thì trả về thông tin của sale
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        [HttpGet("order/add/check/sale/{referralCode}/distribution/{distributionId}")]
        public APIResponse CheckSalerBeforeAddOrder(string referralCode, int distributionId)
        {
            try
            {
                var result = _orderServices.CheckSaleBeforeAddOrder(referralCode, distributionId);
                return new APIResponse(result == null ? Utils.StatusCode.Error : Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Màn sổ lệnh của khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("order/get-order")]
        public APIResponse AppOrderGetAll()
        {
            try
            {
                var result = _orderServices.AppOrderGetAll(AppOrderGroupStatus.SO_LENH);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Màn đang đầu tư của khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("order/get-investing")]
        public APIResponse AppOrderInvetingGetAll()
        {
            try
            {
                var result = _orderServices.AppOrderGetAll(AppOrderGroupStatus.DANG_DAU_TU);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Màn lịch sử đầu tư của khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("order/invesment-history")]
        public APIResponse AppOrderInvetmentHistoryGetAll()
        {
            try
            {
                var result = _orderServices.AppOrderGetAll(AppOrderGroupStatus.DA_TAT_TOAN);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("order/{orderId}")]
        public async Task<APIResponse> AppOrderInvestorDetail([Range(1, int.MaxValue)] int orderId)
        {
            try
            {
                var result = await _orderServices.AppOrderInvestorDetail(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy lệnh cho App đang trong trạng thái khởi tạo hoặc chờ thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("order/cancel/{orderId}")]
        public APIResponse AppCancelOrder([Range(1, int.MaxValue)] int orderId)
        {
            try
            {
                var result = _orderServices.AppCancelOrder(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi phương thức tất toán cuối kỳ
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("order/settlement-method/{orderId}")]
        public APIResponse AppUpdateSettlementMethod([Range(1, int.MaxValue)] int orderId, SettlementMethodDto input)
        {
            try
            {
                var result = _orderServices.AppUpdateSettlementMethod(orderId, input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sale theo mã giới thiệu đặt lệnh
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        [HttpGet("order/find-referral-code")]
        public APIResponse AppSaleOrderFindReferralCode(string referralCode, [Range(1, int.MaxValue)] int policyDetailId)
        {
            try
            {
                var result = _orderServices.AppSaleOrderFindReferralCode(referralCode?.Trim(), policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region hợp đồng
        /// <summary>
        /// Get danh sách hợp đồng mẫu
        /// </summary>
        [HttpGet]
        [Route("contract/find")]
        public APIResponse GetContract([Range(1, int.MaxValue)] int policyDetailId)
        {
            try
            {
                var contracts = _contractTemplateServices.FindAllForApp(policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, contracts, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// export contract tạm
        /// </summary>
        /// <returns></returns>
        [HttpGet("export-contract")]
        public async Task<IActionResult> ExportContract([Range(1, double.MaxValue)] decimal totalValue, [Range(1, int.MaxValue)] int policyDetailId, [Range(1, int.MaxValue)] int BankAccId, [Range(1, int.MaxValue)] int identificationId, [Range(1, int.MaxValue)] int contractTemplateId, [Range(1, int.MaxValue)] int? investorId = null)
        {
            try
            {
                var data = _contractDataServices.GetDataContractFileApp(totalValue, policyDetailId, BankAccId, identificationId, investorId, "");
                var result = await _contractDataServices.ExportContractApp(totalValue, policyDetailId, BankAccId, identificationId, contractTemplateId, data, investorId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// xuất file hợp đồng có chữ đã ký
        /// </summary>
        /// <param name="orderContractFileId">id file trong bảng EP_ORDER_CONTRACT_FILE</param>
        /// <returns></returns>
        [HttpGet("export-contract-signature")]
        public IActionResult ExportFileSignature([Range(1, int.MaxValue)] int orderContractFileId)
        {
            try
            {
                var result = _contractDataServices.ExportContractSignatureApp(orderContractFileId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Get danh sách file hợp đồng đã ký
        /// </summary>
        [HttpGet]
        [Route("contract/find-file-signature")]
        public APIResponse FindAllFileSignatureForApp([Range(1, int.MaxValue)] int orderId)
        {
            try
            {
                var contracts = _contractTemplateServices.FindAllFileSignatureForApp(orderId);
                return new APIResponse(Utils.StatusCode.Success, contracts, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Đổi trạng thái đang giao sang đã nhận cho app
        /// </summary>
        /// <param name="deliveryCode"></param>
        /// <returns></returns>
        [Route("order/delivery-status-recevired/{deliveryCode}")]
        [HttpPut]
        public async Task<APIResponse> ChangeDeliveryStatusRecevired(string deliveryCode)
        {
            try
            {
                var result = await _orderServices.ChangeDeliveryStatusReceviredApp(deliveryCode);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Yêu cầu nhận hợp đồng khi lệnh ở trạng thái đang đầu tư, deliveryStatus sẽ chuyển thành 1
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("order/update-delivery-status/{orderId}")]
        [HttpPut]
        public APIResponse AppRequestDeliveryStatus(int orderId)
        {
            try
            {
                var result = _orderServices.AppRequestDeliveryStatus(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Yêu cầu rút vốn / Chi trả
        /// <summary>
        /// Yêu cầu rút vốn với hợp đồng đang hoạt động
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("order/withdraw-request")]
        public async Task<APIResponse> AppWithdrawalRequest([FromBody] AppWithdrawalRequestDto input)
        {
            try
            {
                var result = await _withdrawalServices.AppWithdrawalRequest(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem thay đổi hợp đồng sau khi rút vốn
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="amountMoney"></param>
        /// <returns></returns>
        [HttpGet("change-after-withdrawal")]
        public APIResponse ChangesAfterWithdrawal(long orderId, decimal amountMoney)
        {
            try
            {
                var result = _orderServices.AppViewThayDoiKhiRutVon(orderId, amountMoney);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Yêu cầu thay đổi phương thức tất toán cuối kỳ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("order/renewals-request")]
        public async Task<APIResponse> AppRenewalsRequest([FromBody] AppCreateRenewalsRequestDto input)
        {
            try
            {
                var result = await _renewalsRequestServices.AppRenewalsRequest(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Check ngày yêu cầu tái tục, nếu ngày hết hạn gửi yêu cầu trước ngày hiện tại -> hợp đồng hết hạn yêu cầu tái tục
        /// </summary>
        /// <returns></returns>
        [HttpGet("order/renewals-request-notification/{orderId}")]
        public APIResponse AppRenewalsRequestNotification(long orderId)
        {
            try
            {
                var result = _renewalsRequestServices.AppRenewalsRequestNotification(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem thông tin chính sách kì hạn yêu cầu tái tục theo id lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost("renewals-request-info/{orderId}")]
        public APIResponse AppRenewalsRequestInfo(int orderId)
        {
            try
            {
                var result = _renewalsRequestServices.AppRenewalsRequestInfo(orderId);
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
        public APIResponse AddRating(CreateInvestRatingDto input)
        {
            try
            {
                _investRatingServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
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
                var result = _investRatingServices.FindLastOrder();
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
