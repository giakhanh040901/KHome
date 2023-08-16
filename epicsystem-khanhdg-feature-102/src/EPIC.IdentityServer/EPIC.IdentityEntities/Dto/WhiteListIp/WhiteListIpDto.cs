using EPIC.IdentityEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.WhiteListIp
{
    public class WhiteListIpDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int? TradingProviderId { get; set; }
        public List<WhiteListIpDetail> WhiteListIPDetails { get; set; }

    }
}
