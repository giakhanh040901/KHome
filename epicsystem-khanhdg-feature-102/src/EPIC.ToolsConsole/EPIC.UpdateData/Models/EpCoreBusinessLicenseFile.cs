using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreBusinessLicenseFile
    {
        public decimal Id { get; set; }
        public decimal? BusinessCustomerId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public decimal? BusinessCustomerTempId { get; set; }
    }
}
