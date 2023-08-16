using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondBlockadeLiberation
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int Type { get; set; }
        public string BlockadeDescription { get; set; }
        public DateTime? BlockadeDate { get; set; }
        public int OrderId { get; set; }
        public string Blockader { get; set; }
        public DateTime? BlockadeTime { get; set; }
        public string LiberationDescription { get; set; }
        public DateTime? LiberationDate { get; set; }
        public string Liberator { get; set; }
        public DateTime? LiberationTime { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
    }
}
