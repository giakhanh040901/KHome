using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    public class ProjectType
    {
        public int? Id { get; set; }
        public int? ProjectId { get; set; }
        public int? Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Deleted { get; set; }
    }
}
