using System;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class PolicyDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int DistributionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public decimal? IncomeTax { get; set; }
        public decimal? TransferTax { get; set; }
        public int? Classify { get; set; }
        public decimal? MinMoney { get; set; }
        public string IsTransfer { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public int RenewalsType { get; set; }
        public int RemindRenewals { get; set; }
        public int ExpirationRenewals { get; set; }
        public decimal MaxWithDraw { get; set; }
        public decimal MinTakeContract { get; set; }
        public int MinInvestDay { get; set; }
    }
}
