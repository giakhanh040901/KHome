using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Message
{
    public class SendMessageDto
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string rid { get; set; }
        public string msg { get; set; }
    }

}
