using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.RocketChat
{
    public class ResponseAddUserToChannelDto
    {
        public ChannelAddUser channel { get; set; }
        public bool success { get; set; }
    }

    public class ChannelAddUser
    {
        public string _id { get; set; }
        public DateTime ts { get; set; }
        public string t { get; set; }
        public string name { get; set; }
        public string[] usernames { get; set; }
        public int msgs { get; set; }
        public DateTime _updatedAt { get; set; }
        public DateTime lm { get; set; }
    }

}
