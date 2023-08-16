using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DigitalSign
{
    public class DigitalSignDto
    {
        public string Server { get; set; }
        public string Secret { get; set; }
        public string Key { get; set; }
        public string StampImageUrl { get; set; }
    }
}
