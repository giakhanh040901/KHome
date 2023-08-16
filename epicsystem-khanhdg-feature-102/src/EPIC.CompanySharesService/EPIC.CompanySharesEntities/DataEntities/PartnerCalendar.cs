using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class PartnerCalendar
    {
        public DateTime WorkingDate { get; set; }
        public int WorkingYear { get; set; }
        public DateTime BusDate { get; set; }
        public string IsDayOff { get; set; }
        public int PartnerId { get; set; }
    }
}
