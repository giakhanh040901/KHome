using EPIC.BondDomain.Interfaces;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreDomain.Interfaces.v2;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
//using EPIC.CoreEntities.Dto.Investor;
//using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.User;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.Dto.UsersChat;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.IdentityServer.Controllers
{
    /// <summary>
    /// api cho app đăng ký tài khoản nhà đầu tư
    /// </summary>
    [Route("api/users/investor-v2")]
    [ApiController]
    public class UserInvestorV2Controller : BaseController
    {
        private readonly IUserServices _userService;
        private readonly IInvestorV2Services _investorServices;
        private readonly NotificationServices _sendEmailServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInvestorV2Controller(
            ILogger<UserInvestorController> logger,
            IUserServices userService,
            IInvestorV2Services investorServices,
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
        /// <summary>
        /// Khách hàng đăng ký tài khoản với phone và email qua app
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public APIResponse RegisterInventor([FromBody] RegisterInvestorDto model)
        {
            try
            {
                _investorServices.RegisterInvestor(model, InvestorSource.ONLINE);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy otp sms luồng đký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Lấy otp email luồng đký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Khách hàng xác thực otp luồng đăng ký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("verify-code")]
        public APIResponse VerifyCodeAsync([FromBody] VerificationCodeDto input)
        {
            try
            {
                _investorServices.VerifyCode(input);
                //var investor = _investorServices.GetByEmailOrPhone(input.Phone);
                //var user = _userService.FindByInvestorId(investor.InvestorId);

                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Quét giấy tờ khi khách hàng đăng ký app
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("ekyc-ocr")]
        public async Task<APIResponse> EkycOCR([FromForm] EKYCOcrDto input)
        {
            try
            {
                _investorServices.CheckTooManyRequestIpAddress(input.Phone);
                var result = await _investorServices.EkycOCRAsync(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin quét giấy tờ trong ảnh
        /// </summary>
        [HttpPost("ocr-info")]
        public async Task<APIResponse> CheckInfoOCRAsync([FromForm] AddIdentificationDto input)
        {
            try
            {
                var result = await _investorServices.CheckInfoOCRAsync(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Khách hàng app xác nhận thông tin ekyc giấy tờ khi đăng ký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        /// Khách hàng đăng ký liên kết ngân hàng từ app ở luồng đăng ký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-bank-account")]
        public async Task<APIResponse> AddBankAccount([FromBody] BankAccountDto input)
        {
            try
            {
                _investorServices.AddBankAccount(input);

                //Gửi email thông báo đăng ký thành công
                await _sendEmailServices.SendNotifyEnterReferralWhenRegister(input.Phone);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Kiểm tra xem investor đã thực hiện đến bước nào của step rồi
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet("register-step/{phone}")]
        public APIResponse GetInvestorStep(string phone)
        {
            try
            {
                var data = _investorServices.GetInvestorStep(phone);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Gửi thông báo đăng ký tài khoản thành công
        /// <summary>
        /// Gửi thông báo đăng ký tài khoản thành công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("notification/register-success")]
        public async Task<APIResponse> NotificationRegisterSuccess([FromBody] NotificationRegisterSuccessDto model)
        {
            try
            {
                await _sendEmailServices.SendEmailRegisterSuccess(model.Phone);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gửi thông báo xác minh tài khoản thành công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("notification/verify-account-success")]
        public async Task<APIResponse> NotificationVerificationAccountSuccess([FromBody] NotificationRegisterSuccessDto model)
        {
            try
            {
                await _sendEmailServices.SendEmailVerificationAccountSuccess(model.Phone);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Face match
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

        /// <summary>
        /// Lưu ảnh mặt khi nhận diện (quay trái, quay phải, nháy mắt, cười)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("face-image")]
        public async Task<APIResponse> SaveFaceMatchImage([FromForm] SaveFaceMatchImageDto dto)
        {
            try
            {
                await _investorServices.UploadFaceImage(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("register-now")]
        public APIResponse RegisterNow()
        {
            try
            {
                _investorServices.RegisterNow();
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Api Sau khi đã đăng ký xong tài khoản
        /// <summary>
        /// Xác nhận thông tin ekyc của giấy tờ (Dành cho tài khoản đã đăng ký xong và vừa thêm mới 1 giấy tờ khác)
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

        #region pin
        /// <summary>
        /// Kiểm tra PIN. Sai quá số lần quy định (5) trong sysvar thì khóa luôn tài khoản
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

        #region get thông tin liên quan
        [HttpGet("trading")]
        [ProducesResponseType(typeof(APIResponse<ViewTradingInfoDto>), (int)HttpStatusCode.OK)]
        public APIResponse GetTrading()
        {
            try
            {
                var data = _investorServices.GetListTrading();
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
    }
}
