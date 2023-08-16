using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Integration
{
    public class IntegrationDirectDto : IntegrationDto
    {
        [JsonPropertyName("receiverPhone")]
        public string ReceiverPhone { get; set; }
    }
}
