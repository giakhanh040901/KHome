using EPIC.MSB.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.CollectMoney
{
    public class RequestUpdateVADto : RequestMsbBaseDto
    {
        [JsonPropertyName("rows")]
        public List<UpdateVARowDto> Rows { get; set; }
    }

    public class UpdateVARowDto : CreateVARowDto { }
}
