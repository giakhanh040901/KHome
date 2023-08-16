using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Callback
{
    public class WebhookLivechatDto
    {
        public string _id { get; set; }
        public string label { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime lastMessageAt { get; set; }
        public string[] tags { get; set; }
        public Visitor visitor { get; set; }
        public Agent agent { get; set; }
        public string type { get; set; }
        public Message[] messages { get; set; }
        public Servedby servedBy { get; set; }
        public DateTime closedAt { get; set; }
        public Closedby closedBy { get; set; }
        public string closer { get; set; }
    }

    public class Visitor
    {
        public string _id { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public Email[] email { get; set; }
        public object phone { get; set; }
    }

    public class Email
    {
        public string address { get; set; }
    }

    public class Agent
    {
        public string _id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

    public class Servedby
    {
        public string _id { get; set; }
        public string username { get; set; }
        public DateTime ts { get; set; }
    }

    public class Closedby
    {
        public string _id { get; set; }
        public string username { get; set; }
    }

    public class Message
    {
        public U u { get; set; }
        public string _id { get; set; }
        public string username { get; set; }
        public string msg { get; set; }
        public DateTime ts { get; set; }
        public string agentId { get; set; }
        public bool closingMessage { get; set; }
    }

    public class U
    {
        public string _id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
    }

}
