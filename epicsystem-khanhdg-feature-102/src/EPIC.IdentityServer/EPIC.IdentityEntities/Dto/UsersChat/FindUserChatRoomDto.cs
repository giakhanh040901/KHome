using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.UsersChat
{
    public class FindUserChatRoomDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "roomId")]
        public string RoomId { get; set; }

        [FromQuery(Name = "visitorId")]
        public string VisitorId { get; set; }
    }
}
