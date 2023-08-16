using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Integration
{
    public class IntegrationContent
    {
        [JsonPropertyName("payload")]
        public IntegrationDto Payload { get; set; }
        [JsonPropertyName("sender")]
        public InvestorDirectMsgDto Sender { get; set; }
    }

    public class InvestorDirectMsgDto
    {
        [JsonPropertyName("investorId")]
        public int InvestorId { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("avatarImageUrl")]
        public string AvatarImageUrl { get; set; }
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
    }
}
