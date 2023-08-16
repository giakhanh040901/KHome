using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class UsersFcmToken
    {
        public decimal Id { get; set; }
        public decimal UserId { get; set; }
        public string FcmToken { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Deleted { get; set; }
    }
}
