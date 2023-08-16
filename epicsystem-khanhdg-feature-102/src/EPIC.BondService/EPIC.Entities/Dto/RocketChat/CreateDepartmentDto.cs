using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.RocketChat
{
    public class CreateDepartmentDto
    {
        [JsonPropertyName("department")]
        public CreateDepartmentRocketchat Department { get; set; }

        [JsonPropertyName("agents")]
        public List<Agent> Agents { get; set; }
    }

    public class CreateDepartmentRocketchat
    {

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("showOnRegistration")]
        public bool ShowOnRegistration { get; set; }

        [JsonPropertyName("showOnOfflineForm")]
        public bool ShowOnOfflineForm { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class DepartmentRocketchat : CreateDepartmentRocketchat
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
    }

    public class Agent
    {
        [JsonPropertyName("agentId")]
        public string AgentId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }
    }

}
