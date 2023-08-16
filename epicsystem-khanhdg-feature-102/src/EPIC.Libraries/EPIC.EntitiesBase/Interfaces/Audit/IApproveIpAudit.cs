using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EntitiesBase.Interfaces.Audit
{
    /// <summary>
    /// Người duyệt
    /// </summary>
    public interface IApproveIpAudit
    {
        public string ApproveIp { get; set; }
    }
}
