using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtHistoryUpdate
{
    public class EvtHistoryUpdateDto
    {
        public int Id { get; set; }
        public int UpdateTable { get; set; }
        public int RealTableId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string FieldName { get; set; }
        public int Action { get; set; }
        public string Summary { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
