using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Utils.Recognition.FPT
{
    public class FaceMatchResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class FaceMatchData
    {
        [JsonPropertyName("isMatch")]
        public bool IsMatch { get; set; }

        [JsonPropertyName("similarity")]
        public double Similarity { get; set; }

        [JsonPropertyName("isBothImgIDCard")]
        public bool IsBothImgIDCard { get; set; }
    }
}
