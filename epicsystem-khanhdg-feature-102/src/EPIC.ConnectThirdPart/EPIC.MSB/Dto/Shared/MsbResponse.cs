using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.Shared
{
    /// <summary>
    /// Base response msb
    /// </summary>
    public class MsbResponse
    {
        [JsonPropertyName("respMessage")]
        public MsbResponseMessage ResponseMessage { get; set; }
    }

    /// <summary>
    /// Base response msb
    /// </summary>
    public class MsbResponse<TResponseDomain> : MsbResponse
    {
        [JsonPropertyName("respDomain")]
        public TResponseDomain ResponseDomain { get; set; }
    }

    /// <summary>
    /// Base Response message msb
    /// </summary>
    public class MsbResponseMessage
    {
        [JsonPropertyName("respCode")]
        public string RespCode { get; set; }

        [JsonPropertyName("respDesc")]
        public string RespDesc { get; set; }
    }
}
