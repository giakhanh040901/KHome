﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.PayMoney
{
    public class RequestProcessTransferDto
    {
        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }

        [JsonPropertyName("mid")]
        public string MId { get; set; }

        [JsonPropertyName("tid")]
        public string TId { get; set; }

        [JsonPropertyName("otp")]
        public string Otp { get; set; }

        [JsonPropertyName("secureHash")]
        public string SecureHash { get; set; }
    }
}
