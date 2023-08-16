using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Livechat
{
    public class LiveChatSendMsgDto
    {
        /// <summary>
        /// Visitor token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// Room id 
        /// </summary>
        public string rid { get; set; }
        /// <summary>
        /// Tin nhắn
        /// </summary>
        public string msg { get; set; }
    }

}
