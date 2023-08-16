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
    public interface IApproveAudit
    {
        public string ApproveBy { get; set; }
        public DateTime? ApproveDate { get; set; }
    }
}
