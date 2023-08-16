using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpProductBondPolicyDeTemp
    {
        public decimal BondPolicyDetailTempId { get; set; }
        public decimal BondPolicyTempId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public decimal? Stt { get; set; }
        public decimal? PeriodQuantity { get; set; }
        public string PeriodType { get; set; }
        public decimal? InterestType { get; set; }
        public decimal? InterestPeriodQuantity { get; set; }
        public string InterestPeriodType { get; set; }
        public decimal? Profit { get; set; }
        public decimal? InterestDays { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
    }
}
