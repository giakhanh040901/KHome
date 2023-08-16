using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.ManagerInvestor;
//using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Shared.Filter;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
//using EPIC.CoreEntities.Dto.Investor;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.WebAPIBase.FIlters;
using EPIC.CoreDomain.Interfaces.v2;
using EPIC.CoreDomain.Implements.v2;
using System.Collections.Generic;
using System.Net;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.Dto.InvConfigContractCode;

namespace EPIC.CoreAPI.Controllers
{
    /// <summary>
    /// Quản lý khách hàng cá nhân
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/manager-investor")]
    [ApiController]
    public class ManagerInvestorController : BaseController
    {
        private readonly IManagerInvestorServices _managerInvestorServices;
        private readonly NotificationServices _sendEmailServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IInvestorV2Services _investorV2Services;

        public ManagerInvestorController(ILogger<ManagerInvestorController> logger, IManagerInvestorServices managerInvestorServices, 
            NotificationServices sendEmailServices, IHttpContextAccessor httpContextAccessor, IInvestorV2Services investorV2Services)
        {
            _logger = logger;
            _managerInvestorServices = managerInvestorServices;
            _sendEmailServices = sendEmailServices;
            _httpContextAccessor = httpContextAccessor;
            _investorV2Services = investorV2Services;
        }

        /// <summary>
        /// OCR lấy thông tin từ giấy tờ. Không lưu gì trên db
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [Route("ekyc")]
        public async Task<APIResponse> PostEkyc([FromForm] EkycManagerInvestorDto input)
        {
            try
            {
                var result = await _managerInvestorServices.EkycOCRAsync(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Upload ảnh nhận diện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("face-image")]
        [PermissionFilter(Permissions.CoreKHCN_CheckFace)]
        public async Task<APIResponse> UploadFaceImage([FromForm] UploadFaceImageDto input)
        {
            try
            {
                var result = await _managerInvestorServices.UploadFaceImageAsync(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo investor và identification vào bảng tạm (Trạng thái tạm)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [Route("add")]
        [PermissionFilter(Permissions.CoreDuyetKHCN_ThemMoi)]
        public APIResponse CreateInvestorTemporary([FromBody] CreateManagerInvestorEkycDto input)
        {
            try
            {
                var investorTemp = _managerInvestorServices.CreateInvestorTemporary(input);
                return new APIResponse(Utils.StatusCode.Success, investorTemp, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mới giấy tờ tuỳ thân (Trạng thái Nháp)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [Route("add/identification")]
        [PermissionFilter(Permissions.CoreDuyetKHCN_GiayTo_ThemMoi, Permissions.CoreKHCN_GiayTo_ThemMoi)]
        public APIResponse CreateIdentificationTemporary([FromBody] CreateIdentificationTemporaryDto input)
        {
            try
            {
                var result = _managerInvestorServices.CreateIdentificationTemporary(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add/user")]
        public APIResponse CreateUser([FromBody] CreateUserByInvestorDto input)
        {
            try
            {
                _managerInvestorServices.CreateUser(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo bank cho investor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add/bank")]
        [PermissionFilter(Permissions.CoreDuyetKHCN_TKNH_ThemMoi, Permissions.CoreKHCN_TKNH_ThemMoi)]
        public APIResponse CreateBank(CreateInvestorBankTempDto dto)
        {
            try
            {
                var result = _managerInvestorServices.CreateBankTemporary(dto);

                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Them contact address
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add/contact-address")]
        [PermissionFilter(Permissions.CoreDuyetKHCN_DiaChi_ThemMoi, Permissions.CoreKHCN_DiaChi_ThemMoi)]
        public APIResponse CreateContactAddress(CreateManagerInvestorContactAddressDto dto)
        {
            try
            {
                var result = _managerInvestorServices.AddContactAddress(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo công ty chứng khoán
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add/stock")]
        //[PermissionFilter(Permissions.CoreDuyetKHCN_DiaChi_ThemMoi, Permissions.CoreKHCN_DiaChi_ThemMoi)]
        public APIResponse CreateStock(CreateInvestorStockDto dto)
        {
            try
            {
                var result = _managerInvestorServices.CreateInvestorStock(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đăng ký tư vấn viên
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add/sale")]
        [PermissionFilter(Permissions.CoreKHCN_TuVanVien_ThemMoi)]
        public APIResponse AddSale(AddSaleManagerInvestorDto dto)
        {
            try
            {
                _managerInvestorServices.AddSale(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin của investor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [Route("update")]
        [PermissionFilter(Permissions.CoreDuyetKHCN_CapNhat, Permissions.CoreKHCN_CapNhat)]
        public APIResponse UpdateInvestor([FromBody] UpdateManagerInvestorDto input)
        {
            try
            {
                var result = _managerInvestorServices.UpdateInvestor(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái của Nhà đầu tư (Nháp => Trình Duyệt)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("request")]
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [PermissionFilter(Permissions.CoreDuyetKHCN_TrinhDuyet)]
        public APIResponse CreateApproveRequest([FromBody] RequestApproveDto dto)
        {
            try
            {
                _managerInvestorServices.CreateApproveRequest(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Trình duyệt => Đã duyệt (Investor)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approve")]
        [PermissionFilter(Permissions.CoreQLPD_KHCN_PheDuyetOrHuy)]
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        public async Task<APIResponse> Approve([FromBody] ApproveManagerInvestorDto dto)
        {
            try
            {
                await _managerInvestorServices.Approve(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("approve/prof")]
        [PermissionFilter(Permissions.CoreQLPD_NDTCN_PheDuyetOrHuy)]
        public APIResponse ApproveProf(ApproveProfDto dto)
        {
            try
            {
                _managerInvestorServices.ApproveProf(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// EPIC duyệt (Xác minh)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("check")]
        [PermissionFilter(Permissions.CoreKHCN_XacMinh)]
        public APIResponse Check([FromBody] ApproveManagerInvestorDto dto)
        {
            try
            {
                _managerInvestorServices.Check(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Huỷ yêu cầu trình duyệt investor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("cancel")]
        [PermissionFilter(Permissions.CoreQLPD_KHCN_PheDuyetOrHuy)]
        public APIResponse CancelRequest([FromBody] CancelRequestManagerInvestorDto dto)
        {
            try
            {
                _managerInvestorServices.Cancel(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Huỷ yêu cầu trình duyệt Nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("cancel/prof")]
        [PermissionFilter(Permissions.CoreQLPD_NDTCN_PheDuyetOrHuy)]
        public APIResponse CancelRequestProf([FromBody] CancelRequestInvestorProfDto dto)
        {
            try
            {
                _managerInvestorServices.CancelRequestProf(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("cancel/phone")]
        [PermissionFilter(Permissions.CoreQLPD_KHCN_PheDuyetOrHuy)]
        public APIResponse CancelRequestPhone([FromBody] CancelRequestNotInvestor dto)
        {
            try
            {
                _managerInvestorServices.CancelRequestPhone(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt email
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("cancel/email")]
        [PermissionFilter(Permissions.CoreQLPD_KHCN_PheDuyetOrHuy)]
        public APIResponse CancelRequestEmail([FromBody] CancelRequestNotInvestor dto)
        {
            try
            {
                _managerInvestorServices.CancelRequestEmail(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo yêu cầu duyệt email
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("request/email")]
        [PermissionFilter(Permissions.CoreKHCN_Email_TrinhDuyet)]
        public APIResponse CreateRequestEmail([FromBody] CreateRequestEmailDto dto)
        {
            try
            {
                _managerInvestorServices.CreateRequestEmail(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt email
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approve/email")]
        [PermissionFilter(Permissions.CoreQLPD_Email_PheDuyetOrHuy)]
        public APIResponse ApproveEmail([FromBody] ApproveEmailDto dto)
        {
            try
            {
                _managerInvestorServices.ApproveEmail(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo yêu cầu duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("request/phone")]
        [PermissionFilter(Permissions.CoreKHCN_Phone_TrinhDuyet)]
        public APIResponse CreateRequestPhone([FromBody] CreateRequestPhoneDto dto)
        {
            try
            {
                _managerInvestorServices.CreateRequestPhone(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approve/phone")]
        [PermissionFilter(Permissions.CoreQLPD_Phone_PheDuyetOrHuy)]
        public APIResponse ApprovePhone([FromBody] ApprovePhoneDto dto)
        {
            try
            {
                _managerInvestorServices.ApprovePhone(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Reset user password
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("user/reset/password")]
        [PermissionFilter(Permissions.CoreKHCN_Account_ResetPassword)]
        public async Task<APIResponse> ResetUserPassword([FromBody] ResetUserPasswordManagerInvestorDto dto)
        {
            try
            {
                var plainPassword = _managerInvestorServices.ResetUserPassword(dto);

                int? tradingProviderId = null;
                await _sendEmailServices.SendEmailResetPasswordSuccess(null, plainPassword, dto.InvestorId, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("user/status")]
        [PermissionFilter(Permissions.CoreKHCN_Account_ChangeStatus)]
        public APIResponse ChangeUserStatus([FromBody] ChangeUserStatusDto dto)
        {
            try
            {
                _managerInvestorServices.ChangeUserStatus(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chọn ngân hàng mặc định
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("default/bank")]
        [PermissionFilter(Permissions.CoreDuyetKHCN_TKNH_SetDefault, Permissions.CoreKHCN_TKNH_SetDefault)]
        public async Task<APIResponse> SetDefaultBank(SetDefaultBankDto dto)
        {
            try
            {
                _managerInvestorServices.SetDefaultBank(dto);

                int? tradingProviderId = 0;
                try
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor);
                }
                catch (Exception)
                {

                }

                await _sendEmailServices.SendEmailSetBankDefaultSuccess(dto.InvestorBankId, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chọn giấy tờ mặc định
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("default/identification")]
        public APIResponse SetDefaultIdentification(SetDefaultIdentificationDto dto)
        {
            try
            {
                _managerInvestorServices.SetDefaultIdentification(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chọn công ty chứng khoán mặc định
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("default/stock")]
        public APIResponse SetDefaultStock(SetDefaultInvestorStockDto dto)
        {
            try
            {
                _managerInvestorServices.SetDefaultStock(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhât đc liên hệ
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("contact-address")]
        public APIResponse UpdateContactAddress(UpdateContactAddressDto dto)
        {
            try
            {
                _managerInvestorServices.UpdateContactAddress(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Set dc lien he mac dinh
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("default/contact-address")]
        [PermissionFilter(Permissions.CoreDuyetKHCN_DiaChi_SetDefault, Permissions.CoreKHCN_DiaChi_SetDefault)]
        public async Task<APIResponse> SetDefaultContactAddress(SetDefaultManagerInvestorContactAddressDto dto)
        {
            try
            {
                _managerInvestorServices.SetDefaultContactAddress(dto);
                int? tradingProviderId = 0;
                try
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor);
                }
                catch (Exception)
                {

                }
                await _sendEmailServices.SendEmailSetContactAddressDefaultSuccess(dto.ContactAddressId, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Upload ảnh đại diện
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("avatar")]
        public APIResponse UploadAvatar([FromForm] UploadAvatarDto dto)
        {
            try
            {
                var data = _managerInvestorServices.UploadAvatar(dto);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Reset mã pin cho investor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("pin")]
        [PermissionFilter(Permissions.CoreKHCN_Account_ResetPin)]
        public async Task<APIResponse> ResetPin(ResetPinDto dto)
        {
            try
            {
                int? tradingProviderId = null;
                var plainPin = _managerInvestorServices.ResetPin(dto);
                await _sendEmailServices.SendEmailResetPinSuccess(dto.UserId, plainPin, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Reset mã pin cho investor không check trading provider id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("pin-epic")]
        public async Task<APIResponse> ResetPinEpic(ResetPinDto dto)
        {
            try
            {
                var plainPin = _managerInvestorServices.ResetPin(dto);
                await _sendEmailServices.SendEmailResetPinSuccess(dto.UserId, plainPin, null);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chọn tư vấn viên mặc định
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("sale/default")]
        [PermissionFilter(Permissions.CoreKHCN_TuVanVien_SetDefault)]
        public APIResponse SetDefaultSale(UpdateSaleIsDefaultDto dto)
        {
            try
            {
                _managerInvestorServices.SetDefaultSale(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// CMS thực hiện ekyc cho tài khoản chưa qua bước ekyc của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("app-investor/identification")]
        public async Task<APIResponse> UpdateIdentificationAppInvestor([FromBody] UpdateAppInvestorIdentificationDto input)
        {
            try
            {
                await _managerInvestorServices.UpdateAppInvestorIdentification(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá liên kết bank
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("bank-acc")]
        public APIResponse DeleteInvestorBankAcc(DeleteBankAccountDto dto)
        {
            try
            {
                var data = _managerInvestorServices.DeleteBankAccount(dto);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay thế giấy tờ cũ bằng giấy tờ mới
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("identification/replace")]
        public APIResponse ReplaceIdentification(ReplaceIdentificationDto dto)
        {
            try
            {
                var data = _managerInvestorServices.ReplaceIdentification(dto);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật liên kết ngân hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("bank-acc/update")]
        public APIResponse UpdateBankAccount(UpdateInvestorBankAccDto dto)
        {
            try
            {
                var data = _managerInvestorServices.UpdateInvestorBankAcc(dto);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Dlsc get all danh sách investor để duyệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list-temporary")]
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [PermissionFilter(Permissions.CoreDuyetKHCN_DanhSach)]
        public APIResponse GetInvestorTemporary([FromQuery] FindListInvestorDto input)
        {
            try
            {
                var result = _managerInvestorServices.GetListToRequest(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        [Route("list-temporarys")]
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [PermissionFilter(Permissions.CoreDuyetKHCN_DanhSach)]
        public APIResponse GetInvestorTemporarys([FromQuery] FindListInvestorDto input)
        {
            try
            {
                var result = _managerInvestorServices.GetListToRequests(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy list investor thật
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        [PermissionFilter(Permissions.CoreKHCN_DanhSach)]
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        public APIResponse GetListInvestor([FromQuery] FindListInvestorDto input)
        {
            try
            {
                var result = _managerInvestorServices.GetListInvestor(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lọc khách hàng cá nhân (Không dùng cho việc hiển thị ở danh sách khách hàng cá nhân)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("filter")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewManagerInvestorDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetListInvestorfilter([FromQuery] FilterManagerInvestorDto input)
        {
            try
            {
                var result = _managerInvestorServices.FilterInvestor(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy list user theo investor id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list-users")]
        [PermissionFilter(Permissions.CoreKHCN_Account)]
        public APIResponse GetListUsers([FromQuery] FindUserByInvestorId dto)
        {
            try
            {
                var result = _managerInvestorServices.GetListUsers(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get bank paging theo investor id
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list-banks")]
        [PermissionFilter(Permissions.CoreDuyetKHCN_TKNH, Permissions.CoreKHCN_TKNH)]
        public APIResponse GetListBanksPaging(int pageSize, int pageNumber, string keyword, int investorId, bool isTemp)
        {
            try
            {
                var result = _managerInvestorServices.GetBankPaging(pageSize, pageNumber, keyword, investorId, isTemp);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get Contact Address
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list-contact-address")]
        [PermissionFilter(Permissions.CoreDuyetKHCN_DiaChi, Permissions.CoreKHCN_DiaChi)]
        public APIResponse GetContactAddressPaging(int pageSize, int pageNumber, string keyword, int investorId, bool isTemp)
        {
            try
            {
                var result = _managerInvestorServices.GetListContactAddress(pageSize, pageNumber, keyword, investorId, isTemp);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách công ty chứng khoán
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list-stock")]
        //[PermissionFilter(Permissions.CoreDuyetKHCN_DiaChi, Permissions.CoreKHCN_DiaChi)]
        public APIResponse GetListStockNoPaging([FromQuery] FindInvestorStockDto dto)
        {
            try
            {
                var result = _managerInvestorServices.GetListStockNoPaging(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin nhà đầu tư theo investorId
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="isTemp">false: lấy investor thật; true: lấy investor temp</param>
        /// <param name="options"></param>

        /// <returns></returns>
        [HttpGet]
        [Route("{investorId}")]
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER, UserData.SUPER_ADMIN })]
        [PermissionFilter(Permissions.CoreDuyetKHCN_ThongTinKhachHang, Permissions.CoreKHCN_ThongTinKhachHang, Permissions.CoreQLPD_KHCN_ThongTinChiTiet)]
        public APIResponse GetInvestor(int investorId, bool isTemp, [FromQuery] OptionFindDto options)
        {
            try
            {
                var result = _managerInvestorServices.FindById(investorId, isTemp, options);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách investor theo keyword tìm theo phone, cccd, cmnd, passport number
        /// dùng cho tìm kiếm thêm lệnh vào sổ lệnh
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all-list")]
        public APIResponse FindAllList(string keyword)
        {
            try
            {
                var result = _managerInvestorServices.FindAllList(keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy ra danh sách người giới thiệu
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        [HttpGet("list-introduce_users/{investorId}")]
        public APIResponse GetIntroduceUsers(int investorId)
        {
            try
            {
                var result = _managerInvestorServices.GetIntroduceUsers(investorId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Kiểm tra xem có là nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <returns></returns>
        [HttpGet("check-prof")]
        public APIResponse InvestorCheckProf()
        {
            try
            {
                _managerInvestorServices.InvestorCheckProf();
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy ra lịch sử thay đổi khi chỉnh sửa investor
        /// </summary>
        /// <param name="approveId"></param>
        /// <returns></returns>
        [HttpGet("diff/{approveId}")]
        [PermissionFilter(Permissions.CoreQLPD_KHCN_XemLichSu, Permissions.CoreQLPD_KHDN_XemLichSu, Permissions.CoreQLPD_NDTCN_XemLichSu, Permissions.CoreQLPD_Sale_XemLichSu, Permissions.CoreQLPD_Email_XemLichSu, Permissions.CoreQLPD_Phone_XemLichSu)]
        public APIResponse DiffAtRequest(int approveId)
        {
            try
            {
                var result = _managerInvestorServices.GetDiff(approveId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy ds file nhà đầu tư chuyên nghiệp theo id của lần duyệt (investorTempId)
        /// </summary>
        /// <param name="investorTempId"></param>
        /// <returns></returns>
        [HttpGet("{investorTempId}/prof")]
        public APIResponse GetProfById(int investorTempId)
        {
            try
            {
                var result = _managerInvestorServices.GetListProfFile(investorTempId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách sale theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        [HttpGet("sale/{investorId}")]
        [PermissionFilter(Permissions.CoreKHCN_TuVanVien)]
        public APIResponse GetListSaleByInvestorId(int investorId)
        {
            try
            {
                var result = _managerInvestorServices.GetListSale(investorId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy investor tạo từ app (chưa xác minh)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("app-investor")]
        public APIResponse GetListSaleByInvestorId([FromQuery] FindInvestorNoEkycDto dto)
        {
            try
            {
                var result = _managerInvestorServices.FindAppNoEkycInvestor(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa investor và các thông tin liên quan theo InvestorId hoặc phone
        /// </summary>
        [HttpDelete("delete")]
        public APIResponse DeletedInvestor(int? investorId, string phone)
        {
            try
            {
                _managerInvestorServices.DeletedInvestor(investorId, phone?.Trim());
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Nhập mã giới thiệu lần đầu cho nhà đầu tư
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        [HttpPut("update-referral-code")]
        public APIResponse UpdateReferralCode(int investorId, string referralCode)
        {
            try
            {
                _investorV2Services.UpdateReferralCode(investorId, referralCode?.Trim());
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
