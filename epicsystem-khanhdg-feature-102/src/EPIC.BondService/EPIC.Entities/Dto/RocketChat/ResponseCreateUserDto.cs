using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.RocketChat
{
    public class ResponseCreateUserDto
    {
        public User user { get; set; }
        public bool success { get; set; }
    }

    public class User
    {
        public string _id { get; set; }
        public DateTime createdAt { get; set; }
        public Services services { get; set; }
        public string username { get; set; }
        public Email[] emails { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public bool active { get; set; }
        public string[] roles { get; set; }
        public DateTime _updatedAt { get; set; }
        public string name { get; set; }
        public Settings settings { get; set; }
    }

}
