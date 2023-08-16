using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.RocketChat
{
    public class ResponseCreateChannelDto
    {
        public Channel channel { get; set; }
        public bool success { get; set; }
    }

    public class Channel
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string t { get; set; }
        public string[] usernames { get; set; }
        public int msgs { get; set; }
        public U u { get; set; }
        public DateTime ts { get; set; }
    }

    public class U
    {
        public string _id { get; set; }
        public string username { get; set; }
    }

}
