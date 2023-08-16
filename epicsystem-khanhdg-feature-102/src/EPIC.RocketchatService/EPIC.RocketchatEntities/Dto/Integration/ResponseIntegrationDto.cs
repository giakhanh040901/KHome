using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Integration
{
    public class ResponseIntegrationDto
    {
        [JsonPropertyName("integration")]
        public IntegrationResult Integration { get; set; }
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
    public class CreatedBy
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("_id")]
        public string Id { get; set; }
    }

    public class IntegrationResult
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("urls")]
        public List<string> Urls { get; set; }

        [JsonPropertyName("scriptEnabled")]
        public bool ScriptEnabled { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("channel")]
        public List<object> Channel { get; set; }

        [JsonPropertyName("_createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("_createdBy")]
        public CreatedBy CreatedBy { get; set; }

        [JsonPropertyName("_updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("_id")]
        public string Id { get; set; }
    }
}
