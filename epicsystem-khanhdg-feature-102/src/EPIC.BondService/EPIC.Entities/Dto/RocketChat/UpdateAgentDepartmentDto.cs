using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace EPIC.Entities.Dto.RocketChat
{
    public class UpdateAgentDepartmentDto
    {
        [JsonPropertyName("upsert")]
        public List<Upsert> Upsert { get; set; }

        [JsonPropertyName("remove")]
        public List<Upsert> Remove { get; set; }
    }

    public class Upsert
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
