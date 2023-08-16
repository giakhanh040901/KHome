using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.Shared
{
    /// <summary>
    /// Chứa Mid và Tid
    /// </summary>
    public class RequestMsbIdDto
    {
        [JsonPropertyName("mId")]
        public string MId { get; set; }

        [JsonPropertyName("tId")]
        public string TId { get; set; }
    }

    /// <summary>
    /// Request kèm token
    /// </summary>
    public class RequestMsbBaseDto : RequestMsbIdDto
    {
        /// <summary>
        /// access token
        /// </summary>
        [JsonPropertyName("tokenKey")]
        public string TokenKey { get; set; }

        /// <summary>
        /// Đầu số của tài khoản thật
        /// </summary>
        [JsonPropertyName("serviceCode")]
        public string ServiceCode { get; set; }
    }
}
