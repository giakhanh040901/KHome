using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.RocketChat
{
    public class CreateChannelDto
    {
        public string name { get; set; }
        public string[] members { get; set; }
        public bool readOnly { get; set; }
    }
}
