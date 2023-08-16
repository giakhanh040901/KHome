using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.Notification
{
    public class ReceiveNotificationPaymentDto : ReceiveNotificationPaymentBaseDto
    {
        [JsonPropertyName("secureHash")]
        public string SecureHash { get; set; }
    }
}
