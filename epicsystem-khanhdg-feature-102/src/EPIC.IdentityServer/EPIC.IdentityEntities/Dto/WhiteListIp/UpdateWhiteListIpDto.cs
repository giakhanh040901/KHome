using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.WhiteListIp
{
    public class UpdateWhiteListIpDto
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public List<UpdateWhiteListIPDetailDto> whiteListIPDetails { get; set; }
    }

    public class UpdateWhiteListIPDetailDto
    {
        public int Id { get; set; }
        public string IpAddressStart { get; set; }
        public string IpAddressEnd { get; set; }
    }
}
