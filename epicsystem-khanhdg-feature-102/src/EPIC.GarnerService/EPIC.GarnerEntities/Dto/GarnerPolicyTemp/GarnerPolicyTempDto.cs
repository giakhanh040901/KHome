using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp;
using System.Collections.Generic;

namespace EPIC.GarnerEntities.Dto.GarnerPolicyTemp
{
    public class GarnerPolicyTempDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal MinMoney { get; set; }
        public int MinInvestDay { get; set; }
        public decimal? MaxMoney { get; set; }
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
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public string Status { get; set; }

        public List<GarnerPolicyDetailTempDto> PolicyDetails { get; set; }
        public List<GarnerContractTemplateTempDto> ContractTemplateTemps { get; set; }
    }
}
