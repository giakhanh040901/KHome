using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.PayMoney
{
    /// <summary>
    /// Response chung cho api chi
    /// </summary>
    public class ResponseRequestPayDto : ResponseRequestPayBaseDto
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }
    }
}
