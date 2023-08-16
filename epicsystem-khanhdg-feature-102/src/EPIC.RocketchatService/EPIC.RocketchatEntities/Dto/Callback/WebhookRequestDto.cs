using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RocketchatEntities.Dto.Callback
{
    public class WebhookRequestDto
    {
        public string _id { get; set; }
        public string label { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime lastMessageAt { get; set; }
        public Visitor visitor { get; set; }
        public Agent agent { get; set; }
        public string type { get; set; }
        public Message[] messages { get; set; }
    }

    //public class Visitor
    //{
    //    public string _id { get; set; }
    //    public string token { get; set; }
    //    public string name { get; set; }
    //    public string username { get; set; }
    //    public Email[] email { get; set; }
    //    public object phone { get; set; }
    //}

    //public class Email
    //{
    //    public string address { get; set; }
    //}

    //public class Agent
    //{
    //    public string _id { get; set; }
    //    public string username { get; set; }
    //    public string name { get; set; }
    //    public string email { get; set; }
    //}

    //public class Message
    //{
    //    public U u { get; set; }
    //    public string _id { get; set; }
    //    public string username { get; set; }
    //    public string msg { get; set; }
    //    public DateTime ts { get; set; }
    //    public string agentId { get; set; }
    //}

    //public class U
    //{
    //    public string _id { get; set; }
    //    public string username { get; set; }
    //    public string name { get; set; }
    //}

}
