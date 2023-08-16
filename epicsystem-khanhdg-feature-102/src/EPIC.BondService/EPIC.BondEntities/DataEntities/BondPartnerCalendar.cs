using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondPartnerCalendar
    {
        [Key]
        public int PartnerId { get; set; }

        [Key]
        public int WorkingYear { get; set; }

        [Key]
        public DateTime WorkingDate { get; set; }
        
        public DateTime BusDate { get; set; }
        public string IsDayOff { get; set; }
    }
}
