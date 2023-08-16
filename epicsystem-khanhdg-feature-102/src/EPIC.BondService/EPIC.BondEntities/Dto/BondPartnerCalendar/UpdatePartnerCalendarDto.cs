using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.PartnerCalendar
{
    public class UpdatePartnerCalendarDto
    {
        public DateTime WorkingDate { get; set; }
        public DateTime BusDate { get; set; }
        public string IsDayOff { get; set; }
    }
}
