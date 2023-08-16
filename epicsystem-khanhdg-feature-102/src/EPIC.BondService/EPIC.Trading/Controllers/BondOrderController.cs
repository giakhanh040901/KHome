using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.BondOrder;
using EPIC.BondEntities.Dto.AppOrder;
using EPIC.CoreDomain.Interfaces;
using EPIC.EntitiesBase.Dto;
using EPIC.Entities.Dto.BondSecondaryContract;
using EPIC.Entities.Dto.Order;
using EPIC.Entities.Dto.OrderContractFile;
using EPIC.Entities.Dto.OrderPayment;
using EPIC.FileEntities.Settings;
using EPIC.Shared.Filter;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/order")]
    [ApiController]
    public class BondOrderController : BaseController
    {
        private readonly IBondOrderService _orderServices;
        private readonly NotificationServices _sendEmailServices;
        private readonly IConfiguration _configuration;
        private readonly IOptions<FileConfig> _fileConfig;

        public BondOrderController(
            ILogger<BondOrderController> logger,
            IBondOrderService orderServices,
            NotificationServices sendEmailServices,
            IConfiguration configuration,
            IOptions<FileConfig> fileConfig)
        {
            _logger = logger;
            _orderServices = orderServices;
            _sendEmailServices = sendEmailServices;
            _configuration = configuration;
            _fileConfig = fileConfig;
        }

        /// <summary>
        /// Tìm kiếm sổ lệnh trong trạng thái đang đầu tư
        /// </summary>
        /// <returns></returns>
        [Route("find-active")]
        [HttpGet]
        [PermissionFilter(Permissions.BondHDPP_HopDong_DanhSach)]
        public APIResponse FindAllOrderActive(int pageNumber, int? pageSize, string keyword, string taxCode, string idNo, string cifCode, string phone, int? status, int? source, int? bondSecondaryId, string bondPolicy, int? bondPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? orderer)
        {
            try
            {
                var result = _orderServices.FindAll(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.DANG_DAU_TU, source, bondSecondaryId, bondPolicy?.Trim(), bondPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, null, orderer);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tim kiếm giao nhận hợp đồng
        /// </summary>
        /// <returns></returns>
        [Route("find-delivery-status")]
        [HttpGet]
        [PermissionFilter(Permissions.BondHDPP_GiaoNhanHopDong_DanhSach)]
        public APIResponse FindAllDeliveryStatus(int pageNumber, int? pageSize, string keyword, string taxCode, string idNo, string cifCode, string phone, int? status, int? source, int? bondSecondaryId, string bondPolicy, int? bondPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, DateTime? pendingDate, DateTime? deliveryDate, DateTime? receivedDate, DateTime? finishedDate, DateTime? date)
        {
            try
            {
                var result = _orderServices.FindAllDeliveryStatus(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.DANG_DAU_TU, source, bondSecondaryId, bondPolicy?.Trim(), bondPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, deliveryStatus, pendingDate, deliveryDate, receivedDate, finishedDate, date);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm trong trạng thái đang xử lý hợp đồng
        /// </summary>
        /// <returns></returns>
        [Route("find-contract-processing")]
        [HttpGet]
        [PermissionFilter(Permissions.BondHDPP_XLHD_DanhSach)]
        public APIResponse FindAllConTractProcessing(int pageNumber, int? pageSize, string taxCode, string idNo, string cifCode, string phone, string keyword, int? status, int? source, int? bondSecondaryId, string bondPolicy, int? bondPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? orderer)
        {
            try
            {
                var result = _orderServices.FindAll(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.XU_LY_HOP_DONG, source, bondSecondaryId, bondPolicy?.Trim(), bondPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, null, orderer);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sổ lệnh trong trạng thái khởi tạo và chờ ký hợp đồng
        /// </summary>
        /// <returns></returns>
        [Route("find")]
        [HttpGet]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_DanhSach, Permissions.BondHDPP_GiaoNhanHopDong_TTC)]
        public APIResponse FindAllOrder(int pageNumber, string taxCode, string idNo, string cifCode, string phone, int? pageSize, string keyword, int? status, int? source, int? bondSecondaryId, string bondPolicy, int? bondPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, int? orderer)
        {
            try
            {
                var result = _orderServices.FindAll(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.SO_LENH, source, bondSecondaryId, bondPolicy?.Trim(), bondPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, deliveryStatus, orderer);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sổ lệnh trong trạng thái phong tỏa hoặc giải tỏa
        /// </summary>
        /// <returns></returns>
        [Route("find-cancel")]
        [HttpGet]
        public APIResponse FindAllCancel(int pageNumber, string taxCode, string idNo, string cifCode, string phone, int? pageSize, string keyword, int? status, int? source, int? bondSecondaryId, string bondPolicy, int? bondPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? orderer)
        {
            try
            {
                var result = _orderServices.FindAll(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.PHONG_TOA, source, bondSecondaryId, bondPolicy?.Trim(), bondPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, null, orderer);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Tìm kiếm sổ lệnh trong trạng thái giao nhận hợp đồng
        /// </summary>
        /// <returns></returns>
        [Route("find-delivery-contract")]
        [HttpGet]

        public APIResponse FindAllDeliveryContract(int pageNumber, string taxCode, string idNo, string cifCode, string phone, int? pageSize, string keyword, int? status, int? source, int? bondSecondaryId, string bondPolicy, int? bondPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, int? orderer)
        {
            try
            {
                var result = _orderServices.FindAll(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.SO_LENH, source, bondSecondaryId, bondPolicy?.Trim(), bondPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, deliveryStatus, orderer);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm lệnh theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("find/{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.Bond_HDPP_TTC_ChiTiet)]
        public APIResponse GetOrder(int id)
        {
            try
            {
                var result = _orderServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("price/find")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_LoiTuc_DanhSach)]
        public APIResponse FindPriceByDate(int bondSecondaryId, DateTime priceDate)
        {
            try
            {
                var result = _orderServices.FindPriceByDate(bondSecondaryId, priceDate);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Tạo lệnh offline, sinh mã hợp đồng, dò cif code có nằm trong tập khách hàng của partner không
        /// các điều kiện where kèm theo partner id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_ThemMoi)]
        public async Task<APIResponse> AddOrder([FromBody] CreateOrderDto body)
        {
            try
            {
                var result = await _orderServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTC_CapNhat)]
        public APIResponse UpdateOrder([FromBody] UpdateOrderDto body, int orderId)
        {
            try
            {
                var result = _orderServices.Update(body, orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update (file lại data) hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("update-contract-file")]
        [HttpPut]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS)]
        public async Task<APIResponse> UpdateContractFile(int orderId)
        {
            try
            {
                await _orderServices.UpdateContractFile(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Ký hợp đồng có chữ ký điện tử khi order ở trạng thái chờ duyệt hoặc đang đầu tư
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("sign-contract-file")]
        [HttpPut]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu)]
        public APIResponse SignContract(int orderId)
        {
            try
            {
                _orderServices.UpdateContractFileSignPdf(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpPost("payment/add")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi)]
        public APIResponse OrderPaymentAdd(CreateOrderPaymentDto input)
        {
            try
            {
                var result = _orderServices.AddPayment(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sửa thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpPut("payment/update")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_CapNhat)]
        public APIResponse OrderPaymentUpdate(int orderPaymentId, [FromBody] UpdateOrderPaymentDto input)
        {
            try
            {
                var result = _orderServices.Update(input, orderPaymentId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        ///  Xóa thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("payment/delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_Xoa)]
        public APIResponse DeleteOrderPayment(int id)
        {
            try
            {
                var result = _orderServices.DeleteOrderPayment(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Lấy danh sách thanh toán
        /// </summary>
        /// <returns></returns>
        [Route("payment/find")]
        [HttpGet]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_DanhSach)]
        public APIResponse FindAllOrderPayment(int orderId, int pageNumber, int? pageSize, string keyword, int? status)
        {
            try
            {
                var result = _orderServices.FindAll(orderId, pageSize ?? 100, pageNumber, keyword?.Trim(), status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm thanh toán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("payment/find/{id}")]
        [HttpGet]
        public APIResponse GetOrderPayment(int id)
        {
            try
            {
                var result = _orderServices.FindPaymentById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái chờ xử lý sang đang giao
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delivery-status-delivered/{id}")]
        [HttpPut]

        public APIResponse ChangeDeliveryStatusDelivered(int id)
        {
            try
            {
                var result = _orderServices.ChangeDeliveryStatusDelivered(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái từ đang giao sang đã nhận
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delivery-status-received/{id}")]
        [HttpPut]
        [PermissionFilter(Permissions.BondHDPP_GiaoNhanHopDong_DoiTrangThai)]
        public APIResponse ChangeDeliveryStatusReceived(int id)
        {
            try
            {
                var result = _orderServices.ChangeDeliveryStatusReceived(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái từ đã nhận sang hoàn thành
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delivery-status-done/{id}")]
        [HttpPut]
        public APIResponse ChangeDeliveryStatusDone(int id)
        {
            try
            {
                var result = _orderServices.ChangeDeliveryStatusDone(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Duyệt thanh toán, hủy thanh toán
        /// duyệt thanh toán nếu đủ tiền thì update trạng thái order thành chờ duyệt hợp đồng, nếu k đủ tiền thì về chờ thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpPut("payment/approve/{orderPaymentId}")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet, Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet)]
        public async Task<APIResponse> OrderPaymentApprove(int orderPaymentId,
            [IntegerRange(AllowableValues = new int[] { OrderPaymentStatus.DA_THANH_TOAN, OrderPaymentStatus.HUY_THANH_TOAN })] int status)
        {
            try
            {
                var result = _orderServices.ApprovePayment(orderPaymentId, status);

                //gửi email thông báo chuyển tiền thành công, nếu chuyển đủ tiền thì sẽ gửi email đầu tư thành công
                if (status == OrderPaymentStatus.DA_THANH_TOAN)
                {
                    await _sendEmailServices.SendEmailBondApprovePayment(orderPaymentId);
                }
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông báo lại chuyển tiền thành công
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        [HttpPost("payment/resend-notification/{orderPaymentId}")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao)]
        public async Task<APIResponse> PaymentResentNotification(int orderPaymentId)
        {
            try
            {
                await _sendEmailServices.SendEmailBondApprovePayment(orderPaymentId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tính lợi tức cho lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("interest/{orderId}")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_LoiTuc_DanhSach)]
        public APIResponse GetInterestInfo(int orderId)
        {
            try
            {
                var result = _orderServices.GetProfitInfo(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sửa file scan hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpPut("scan-contract-file/update")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS)]
        public APIResponse SecondaryContractScanUpdate([FromBody] UpdateBondSecondaryContractDto input)
        {
            try
            {
                var result = _orderServices.UpdateSecondaryContractFileScan(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm file scan hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpPost("scan-contract-file/add")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS)]
        public APIResponse SecondaryContractScanAdd([FromBody] CreateBondSecondaryContractDto input)
        {
            try
            {
                var result = _orderServices.CreateSecondaryContractFileScan(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa order theo trạng thái khởi tạo
        /// </summary>
        /// <returns></returns>
        [Route("order/delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _orderServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #region cập nhật lại thông tin
        /// <summary>
        /// Cập nhật tài khoản thụ hưởng của khách hàng mua trái phiếu
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="investorBankAccId"></param>
        /// <returns></returns>
        [HttpPut("update/bank-account")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTC_DoiTKNganHang)]
        public APIResponse UpdateBankAccount(int orderId, [Required] int? investorBankAccId)
        {
            try
            {
                var result = _orderServices.UpdateInvestorBankAccount(orderId, investorBankAccId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật số tiền đầu tư
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="totalValue"></param>
        /// <returns></returns>
        [HttpPut("update/total-value")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu)]
        public APIResponse UpdateTotalValue(int orderId, [Required(ErrorMessage = "Số tiền đầu tư không được bỏ trống")] decimal? totalValue)
        {
            try
            {
                var result = _orderServices.UpdateTotalValue(orderId, totalValue);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật kỳ hạn, chính sách
        /// </summary>
        /// <returns></returns>
        [HttpPut("update/update-policy-detail")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTC_DoiKyHan)]
        public APIResponse ContractUpdatePoliceDetail(int orderId, [Required(ErrorMessage = "Kỳ hạn không được bỏ trống")] int bondPolicyDetailId)
        {
            try
            {
                var result = _orderServices.UpdatePolicyDetail(orderId, bondPolicyDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật mã giới thiệu của nhân viên tư vấn
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="saleReferralCode"></param>
        /// <returns></returns>
        [HttpPut("update/referral-code")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTC_DoiMaGT)]
        public APIResponse UpdateReferralCode(int orderId, string saleReferralCode)
        {
            try
            {
                var result = _orderServices.UpdateReferralCode(orderId, saleReferralCode);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật nguồn đặt lệnh từ offline, sale đặt lệnh, sang online
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("update/source")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline)]
        public APIResponse UpdateSource(int orderId)
        {
            try
            {
                var result = _orderServices.UpdateSource(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt hợp đồng khi đầu tư offline
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("update/approve")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy)]
        public async Task<APIResponse> ContractApprove(int orderId)
        {
            try
            {
                var result = _orderServices.OrderApprove(orderId);
                await _sendEmailServices.SendEmailBondOrderActive(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông báo lại chuyển tiền thành công
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost("resend-notification/{orderId}")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao)]
        public async Task<APIResponse> OrderResendNotification(int orderId)
        {
            try
            {
                await _sendEmailServices.SendEmailBondOrderActive(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt hợp đồng khi đầu tư offline
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("update/cancel")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy)]
        public APIResponse ContractCancel(int orderId)
        {
            try
            {
                var result = _orderServices.OrderCancel(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        /// <summary>
        /// Kiểm tra ngày đầu tư
        /// </summary>
        /// <param name="policyDetailId"></param>
        /// <param name="priceDate"></param>
        /// <returns></returns>
        [HttpGet("check-investment-day")]
        public APIResponse CheckInvestmentDay(int policyDetailId, DateTime priceDate)
        {
            try
            {
                var result = _orderServices.CheckInvestmentDay(policyDetailId, priceDate);
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
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update/settlement-method")]
        public APIResponse UpdateSettlementMethod(int id, SettlementMethodDto input)
        {
            try
            {
                var result = _orderServices.UpdateSettlementMethod(id, input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        
        /// <summary>
        /// Lấy danh sách chi trả để lập chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("lap-danh-sach-chi-tra")]
        public APIResponse LapDanhSachChiTra([FromQuery] DanhSachChiTraFitlerDto input)
        {
            try
            {
                var result = _orderServices.LapDanhSachChiTra(input);
                return new APIResponse(Utils.StatusCode.Success, result.Items, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// update trường isSign theo orderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("update-contract-file/update-is-sign")]
        public APIResponse UpdateIsSignByOrderId(int orderId)
        {
            try
            {
                var result = _orderServices.UpdateIsSignByOrderId(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
