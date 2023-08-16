using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Integration
{
    /// <summary>
    /// Body gửi đi
    /// </summary>
    public class ProcessIntegrationDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("scriptEnabled")]
        public bool ScriptEnabled { get; set; }

        [JsonPropertyName("urls")]
        public List<string> Urls { get; set; }
    }
}
