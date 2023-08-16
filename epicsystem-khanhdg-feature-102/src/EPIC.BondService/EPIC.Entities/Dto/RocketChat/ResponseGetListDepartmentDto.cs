using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace EPIC.Entities.Dto.RocketChat
{
    public class ResponseGetListDepartmentDto
    {
        [JsonPropertyName("departments")]
        public List<DepartmentRocketchat> Departments { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }


}
