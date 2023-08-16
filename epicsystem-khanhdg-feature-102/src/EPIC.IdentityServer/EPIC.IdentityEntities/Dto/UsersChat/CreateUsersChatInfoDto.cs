using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.UsersChat
{
    public class CreateUsersChatInfoDto
    {
        public int? UserId { get; set; }
        public string RoomId { get; set; }
        public string RoomToken { get; set; }
        public DateTime? RoomStartDate { get; set; }
        public DateTime? RoomEndDate { get; set; }
        public string AgentId { get; set; }
        public string VisitorId { get; set; }
        public string VisitorToken { get; set; }
        public string LastMessage { get; set; }
    }
}
