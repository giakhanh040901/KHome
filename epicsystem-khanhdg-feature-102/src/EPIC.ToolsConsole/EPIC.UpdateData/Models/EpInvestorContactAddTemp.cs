using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvestorContactAddTemp
    {
        public decimal ContactAddressId { get; set; }
        public decimal InvestorId { get; set; }
        public string ContactAddress { get; set; }
        public string IsDefault { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public decimal? InvestorGroupId { get; set; }
        public string DetailAddress { get; set; }
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string WardCode { get; set; }
        public decimal? ReferId { get; set; }
    }
}
