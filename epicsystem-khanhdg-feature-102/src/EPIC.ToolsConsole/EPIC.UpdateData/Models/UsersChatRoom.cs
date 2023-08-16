using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class UsersChatRoom
    {
        public decimal Id { get; set; }
        public decimal? UserId { get; set; }
        public string RoomId { get; set; }
        public string RoomToken { get; set; }
        public DateTime? RoomStartDate { get; set; }
        public DateTime? RoomEndDate { get; set; }
        public string AgentId { get; set; }
        public string VisitorId { get; set; }
        public string VisitorToken { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Deleted { get; set; }
    }
}
