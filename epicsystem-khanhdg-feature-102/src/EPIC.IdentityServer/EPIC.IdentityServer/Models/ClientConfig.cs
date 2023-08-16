using System.Collections.Generic;

namespace EPIC.IdentityServer.Models
{
    public class ClientConfig
    {
        public string ClientId { get; set; }
        /// <summary>
        /// Các bí mật để kiểm tra client khi gọi lên endpoint get access token
        /// </summary>
        public List<string> ClientSecrets { get; set; }
        /// <summary>
        /// Access token expire
        /// </summary>
        public int AccessTokenLifetime { get; set; }
        /// <summary>
        /// danh sách scope api cho phép gọi
        /// </summary>
        public List<string> AllowedScopes { get; set; }
        /// <summary>
        /// Cho phép refresh token
        /// </summary>
        public bool AllowOfflineAccess { get; set; }
        /// <summary>
        /// Các grant type được phép nếu không có mặc định sẽ là password và email_phone
        /// </summary>
        public List<string> AllowedGrantTypes { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
        public List<string> RedirectUris { get; set; }
    }
}
