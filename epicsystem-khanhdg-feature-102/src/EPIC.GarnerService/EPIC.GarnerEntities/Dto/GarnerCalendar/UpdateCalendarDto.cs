using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.Calendar
{
    public class UpdateCalendarDto
    {
        public DateTime WorkingDate { get; set; }
        public DateTime? WorkingEndDate { get; set; }
        public string IsDayOff { get; set; }
    }
}
