using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Integration
{
    public class AttachmentDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("title_link")]
        public string TitleLink { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }
    }
}
