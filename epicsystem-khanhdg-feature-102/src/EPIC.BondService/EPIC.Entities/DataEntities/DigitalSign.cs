using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    public class DigitalSign
    {
        public int? BusinessCustomerTempId { get; set; }
        public string Server { get; set; }
        public string Secret { get; set; }
        public string Key { get; set; }
        public string StampImageUrl { get; set; }
        public string ModifiedBy { get; set; }
    }
}
