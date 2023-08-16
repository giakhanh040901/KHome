using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerHistory;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerOrderContractFile;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/order")]
    [ApiController]
    public class GarnerOrderController : BaseController
    {
        private readonly IGarnerOrderServices _garnerOrderServices;
        private readonly IGarnerOrderContractFileServices _garnerOrderContractFileServices;

        public GarnerOrderController(ILogger<GarnerOrderController> logger,
            IGarnerOrderServices garnerOrderServices,
            IGarnerOrderContractFileServices garnerOrderContractFileServices)
        {
            _logger = logger;
            _garnerOrderServices = garnerOrderServices;
            _garnerOrderContractFileServices = garnerOrderContractFileServices;
        }

        /// <summary>
        /// Thêm lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<GarnerOrderMoreInfoDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> Add([FromBody] CreateGarnerOrderDto input)
        {
            try
            {
                var result = await _garnerOrderServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy tất cả sổ lệnh có trạng thái 1,2,3,9
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterGarnerOrderDto input)
        {
            try
            {
                var result = _garnerOrderServices.FindAll(input, new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_KY_HOP_DONG});
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-all-history")]
        [PermissionFilter(Permissions.GarnerSoLenh_LichSuHD_DanhSach, Permissions.GarnerHDPP_XLRutTien_ThongTinChiTiet, Permissions.GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet, Permissions.GarnerHDPP_LSTL_ThongTinChiTiet)]
        public APIResponse FindAll([FromQuery] FilterGarnerHistoryDto input)
        {
            try
            {
                var result = _garnerOrderServices.FindAllHistoryTable(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy sổ lệnh theo Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTC_XemChiTiet, Permissions.GarnerHDPP_XLRutTien_ThongTinChiTiet, Permissions.GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet, Permissions.GarnerHDPP_LSTL_ThongTinChiTiet)]
        public APIResponse FindOrderById(long orderId)
        {
            try
            {
                var result = _garnerOrderServices.FindById(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lợi tức tương lai
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("profit-future/{orderId}")]
        public APIResponse ProfitFuture(long orderId)
        {
            try
            {
                var result = _garnerOrderServices.ProfitFuture(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy ra danh sách chính sách theo cifCode
        /// </summary>
        /// <param name="cifCode"></param>
        /// <returns></returns>
        [HttpGet("policy/find-by-cifcode/{cifCode}")]
        public APIResponse GetListPolicyByCifCode(string cifCode)
        {
            try
            {
                var result = _garnerOrderServices.GetListPolicyByCifCode(cifCode.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách ngân hàng đặt lệnh theo cifCode
        /// </summary>
        [HttpGet("banks/find-by-cifcode/{cifCode}")]
        public APIResponse FindListBankOfCifCode(string cifCode)
        {
            try
            {
                var result = _garnerOrderServices.FindListBankOfCifCode(cifCode?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("approve/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy)]
        [WhiteListIpFilter(WhiteListIpTypes.GarnerDuyetHopDong)]
        public async Task<APIResponse> OrderApprove(long orderId)
        {
            try
            {
                await _garnerOrderServices.OrderApprove(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xử lý yêu cầu nhận hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("process-contract/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung)]
        public APIResponse ProcessContract(long orderId)
        {
            try
            {
                var result = _garnerOrderServices.ProcessContract(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái giao nhận hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("change-delivery-status/{orderId}")]
        public APIResponse ChangeDeliveryStatus(long orderId)
        {
            try
            {
                var result = _garnerOrderServices.ChangeDeliveryStatus(orderId);
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
        [HttpPut("update/cancel/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy)]
        public APIResponse OrderCancel(int orderId)
        {
            try
            {
                var result = _garnerOrderServices.OrderCancel(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
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
        [HttpPut("update/source/{orderId}")]
        public async Task<APIResponse> UpdateSource(long orderId)
        {
            try
            {
                var result = await _garnerOrderServices.UpdateSource(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTC_CapNhat)]
        public APIResponse Update([FromBody] UpdateGarnerOrderDto input)
        {
            try
            {
                var result = _garnerOrderServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa hợp đồng ở trạng thái khởi tạo
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        [HttpPut("deleted")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_Xoa)]
        public APIResponse Deleted(List<long> orderIds)
        {
            try
            {
                _garnerOrderServices.Deleted(orderIds);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái chờ xử lý sang đang giao
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("delivery-status-delivery/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_GiaoNhanHopDong_DoiTrangThai)]
        public APIResponse ChangeDeliveryStatusDelivered(int orderId)
        {
            try
            {
                var result = _garnerOrderServices.ChangeDeliveryStatusDelivered(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái từ đang giao sang đã nhận
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("delivery-status-received/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_GiaoNhanHopDong_DoiTrangThai)]
        public APIResponse ChangeDeliveryStatusReceived(int orderId)
        {
            try
            {
                var result = _garnerOrderServices.ChangeDeliveryStatusReceived(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái từ đã nhận sang hoàn thành
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("delivery-status-done/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_GiaoNhanHopDong_DoiTrangThai)]
        public APIResponse ChangeDeliveryStatusDone(int orderId)
        {
            try
            {
                var result = _garnerOrderServices.ChangeDeliveryStatusDone(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy ra order có trạng thái  4
        /// </summary>
        /// <returns></returns>
        [HttpGet("processing/find-all")]
        [PermissionFilter(Permissions.GarnerHDPP_XLHD_DanhSach)]
        public APIResponse FindAllConTractProcessing([FromQuery] FilterGarnerOrderDto input)
        {
            try
            {
                var result = _garnerOrderServices.FindAll(input, new int[] { OrderStatus.CHO_DUYET_HOP_DONG });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy ra trạng thái order có Id = 5,6
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("active/find-all")]
        [PermissionFilter(Permissions.GarnerHDPP_HopDong_DanhSach)]
        public APIResponse FindAllConTractActive([FromQuery] FilterGarnerOrderDto input)
        {
            try
            {
                var result = _garnerOrderServices.FindAll(input, new int[] { OrderStatus.PHONG_TOA, OrderStatus.DANG_DAU_TU });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy ra trạng thái order có Id = 8
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("settlement/find-all")]
        [PermissionFilter(Permissions.GarnerHDPP_HopDong_DanhSach)]
        public APIResponse FindAllConTractSettlement([FromQuery] FilterGarnerOrderDto input)
        {
            try
            {
                var result = _garnerOrderServices.FindAll(input, new int[] { OrderStatus.TAT_TOAN });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tìm kiếm phân trang, giao nhận hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("active/find-all-delivery")]
        [PermissionFilter(Permissions.GarnerHDPP_GiaoNhanHopDong_DanhSach)]
        public APIResponse FindAllDelivery([FromQuery] FilterGarnerOrderDto input)
        {
            try
            {
                var result = _garnerOrderServices.FindAll(input, new int[] { OrderStatus.DANG_DAU_TU }, false, true);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm theo policy, chia trang theo cifcode và policy id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("policy/find-all")]
        public APIResponse FindAllByPolicy([FromQuery] FilterGarnerOrderDto input)
        {
            try
            {
                var result = _garnerOrderServices.FindAllByPolicy(input, new int[] { OrderStatus.DANG_DAU_TU, OrderStatus.PHONG_TOA });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Mẫu file import
        /// </summary>
        /// <returns></returns>
        [HttpGet("import-template")]
        [AllowAnonymous]
        public IActionResult ImportFileTemplate()
        {
            try
            {
                var result = _garnerOrderServices.ImportFileTemplate();
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Thêm hợp đồng từ file excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("import")]
        public async Task<APIResponse> ImportExcelOrder([FromForm] ImportExcelOrderDto dto)
        {
            try
            {
                await _garnerOrderServices.ImportExcelOrder(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #region Cập nhật lại thông tin

        ///// <summary>
        ///// Cập nhật tài khoản thụ hưởng của khách hàng
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <param name="investorBankAccId"></param>
        ///// <returns></returns>
        //[HttpPut("update/bank-account/{orderId}")]
        //[PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTC_DoiTKNganHang)]
        //[ProducesResponseType(typeof(APIResponse<GarnerOrder>), (int)HttpStatusCode.OK)]
        //public APIResponse UpdateBankAccount(long orderId, [Required] int? investorBankAccId)
        //{
        //    try
        //    {
        //        var result = _garnerOrderServices.UpdateInvestorBankAccount(orderId, investorBankAccId);
        //        return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        /// <summary>
        /// Cập nhật số tiền đầu tư
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="totalValue"></param>
        /// <returns></returns>
        [HttpPut("update/total-value/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu)]
        [ProducesResponseType(typeof(APIResponse<GarnerOrder>), (int)HttpStatusCode.OK)]
        public APIResponse UpdateTotalValue(long orderId, [Required(ErrorMessage = "Số tiền đầu tư không được bỏ trống")] decimal? totalValue)
        {
            try
            {
                var result = _garnerOrderServices.UpdateTotalValue(orderId, totalValue);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật chính sách vào sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpPut("update/update-policy/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTC_DoiKyHan)]
        [ProducesResponseType(typeof(APIResponse<GarnerOrder>), (int)HttpStatusCode.OK)]
        public APIResponse ContractUpdatePolice(long orderId, int policyId)
        {
            try
            {
                var result = _garnerOrderServices.UpdatePolicy(orderId, policyId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật kỳ hạn, chính sách
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        [HttpPut("update/update-policy-detail/{orderId}")]
        [ProducesResponseType(typeof(APIResponse<GarnerOrder>), (int)HttpStatusCode.OK)]
        public APIResponse ContractUpdatePoliceDetail(long orderId, [Required(ErrorMessage = "Kỳ hạn không được bỏ trống")] int policyDetailId)
        {
            try
            {
                var result = _garnerOrderServices.UpdatePolicyDetail(orderId, policyDetailId);
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
        [HttpPut("update/referral-code/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTC_DoiMaGT)]
        [ProducesResponseType(typeof(APIResponse<GarnerOrder>), (int)HttpStatusCode.OK)]
        public APIResponse UpdateReferralCode(long orderId, string saleReferralCode)
        {
            try
            {
                var result = _garnerOrderServices.UpdateReferralCode(orderId, saleReferralCode);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update/info-customer")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_TTC_DoiMaGT)]
        [ProducesResponseType(typeof(APIResponse<GarnerOrder>), (int)HttpStatusCode.OK)]
        public APIResponse UpdateInfoCustomer([FromBody] UpdateGarnerInfoCustomerDto input)
        {
            try
            {
                var result = _garnerOrderServices.UpdateInfoCustomer(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion Cập nhật lại thông tin

        #region Hợp đồng

        /// <summary>
        /// Kí điện tử
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("sign-contract-file/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu)]
        public APIResponse SignContractFile(int orderId)
        {
            try
            {
                _garnerOrderContractFileServices.UpdateContractFileSignPdf(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
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
        [HttpPut("update-contract-file/{orderId}")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS)]
        public async Task<APIResponse> UpdateContractFile(int orderId)
        {
            try
            {
                await _garnerOrderContractFileServices.UpdateContractFile(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update file Scan hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("update-file-scan")]
        [HttpPut]
        public APIResponse UpdateFileScanContract([FromQuery] UpdateOrderContractFileDto input)
        {
            try
            {
                _garnerOrderContractFileServices.UpdateFileScanContract(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gửi lại email thông báo
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost("resend-notify/{orderId}")]
        public async Task<APIResponse> ResendNotify(long orderId)
        {
            try
            {
                await _garnerOrderServices.ResendNotifyOrderApprove(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion Hợp đồng
    }
}