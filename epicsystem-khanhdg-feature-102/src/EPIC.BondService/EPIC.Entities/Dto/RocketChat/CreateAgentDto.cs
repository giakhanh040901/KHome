using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.RocketChat
{
    public class CreateAgentDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
