using EPIC.Entities.Dto.Notification.ResetPassword;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Notification;
using EPIC.Utils.Net.MimeTypes;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ApprovePersonalPaper;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.CreateCustomerInformation;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.FillReferralCode;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ModifyBankAccount;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ModifyPersonalPaper;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ModifyTradingAddressDefault;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.NewDeviceAccessAccount;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.NotifyOTPEmailTemplate;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.NotifyVerifyEmailTemplate;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.RegisterAccount;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.ResetPin;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.SendOTP;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.VerifyEmailDto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService
{
    /// <summary>
    /// Class gọi đến các api của media, cách sử dụng chỉ cần inject vào
    /// </summary>
    public class SharedNotificationApiUtils
    {
        private readonly ILogger _logger;
        private readonly IOptions<SharedApiConfiguration> _sharedApiConfiguration;
        private readonly string _apiNotification;
        private readonly IHttpContextAccessor _httpContext;

        private readonly string ENDPOINT_NOTIFICATION_SEND = "api/media/notification/send-system";

        public SharedNotificationApiUtils(ILogger<SharedMediaApiUtils> logger,
            IHttpContextAccessor httpContext,
            IOptions<SharedApiConfiguration> sharedApiConfiguration)
        {
            _logger = logger;
            _sharedApiConfiguration = sharedApiConfiguration;
            _apiNotification = sharedApiConfiguration.Value.ApiNotification;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Send EMAIL, SMS, PUSH APP
        /// </summary>
        /// <typeparam name="T">DTO</typeparam>
        /// <param name="dto">Data</param>
        /// <param name="key">Key</param>
        /// <param name="receiver">Receiver thông tin người nhận</param>
        /// <param name="attachments">Danh sách file đính kèm dạng list url</param>
        /// <returns></returns>
        public async Task SendEmailAsync<T>(T dto, string key, Receiver receiver, ParamsChooseTemplate otherParam, List<string> attachments = null)
        {
            var body = new SendEmailDto<T>()
            {
                Key = key,
                Data = dto,
                Receiver = receiver,
                Attachments = attachments
            };

            string json = JsonSerializer.Serialize(body);

            await SendEmailAsync(json, otherParam?.TradingProviderId);
        }

        /// <summary>
        /// Gửi email thông báo OTP được gửi tới khách hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task SendEmailOTPSuccessDto(SendOTPContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationSendOTPDto() 
            { 
                Key = KeyTemplate.TK_THONGBAO_OTP,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Gửi email thông báo đăng ký tài khoản thành công
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task RegisterAccountSuccessDto(RegisterAccountContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationRegisterAccountDto()
            {
                Key = KeyTemplate.TK_DANGKY_TK_OK,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Gửi email thông báo thông tin giấy tờ cá nhân được thay đổi thành công
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task ModifyPersonalPaperSuccessDto(ModifyPersonalPaperContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationModifyPersonalPaperDto() 
            {
                Key = KeyTemplate.TK_GIAY_TO_THAY_DOI_OK,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Gửi email thông báo thông tin giấy tờ cá nhân đã được phê duyệt
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task ApprovePersonalPaperSuccessDto(ApprovePersonalPaperContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationApprovePersonalPaperDto()
            {
                Key = KeyTemplate.TK_GIAY_TO_CA_NHAN_PHE_DUYET_OK,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Gửi email thông báo xác minh email thành công
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task VerifyEmailSuccessDto(VerifyEmailContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationVerifyEmailDto()
            {
                Key = KeyTemplate.TK_EMAIL_XACMINH_OK,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Gửi email thông báo thay đổi địa chỉ giao dịch mặc định
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task ModifyTradingAddressSuccessDto(ModifyTradingAddressContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationModifyTradingAddressDefaultDto()
            {
                Key = KeyTemplate.TK_THAYDOI_DIACHI_GIAO_DICH_MACDINH,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Thông báo có thiết bị mới truy cập vào tài khoản
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task NewDeviceAccessAccountDto(NewDeviceAccessAccountContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationNewDeviceAccessAccountDto()
            {
                Key = KeyTemplate.TK_THIETBI_TRUYCAP_TK,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Thông báo thông tin khách hàng khởi tạo thành công
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task CreateCustomerInformationSuccessDto(CreateCustomerInformationContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationCreateCustomerInformationDto()
            {
                Key = KeyTemplate.TK_KHOI_TAO_THANH_CONG,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Thông báo mẫu thông báo OTP qua email
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task NotifyOTPEmailTemplateDto(NotifyOTPEmailTemplateContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationNotifyOTPEmailTemplateDto()
            {
                Key = KeyTemplate.TK_THONG_BAO_OTP_EMAIL,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Thông báo mẫu thông báo xác minh địa chỉ email
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task NotifyVerifyEmailTemplateDto(NotifyVerifyEmailTemplateContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationNotifyVerifyEmailTemplateDto()
            {
                Key = KeyTemplate.TK_XAC_MINH_EMAIL,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Thông báo thay đổi tài khoản ngân hàng thụ hưởng
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task ModifyBankAccountDto(ModifyBankAccountContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationModifyBankAccountDto()
            {
                Key = KeyTemplate.TK_THAY_DOI_TK_THU_HUONG,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        /// <summary>
        /// Thông báo khi có người mới nhập mã giới thiệu của mình
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public async Task FillReferralCodeDto(FillReferralCodeContent dto, Receiver receiver, ParamsChooseTemplate other)
        {
            var body = new NotificationFillReferralCodeDto()
            {
                Key = KeyTemplate.TK_NGUOI_NHAP_MA_GIOI_THIEU,
                Data = dto,
                Receiver = receiver
            };

            string json = JsonSerializer.Serialize(body);
            await SendEmailAsync(json, other?.TradingProviderId);
        }

        private async Task SendEmailAsync(string json, int? tradingProviderId)
        {
            if (tradingProviderId == null)
            {
                tradingProviderId = 0;
            }
            HttpClient httpClient = new();

            var content = new StringContent(json, Encoding.UTF8, MimeTypeNames.ApplicationJson);

            string path = $"{_apiNotification}/{ENDPOINT_NOTIFICATION_SEND}/{tradingProviderId}";

            HttpResponseMessage res = null;
            try
            {
                res = await httpClient.PostAsync(path, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không gọi được api gửi email thông báo: Path = {path}, RequestBody = {json}, exception: {ex.Message}");
            }
            if (res != null)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation($"gọi api gửi thông báo thành công: Path = {path}, RequestBody = {json}, StatusCode = {res.StatusCode}, ResponeBody = {await res.Content.ReadAsStringAsync()}");
                }
                else
                {
                    _logger.LogError($"Không gọi được api gửi thông báo: Path = {path}, RequestBody = {json}, StatusCode = {res.StatusCode}, ResponeBody = {await res.Content.ReadAsStringAsync()}");
                }
            }
        }
    }
}