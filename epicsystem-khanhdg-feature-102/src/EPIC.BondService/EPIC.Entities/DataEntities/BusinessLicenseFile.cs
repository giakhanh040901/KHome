using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    public class BusinessLicenseFile : IFullAudited
    {
        public int Id { get; set; }
        public int? BusinessCustomerId { get; set; }
        public int? BusinessCustomerTempId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
    }
}
