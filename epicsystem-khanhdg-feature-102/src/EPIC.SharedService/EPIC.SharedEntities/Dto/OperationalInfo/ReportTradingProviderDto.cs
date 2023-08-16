using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedEntities.Dto.OperationalInfo
{
    public class ReportTradingProviderDto
    {
        public int Id { get; set; }
        public int BusinessCustomerId { get; set; }
        public int? Status { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public string IsShow { get; set; }
    }
}
