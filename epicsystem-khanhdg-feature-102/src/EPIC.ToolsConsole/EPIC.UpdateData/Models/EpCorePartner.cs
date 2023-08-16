using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCorePartner
    {
        public decimal PartnerId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public DateTime? LicenseDate { get; set; }
        public string LicenseIssuer { get; set; }
        public decimal? Capital { get; set; }
        public string RepName { get; set; }
        public string RepPosition { get; set; }
        public string Status { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string TradingAddress { get; set; }
        public string Nation { get; set; }
        public string DecisionNo { get; set; }
        public DateTime? DecisionDate { get; set; }
        public decimal? NumberModified { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
