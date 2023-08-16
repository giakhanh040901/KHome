using EPIC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class CpsSecondary : EntityTypeMapper
    {
        public int Id { get; set; }
        public int CpsId { get; set; }
        public int TradingProviderId { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public long? Quantity { get; set; }
        public int Status { get; set; }
        public DateTime? OpenCellDate { get; set; }
        public DateTime? CloseCellDate { get; set; }
        public string IsClose { get; set; }
        public string IsShowApp { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string IsCheck { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        public string OverviewImageUrl { get; set; }
    }

    /// <summary>
    /// KET QUA TRA RA TU PROC GET ALL
    /// </summary>
    public class CpsSecondaryView : EntityTypeMapper
    {
        // SECONDARY
        public int CpsSecondaryId { get; set; }
        public int CpsPrimaryId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime HoldDate { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string IsCheck { get; set; }
        public DateTime ModifiedDate { get; set; }
        // POLICY
        public int CpsPolicyId { get; set; }
        public int TradingProviderId { get; set; }
        public string PolicyCode { get; set; }
        public string PolicyName { get; set; }
        public int PolicyType { get; set; }
        public string PolicyTypeInvestor { get; set; }
        public int PolicyNumberClosePer { get; set; }
        public decimal PolicyIncomeTax { get; set; }
        public decimal PolicyMinMoney { get; set; }
        public string PolicyStatus { get; set; }
        // POLICY DETAIL
        public int CpsPolicyDetailId { get; set; }
        public string DetailCode { get; set; }
        public string DetailName { get; set; }
        public decimal DetailType { get; set; }
        public string DetailInterestPeriodType { get; set; }
        public decimal DetailInterestPeriod { get; set; }
        public string DetailTypeInvestor { get; set; }
        public decimal DetailNumberClosePer { get; set; }
        public decimal DetailIncomeTax { get; set; }
        public decimal DetailMinMoney { get; set; }
        public string DetailStatus { get; set; }
        // PRIMARY
        public string PrimaryName { get; set; }
    }
}
