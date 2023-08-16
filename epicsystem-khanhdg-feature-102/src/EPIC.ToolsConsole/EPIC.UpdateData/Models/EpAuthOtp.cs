using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpAuthOtp
    {
        public decimal Id { get; set; }
        public string Phone { get; set; }
        public string OtpCode { get; set; }
        public decimal? UserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string IsActive { get; set; }
    }
}
