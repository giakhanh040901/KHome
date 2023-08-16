using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.RegisterAccount;
using System.Text.Json.Serialization;

namespace EPIC.Notification.Dto.IdentityNotification
{
    /// <summary>
    /// Nội dung Noti xác minh thông tin tài khoản khách hàng
    /// </summary>
    public class VerificationAccountSuccessContent : RegisterAccountContent
    {
        [JsonPropertyName("FinalStepDate")]
        public string FinalStepDate { get; set; }
    }
}
