using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.RocketChat
{
    public class AddUserToChannelDto
    {
        public string userId { get; set; }
        public string roomId { get; set; }
    }
}
