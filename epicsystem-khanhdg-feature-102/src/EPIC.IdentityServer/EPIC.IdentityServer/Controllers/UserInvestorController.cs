using EPIC.CoreDomain.Interfaces;
using EPIC.CoreSharedEntities.Dto.Investor;
//using EPIC.CoreEntities.Dto.Investor;
//using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.User;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.Dto.UsersChat;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.IdentityServer.Controllers
{
    /// <summary>
    /// api cho app đăng ký tài khoản nhà đầu tư
    /// </summary>
    [Route("api/users/investor")]
    [ApiController]
    public class UserInvestorController : BaseController
    {
        private readonly IUserServices _userService;
        private readonly IInvestorServices _investorServices;
        private readonly NotificationServices _sendEmailServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInvestorController(
            ILogger<UserInvestorController> logger,
            IUserServices userService,
            IInvestorServices investorServices,
            NotificationServices sendEmailServices,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userService = userService;
            _investorServices = investorServices;
            _sendEmailServices = sendEmailServices;
            _httpContextAccessor = httpContextAccessor;
        }

        #region đăng ký nhà đầu tư
        [HttpPost("create-verification-code-send-sms")]
        public async Task<APIResponse> CreateVerificationCodeSendSmsAsync([FromBody] InvestorEmailPhoneDto input)
        {
            try
            {
                await _investorServices.CreateVerificationCodeSendSmsAsync(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("create-verification-code-send-mail")]
        public async Task<APIResponse> CreateVerificationCodeSendMailAsync([FromBody] InvestorEmailPhoneDto input)
        {
            try
            {
                await _investorServices.CreateVerificationCodeSendMailAsync(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("verify-code")]
        public APIResponse VerifyCodeAsync([FromBody] VerificationCodeDto input)
        {
            try
            {
                _investorServices.VerifyCode(input);
                var investor = _investorServices.GetByEmailOrPhone(input.Phone);
                var user = _userService.FindByInvestorId(investor.InvestorId);

                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("register")]
        public APIResponse RegisterInventor([FromBody] RegisterInvestorDto model)
        {
            try
            {
                _investorServices.RegisterInventor(model);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("ekyc-ocr")]
        public async Task<APIResponse> EkycOCR([FromForm] EKYCOcrDto input)
        {
            try
            {
                var result = await _investorServices.EkycOCRAsync(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("ekyc-confirm-info")]
        public APIResponse EkycConfirm([FromBody] EKYCConfirmInfoDto input)
        {
            try
            {
                _investorServices.EkycConfirmInfo(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Nhận diện gương mặt trong luồng đăng ký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("ekyc-face-match")]
        public async Task<APIResponse> EkycFaceMatch([FromForm] EKYCFaceMatchDto input)
        {
            try
            {
                var result = await _investorServices.EkycFaceMatchAsync(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Nhận diện gương mặt sau khi login
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("ekyc-face-match/logged-in")]
        [Authorize]
        public async Task<APIResponse> EkycFaceMatchLoggedIn([FromForm] FaceRecognitionLoggedInDto input)
        {
            try
            {
                var result = await _investorServices.EkycFaceMatchLoggedInAsync(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("add-bank-account")]
        public async Task<APIResponse> AddBankAccount([FromBody] BankAccountDto input)
        {
            try
            {
                _investorServices.AddBankAccount(input);
                //Gửi email thông báo đăng ký thành công
                await _sendEmailServices.SendEmailRegisterSuccess(input.Phone);
                await _sendEmailServices.SendNotifyEnterReferralWhenRegister(input.Phone);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region mật khẩu nhà đầu tư
        /// <summary>
        /// quên mật khẩu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("forgot-password")]
        public async Task<APIResponse> ForgotPassword([FromBody] ForgotPasswordDto input)
        {
            try
            {
                await _investorServices.ForgotPasswordAsync(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Kiểm tra otp quên mật khẩu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("verify-otp")]
        public APIResponse VerifyOTP([FromBody] VerifyOTPDto input)
        {
            try
            {
                var result = _investorServices.VerifyOTPResetPass(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Khôi phục mật khẩu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("reset-password")]
        public APIResponse ResetPassword([FromBody] ResetPasswordDto input)
        {
            try
            {
                _investorServices.ResetPassword(input);
                //await _sendEmailServices.SendEmailResetPasswordSuccess(input.EmailOrPhone, input.Password);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpPut("change-password")]
        public APIResponse ChangePassword([FromBody] ChangePasswordDto input)
        {
            try
            {
                _investorServices.ChangePassword(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpPut("change-password-temp")]
        public APIResponse ChangePasswordTemp([FromBody] ChangePasswordTempDto input)
        {
            try
            {
                _investorServices.ChangePasswordTemp(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpPut("change-pin")]
        public APIResponse ChangePin([FromBody] ChangePinDto input)
        {
            try
            {
                _investorServices.ChangePin(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpPost("validate-pin")]
        public APIResponse ValidatePin([FromBody] ValidatePinDto input)
        {
            try
            {
                var result = _investorServices.ValidatePin(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region thông tin nhà đầu tư

        /// <summary>
        /// Thêm giấy tờ tùy thân
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("add/identification")]
        public async Task<APIResponse> AddIdentification([FromForm] AddIdentificationDto input)
        {
            try
            {
                var result = await _investorServices.AddIdentification(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chọn giấy tờ mặc định
        /// </summary>
        /// <param name="identificationId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("default/identification/{identificationId}")]
        public APIResponse SetDefaultIdentification(int identificationId)
        {
            try
            {
                _investorServices.SetDefaultIdentification(identificationId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy list giấy tờ của investor
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("identification/list")]
        public APIResponse GetListIdentification([FromQuery] AppGetListIdentificationDto dto)
        {
            try
            {
                var result = _investorServices.GetListIdentification(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy giấy tờ mặc định
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("identification/default")]
        public APIResponse GetDefaultIdentification()
        {
            try
            {
                var result = _investorServices.GetDefaultIdentification();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin user để hiển thị ở màn hình Thông tin người dùng (App)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("info")]
        public APIResponse GetMyInfo()
        {
            try
            {
                var result = _investorServices.GetMyInfo();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// App cập nhật ảnh đại diện
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("avatar")]
        public APIResponse UploadAvatar([FromForm] AppUploadAvatarDto dto)
        {
            try
            {
                _investorServices.UploadAvatar(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xác nhận thông tin ekyc của giấy tờ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("confirm-identification")]
        [Authorize]
        public APIResponse ConfirmIdentificationEkyc([FromBody] ConfirmIdentificationEkycDto input)
        {
            try
            {
                _investorServices.ConfirmIdentificationEkyc(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion

        #region lấy thông tin đặt lệnh
        /// <summary>
        /// Lấy thông tin giấy tờ
        /// </summary>
        /// <returns></returns>
        [HttpGet("id-info")]
        public APIResponse GetListIdInfo()
        {
            try
            {
                var result = _investorServices.GetListIdInfo();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin địa chỉ giao dịch
        /// </summary>
        /// <returns></returns>
        [HttpGet("trans-address")]
        public APIResponse GetListTransactionAddess()
        {
            try
            {
                var result = _investorServices.GetListTransactionAddress();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm địa chỉ giao dịch
        /// </summary>
        /// <returns></returns>
        [HttpPost("add-trans-address")]
        public APIResponse AddTransactionAddess()
        {
            try
            {
                _investorServices.AddTransactionAddess();
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region sinh otp
        [Authorize]
        [HttpPost("generate-otp-mail")]
        public async Task<APIResponse> GenerateOtpMail()
        {
            try
            {
                _investorServices.GenerateOtpMail();
                await _sendEmailServices.SendEmailOtpEmail();
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpPost("generate-otp-sms")]
        public async Task<APIResponse> GenerateOtpSms()
        {
            try
            {
                _investorServices.GenerateOtpSms();
                await _sendEmailServices.SendEmailOtp();
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Tạo OTP giao nhận hợp đồng
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpPost("generate-otp-sms-by-phone/{phone}/{tradingProviderId}")]
        public async Task<APIResponse> GenerateOtpSmsByPhone(string phone, int tradingProviderId)
        {
            try
            {
                _investorServices.GenerateOtpSmsByPhone(phone);
                await _sendEmailServices.SendEmailOtpByPhone(phone, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region contact address
        /// <summary>
        /// Tạo đc liên hệ
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("add/contact-address")]
        public APIResponse AddContactAddress(CreateContactAddressDto dto)
        {
            try
            {
                _investorServices.AddContactAddress(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật đc liên hệ
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update/contact-address")]
        public async Task<APIResponse> UpdateContactAddress(UpdateContactAddressDto dto)
        {
            try
            {
                await _investorServices.UpdateContactAddress(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chọn đc liên hệ mặc định
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("default/contact-address")]
        public async Task<APIResponse> SetDefaultContactAddress(SetDefaultContactAddressDto dto)
        {
            try
            {
                _investorServices.SetDefaultContactAddress(dto);
                await _sendEmailServices.SendEmailSetContactAddressDefaultSuccess(dto.ContactAddressId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get list đc liên hệ 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("contact-address")]
        public APIResponse GetListContactAddress(string keyword)
        {
            try
            {
                var result = _investorServices.GetListContactAddress(keyword);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion

        #region Tài khoản ngân hàng
        /// <summary>
        /// Danh sách tài khoản ngân hàng đã được liên kết
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("list/bank")]
        public APIResponse GetListBank()
        {
            try
            {
                var result = _investorServices.GetListBank();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm liên kết tài khoản ngân hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("add/bank")]
        public APIResponse AddBank(CreateBankDto dto)
        {
            try
            {
                _investorServices.AddBank(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chọn ngân hàng mặc định
        /// </summary>
        /// <param name="investorBankId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("default/{investorBankId}")]
        public async Task<APIResponse> SetDefaultBank(int investorBankId)
        {
            try
            {
                _investorServices.SetBankDefault(investorBankId);
                await _sendEmailServices.SendEmailSetBankDefaultSuccess(investorBankId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá tk ngân hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("bank/{id}")]
        public APIResponse DeleteBankAccount(int id)
        {
            try
            {
                _investorServices.DeleteBankAccount(new AppDeleteBankDto
                {
                    Id = id
                });
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Nhà đầu tư chuyên nghiệp
        /// <summary>
        /// Upload file nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("upload/prof")]
        public APIResponse UploadProfFile([FromForm] UploadProfFileDto dto)
        {
            try
            {
                _investorServices.UploadProfFile(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Xác nhận email
        /// <summary>
        /// Gửi thông báo xác nhận email
        /// </summary>
        /// <returns></returns>
        [HttpPost("send-verify-email")]
        [Authorize]
        public async Task<APIResponse> SendVerifyEmail()
        {
            try
            {
                await _sendEmailServices.SendEmailVerify();
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đường link xác thực email
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        [HttpGet("verify-email/{verifyCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(string verifyCode)
        {
            try
            {
                _investorServices.VerifyEmail(verifyCode);
                await _sendEmailServices.SendEmailVerifyEmailSuccess(verifyCode);
                return Redirect("/api/users/view/verify-email-success");
            }
            catch (Exception ex)
            {
                var err = OkException(ex);
                return Ok($"Error: {err?.Message}");
            }

        }
        #endregion

        #region Mã giới thiệu
        /// <summary>
        /// Đăng ký mã giới thiệu lần đầu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("referral-code/register")]
        [Authorize]
        public async Task<APIResponse> ScanReferralCodeFirstTime([FromBody] ScanReferralCodeFirstTimeDto input)
        {
            try
            {
                _investorServices.ScanReferralCodeFirstTime(input);
                await _sendEmailServices.SendNotifyEnterReferral(CommonUtils.GetCurrentInvestorId(_httpContextAccessor));
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách mã giới thiệu sale
        /// </summary>
        /// <returns></returns>
        [HttpGet("referral-code/all")]
        [Authorize]
        public APIResponse GetListReferralCode()
        {
            try
            {
                var data = _investorServices.GetListReferralCodeSale();
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy một vài thông tin tư vấn viên theo mã giới thiệu của chính người đó
        /// </summary>
        /// <param name="referralCodeSelf"></param>
        /// <returns></returns>
        [HttpGet("referral-code/info/{referralCodeSelf}")]
        public APIResponse GetSaleInfoByReferralCode(string referralCodeSelf)
        {
            try
            {
                var data = _investorServices.GetShortSalerInfoByReferralCodeSelf(referralCodeSelf);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Mã giới thiệu có tồn tại không
        /// </summary>
        /// <param name="referralCodeSelf"></param>
        /// <returns></returns>
        [HttpGet("referral-code/exists/{referralCodeSelf}")]
        public APIResponse IsReferralCodeExist(string referralCodeSelf)
        {
            try
            {
                var data = _investorServices.isReferralCodeExist(referralCodeSelf);
                var code = data ? Utils.StatusCode.Success : Utils.StatusCode.Error;
                return new APIResponse(code, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Quét mã giới thiệu của sale
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("referral-code/sale")]
        [Authorize]
        public APIResponse AddReferralCodeSale([FromBody] ScanReferralCodeSaleDto input)
        {
            try
            {
                _investorServices.ScanReferralCodeSale(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chọn mã giới thiệu mặc định
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("referral-code/default/{id}")]
        [Authorize]
        public APIResponse SetDefaultReferralCode(decimal id)
        {
            try
            {
                _investorServices.SetDefaultReferralCodeSale(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion

        #region Khoá tài khoản
        /// <summary>
        /// Tự sát. Tự khóa tài khoản của chính mình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("self/lock")]
        [Authorize]
        public APIResponse LockMyAccount([FromBody] AppDeactiveMyUserAccountDto input)
        {
            try
            {
                _investorServices.DeactiveMyUserAccount(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region App thông tin trò chuyện
        /// <summary>
        /// App rc lưu thông tin trò chuyện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("chat/room-info")]
        [Authorize]
        public APIResponse SaveUserChatRoomInfo([FromBody] CreateUsersChatInfoDto input)
        {
            try
            {
                _userService.SaveUserChatRoomInfo(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// App rc lấy thông tin trò chuyện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("chat/room-info")]
        [Authorize]
        public APIResponse GetUserChatRoomInfo([FromQuery] FindUserChatRoomDto input)
        {
            try
            {
                var data = _userService.GetUserChatRoomInfo(input);
                var status = data?.TotalItems > 0 ? Utils.StatusCode.Success : Utils.StatusCode.Error;
                return new APIResponse(status, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion
    }
}
