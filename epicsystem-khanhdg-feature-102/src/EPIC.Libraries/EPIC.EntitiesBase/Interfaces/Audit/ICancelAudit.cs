using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EntitiesBase.Interfaces.Audit
{
    /// <summary>
    /// Người huỷ
    /// </summary>
    public interface ICancelAudit
    {
        string CancelBy { get; set; }
        DateTime? CancelDate { get; set; }
    }
}
