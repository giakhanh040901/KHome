using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Livechat
{
    public class ResponseLiveChatSendMsgDto
    {
        public Message message { get; set; }
        public bool success { get; set; }
    }

    public class Message
    {
        public string _id { get; set; }
        public string rid { get; set; }
        public string msg { get; set; }
        public string token { get; set; }
        public string alias { get; set; }
        public DateTime ts { get; set; }
        public U u { get; set; }
        public object[] urls { get; set; }
        public object[] mentions { get; set; }
        public object[] channels { get; set; }
        public Md[] md { get; set; }
        public DateTime _updatedAt { get; set; }
    }

    public class U
    {
        public string _id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
    }

    public class Md
    {
        public string type { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        public string type { get; set; }
        public string value { get; set; }
    }

}
