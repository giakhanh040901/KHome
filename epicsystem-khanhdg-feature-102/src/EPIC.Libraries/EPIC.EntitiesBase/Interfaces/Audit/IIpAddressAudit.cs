using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EntitiesBase.Interfaces.Audit
{
    /// <summary>
    /// Địa chỉ Ip
    /// </summary>
    public interface IIpAddressAudit
    {
        public string IpAddress { get; set; }
    }
}
