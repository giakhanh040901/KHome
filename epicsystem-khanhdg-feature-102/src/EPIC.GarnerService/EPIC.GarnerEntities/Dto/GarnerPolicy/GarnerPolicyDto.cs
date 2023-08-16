using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerPolicy
{
    public class GarnerPolicyDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int DistributionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal MinMoney { get; set; }
        public decimal? MaxMoney { get; set; }
        public int MinInvestDay { get; set; }
        public decimal IncomeTax { get; set; }
        public string InvestorType { get; set; }
        public int Classify { get; set; }
        public int CalculateType { get; set; }
        public int GarnerType { get; set; }
        public int? InterestType { get; set; }
        public int? InterestPeriodQuantity { get; set; }
        public string InterestPeriodType { get; set; }
        public int? RepeatFixedDate { get; set; }
        public decimal MinWithdraw { get; set; }
        public decimal? MaxWithdraw { get; set; }
        public decimal WithdrawFee { get; set; }
        public int WithdrawFeeType { get; set; }
        public int OrderOfWithdrawal { get; set; }
        public string IsTransferAssets { get; set; }
        public decimal TransferAssetsFee { get; set; }
        public string IsShowApp { get; set; }
        public string Status { get; set; }
        public string IsDefault { get; set; }
        public string IsDefaultEpic { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
    }
}
