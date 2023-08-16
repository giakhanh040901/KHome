using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Integration
{
    public class IntegrationDto
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("attachments")]
        public List<AttachmentDto> Attachments { get; set; }
    }
}
