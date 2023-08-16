using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvProject
    {
        public decimal Id { get; set; }
        public decimal? PartnerId { get; set; }
        public decimal? OwnerId { get; set; }
        public decimal? GeneralContractorId { get; set; }
        public string InvCode { get; set; }
        public string InvName { get; set; }
        public string Content { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Image { get; set; }
        public string IsPaymentGuarantee { get; set; }
        public string Area { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LocationDescription { get; set; }
        public decimal? TotalInvestment { get; set; }
        public decimal? ProjectType { get; set; }
        public string ProjectProgress { get; set; }
        public string GuaranteeOrganization { get; set; }
        public string IsCheck { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? TotalInvestmentDisplay { get; set; }
        public string HasTotalInvestmentSub { get; set; }
    }
}
