using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.RocketChat
{
    public class ResponseCreateDepartmentDto
    {
        [JsonPropertyName("department")]
        public DepartmentRocketchat Department { get; set; }

        [JsonPropertyName("agents")]
        public object[] Agents { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }

}
