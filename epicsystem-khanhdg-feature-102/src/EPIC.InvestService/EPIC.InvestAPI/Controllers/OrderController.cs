using EPIC.FileEntities.Settings;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestEntities.Dto.OrderContractFile;
using EPIC.InvestEntities.Dto.OrderPayment;
using EPIC.Notification.Services;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Controllers;
using EPIC.Utils.Validation;
using EPIC.WebAPIBase.FIlters;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [ApiController]
    [Route("api/invest/order")]
    public class OrderController : BaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly IInvestOrderContractFileServices _orderContractFileServices;
        private readonly IConfiguration _configuration;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly InvestNotificationServices _sendEmailServices;

        public OrderController(ILogger<OrderController> logger,
            IOrderServices orderServices,
            IInvestOrderContractFileServices orderContractFileServices,
            IConfiguration configuration,
            IOptions<FileConfig> fileConfig,
            InvestNotificationServices sendEmailServices)
        {
            _logger = logger;
            _orderServices = orderServices;
            _orderContractFileServices = orderContractFileServices;
            _configuration = configuration;
            _fileConfig = fileConfig;
            _sendEmailServices = sendEmailServices;
        }

        /// <summary>
        /// Tìm kiếm sổ lệnh trong trạng thái đang đầu tư
        /// </summary>
        /// <returns></returns>
        [Route("find-active")]
        [HttpGet]
        [PermissionFilter(Permissions.InvestHDPP_HopDong)]
        public APIResponse FindAllOrderActive([FromQuery] InvestOrderFilterDto input)
        {
            try
            {
                var result = _orderServices.FindAll(input, OrderGroupStatus.DANG_DAU_TU);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("find-delivery-status")]
        [PermissionFilter(Permissions.InvestHDPP_GiaoNhanHopDong_DanhSach)]
        [HttpGet]
        public APIResponse FindAllDeliveryStatus([FromQuery] InvestOrderFilterDto input)
        {
            try
            {
                var result = _orderServices.FindAllDeliveryStatus(input, null);
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
        [PermissionFilter(Permissions.InvestHDPP_GiaoNhanHopDong_DanhSach)]
        public APIResponse FindAllDeliveryContract([FromQuery] InvestOrderFilterDto input)
        {
            try
            {
                var result = _orderServices.FindAll(input, OrderGroupStatus.SO_LENH);
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
        [PermissionFilter(Permissions.InvestHDPP_XLHD_DanhSach)]
        public APIResponse FindAllConTractProcessing([FromQuery] InvestOrderFilterDto input)
        {
            try
            {
                var result = _orderServices.FindAll(input, OrderGroupStatus.XU_LY_HOP_DONG);
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
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_DanhSach, Permissions.InvestHDPP_GiaoNhanHopDong_TTC)]
        public APIResponse FindAllOrder([FromQuery] InvestOrderFilterDto input)
        {
            try
            {
                var result = _orderServices.FindAll(input, OrderGroupStatus.SO_LENH);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sách tất toán của hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-settlement")]
        public APIResponse FindAllOrderSettlement([FromQuery] InvestOrderFilterDto input)
        {
            try
            {
                var result = _orderServices.FindAll(input, OrderStatus.TAT_TOAN);
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
        public APIResponse FindAllCancel([FromQuery] InvestOrderFilterDto input)
        {
            try
            {
                var result = _orderServices.FindAll(input, OrderGroupStatus.PHONG_TOA);
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
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTC_XemChiTiet, Permissions.InvestPDYCRV_ChiTietHD, Permissions.InvestHopDong_HopDongDaoHan_ThongTinDauTu, Permissions.InvestHDPP_LSDT_ThongTinDauTu)]
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

        /// <summary>
        /// Lấy danh sách chi trả để lập chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("lap-danh-sach-chi-tra")]
        //[PermissionFilter(Permissions.InvestHDPP_CTLT_DanhSach)]
        public async Task<APIResponse> LapDanhSachChiTra([FromQuery] InterestPaymentFilterDto input)
        {
            try
            {
                var result = await _orderServices.LapDanhSachChiTra(input, IsLastPeriod.NO);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chi trả để lập chi trả cuối kỳ (tất toán, có thẻ có tái tục vốn)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("lap-danh-sach-chi-tra-cuoi-ky")]
        [PermissionFilter(Permissions.InvestHopDong_HopDongDaoHan_DanhSach)]
        public async Task<APIResponse> LapDanhSachChiTraCuoiKy([FromQuery] InterestPaymentFilterDto input)
        {
            try
            {
                var result = await _orderServices.LapDanhSachChiTra(input, IsLastPeriod.YES);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lập thông báo đến hạn đáo hạn của hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpPost("notice-payment-duedate")]
        public async Task<APIResponse> NoticePaymentDueDate()
        {
            try
            {
                var result = await _orderServices.NoticePaymentDueDate();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lập thông báo đến hạn đáo hạn của hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpPost("invest-notify-payment-duedate")]
        public APIResponse NotifyPaymentDueDate()
        {
            try
            {
                RecurringJob.AddOrUpdate("invest-notify-payment-duedate", () => _orderServices.NotifyPaymentDueDate(), Cron.Daily(0, 0), TimeZoneInfo.Local);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
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
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<ViewOrderDto>), (int)HttpStatusCode.OK)]
        public APIResponse AddOrder([FromBody] CreateOrderDto body)
        {
            try
            {
                var result = _orderServices.Add(body);
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
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTC_CapNhat)]
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
        /// xóa order theo trạng thái khởi tạo
        /// </summary>
        /// <returns></returns>
        [Route("delete")]
        [HttpPut]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_Xoa)]
        public APIResponse Delete(List<int> ids)
        {
            try
            {
                _orderServices.Delete(ids);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
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
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<OrderPaymentDto>), (int)HttpStatusCode.OK)]
        public APIResponse OrderPaymentAdd(CreateOrderPaymentDto input)
        {
            try
            {
                var result = _orderServices.OrderPaymentAdd(input);
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
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_CapNhat)]
        public APIResponse UpdateOrderPayment(int id, [FromBody] UpdateOrderPaymentDto input)
        {
            try
            {
                var result = _orderServices.UpdateOrderPayment(input, id);
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
        [HttpDelete("payment/delete/{id}")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_Xoa)]
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
        [HttpGet("payment/find")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_DanhSach)]
        public APIResponse FindAllOrderPayment(int orderId, int pageNumber, int? pageSize, string keyword, int? status)
        {
            try
            {
                var result = _orderServices.FindAllOrderPayment(orderId, pageSize ?? 100, pageNumber, keyword?.Trim(), status);
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
        [HttpGet("payment/find/{id}")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan)]
        public APIResponse FindPaymentById(int id)
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
        /// Duyệt thanh toán, hủy thanh toán
        /// duyệt thanh toán nếu đủ tiền thì update trạng thái order thành chờ duyệt hợp đồng, nếu k đủ tiền thì về chờ thanh toán
        /// </summary>
        /// <returns></returns>
        [HttpPut("payment/approve/{orderPaymentId}")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet, Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet)]
        [WhiteListIpFilter(WhiteListIpTypes.InvestDuyetVaoTien)]
        public async Task<APIResponse> ApproveOrderPayment(int orderPaymentId,
            [IntegerRange(AllowableValues = new int[] { OrderPaymentStatus.DA_THANH_TOAN, OrderPaymentStatus.HUY_THANH_TOAN })] int status)
        {
            try
            {
                var result = _orderServices.ApproveOrderPayment(orderPaymentId, status);
                if (status == OrderPaymentStatus.DA_THANH_TOAN)
                {
                    await _sendEmailServices.SendEmailInvestApprovePayment(orderPaymentId);
                }
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông báo lại chuyển tiền
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        [HttpPost("payment/resend-notification/{orderPaymentId}")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao)]
        public async Task<APIResponse> PaymentResendNotification(int orderPaymentId)
        {
            try
            {
                await _sendEmailServices.SendEmailInvestApprovePayment(orderPaymentId);
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
        [ProducesResponseType(typeof(APIResponse<InvestOrderCashFlowDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.InvestSoLenh_LoiNhuan_DanhSach)]
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
        /// Update (file lại data) hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("update-contract-file")]
        [HttpPut]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS)]
        public async Task<APIResponse> UpdateContractFile(int orderId)
        {
            try
            {
                await _orderContractFileServices.UpdateContractFile(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
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
        [PermissionFilter(Permissions.InvestHDPP_GiaoNhanHopDong_DoiTrangThai)]
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
        [PermissionFilter(Permissions.InvestHDPP_GiaoNhanHopDong_DoiTrangThai)]
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
        [PermissionFilter(Permissions.InvestHDPP_GiaoNhanHopDong_DoiTrangThai)]
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
        /// Ký điện tử
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("sign-contract-file")]
        [HttpPut]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu)]
        public APIResponse SignContractFile(int orderId)
        {
            try
            {
                _orderContractFileServices.UpdateContractFileSignPdf(orderId, ContractTypes.DAT_LENH);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
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
        [HttpPost("order-contract-file/add")]
        public APIResponse OrderContractFileAdd([FromBody] CreateOrderContractFileDto input)
        {
            try
            {
                var result = _orderServices.AddOrderContractFile(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update đường dẫn file mẫu
        /// </summary>
        /// <returns></returns>
        [HttpPut("order-contract-file/update")]
        public APIResponse OrdeContractFileUpdate([FromBody] UpdateOrderContractFileDto input)
        {
            try
            {
                _orderServices.UpdateOrderContractFile(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #region cập nhật lại thông tin
        /// <summary>
        /// cập nhật thông tin khách hàng 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update/info-customer")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTC_DoiTTKhachHang)]
        public APIResponse UpdateInfoCustomer([FromBody] UpdateInfoCustomerDto input)
        {
            try
            {
                var result = _orderServices.UpdateInfoCustomer(input.OrderId, input.InvestorBankAccId, input.ContractAddressId, input.investorIdenId);
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
        /// <param name="id"></param>
        /// <param name="totalValue"></param>
        /// <returns></returns>
        [HttpPut("update/total-value")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu)]
        public APIResponse UpdateTotalValue(int id, [Required(ErrorMessage = "Số tiền đầu tư không được bỏ trống")] decimal? totalValue)
        {
            try
            {
                var result = _orderServices.UpdateTotalValue(id, totalValue);
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
        /// <param name="id"></param>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        [HttpPut("update/update-policy-detail")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTC_DoiKyHan)]

        public APIResponse ContractUpdatePoliceDetail(int id, [Required(ErrorMessage = "Kỳ hạn không được bỏ trống")] int policyDetailId)
        {
            try
            {
                var result = _orderServices.UpdatePolicyDetail(id, policyDetailId);
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
        /// <param name="id"></param>
        /// <param name="saleReferralCode"></param>
        /// <returns></returns>
        [HttpPut("update/referral-code")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_TTC_DoiMaGT)]
        public APIResponse UpdateReferralCode(int id, string saleReferralCode)
        {
            try
            {
                var result = _orderServices.UpdateReferralCode(id, saleReferralCode);
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update/source")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline)]
        public async Task<APIResponse> UpdateSource(int id)
        {
            try
            {
                var result = await _orderServices.UpdateSource(id);
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update/approve")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy)]
        [WhiteListIpFilter(WhiteListIpTypes.InvestDuyetHopDong)]
        public async Task<APIResponse> ContractApprove(int id)
        {
            try
            {
                var result = _orderServices.OrderApprove(id);
                await _sendEmailServices.SendEmailInvestOrderActive(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông báo lại đầu tư thành công
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost("resend-notification/{orderId}")]
        public async Task<APIResponse> OrderResendNotification(int orderId)
        {
            try
            {
                await _sendEmailServices.SendEmailInvestOrderActive(orderId);
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update/cancel")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy)]
        public APIResponse ContractCancel(int id)
        {
            try
            {
                var result = _orderServices.OrderCancel(id);
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

        [HttpGet("rut-von")]
        public APIResponse RutVon(long orderId, decimal soTienRut, DateTime priceDate)
        {
            try
            {
                var result = _orderServices.RutVon(orderId, soTienRut, priceDate);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("theo-doi-rut-von")]
        public APIResponse TheoDoiRutTruocHan(long orderId)
        {
            try
            {
                var result = _orderServices.TheoDoiRutTruocHan(orderId);
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

        [HttpGet("get-order-history-update")]
        [PermissionFilter(Permissions.InvestSoLenh_LichSuHD_DanhSach)]
        public APIResponse GetInvestOrderHistoryUpdate(int pageNumber, int? pageSize, string keyword, int orderId)
        {
            try
            {
                var result = _orderServices.GetOrderHistoryUpdate(pageNumber, pageSize, keyword?.Trim(), orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// update trường isSign theo orderId (Hủy ký)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("order-contract-file/update-is-sign")]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu)]
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

        /// <summary>
        /// Xử lý yêu cầu nhận hợp đồng
        /// </summary>
        /// <returns></returns>
        [Route("process-contract")]
        [HttpPut]
        [PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung)]
        public APIResponse ProcessContract(int orderId)
        {
            try
            {
                var result = _orderServices.ProcessContract(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lịch sử đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("invest-history")]
        [PermissionFilter(Permissions.InvestHDPP_LSDT_DanhSach)]
        public APIResponse FindAllInvestHistory([FromQuery] FilterInvestOrderDto input)
        {
            try
            {
                var result = _orderServices.FindAllInvestHistory(input, new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_KY_HOP_DONG });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// hủy yêu cầu tái tục mới nhất theo orderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("cancel-renewal-request")]
        public APIResponse CancelRenewalRequest(long orderId)
        {
            try
            {
                _orderServices.CancelRenewalRequestByOrderId(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("renewals-request")]
        //[PermissionFilter(Permissions.InvestHDPP_HDTT_DanhSach)]
        public APIResponse FindAllRenewalsReqeust([FromQuery] FilterRenewalsRequestDto input)
        {
            try
            {
                var result = _orderServices.GetAllRenewalsRequest(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        ///  Cập nhật lại dòng tiền
        /// </summary>
        [HttpPut("update-cash-flow")]
        public APIResponse UpdateOrderCashFlow(int orderId)
        {
            try
            {
                _orderServices.UpdateOrderCashFlow(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
