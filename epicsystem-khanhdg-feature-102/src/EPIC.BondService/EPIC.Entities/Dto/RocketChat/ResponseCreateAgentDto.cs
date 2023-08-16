using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.RocketChat
{
    public class ResponseCreateAgentDto
    {
        [JsonPropertyName("user")]
        public ResponseCreateAgentUser User { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

    public class ResponseCreateAgentUser
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }



}
