using EPIC.BondEntities.Dto;
using EPIC.Entities.Dto.DistributionContract;
using EPIC.Entities.Dto.JuridicalFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ProductBond
{
    /// <summary>
    /// Thông tin chung của Lô trái phiếu theo bán theo kỳ hạn
    /// </summary>
    public class BondInfoSecondaryDto
    {
        public int SecondaryId { get; set; }
        //public int BondId { get; set; }
        public string BondCode { get; set; }
        public string TradingProviderName { get; set; }
        public decimal? Profit { get; set; }
        public string Icon { get; set; }
    }

    public class BondInfoSecondaryFindDto
    {
        public int SecondaryId { get; set; }
        public int TradingProviderId { get; set; }
        public string BondCode { get; set; }
        public string TradingProviderName { get; set; }
        public decimal? Profit { get; set; }
        public string Icon { get; set; }
    }

    /// <summary>
    /// Lấy thông tin của lô
    /// </summary>
    public class AppBondInfoDto
    {
        public int SecondaryId { get; set; }
        public int BondId { get; set; }
        public string BondCode { get; set; }
        public string TradingProviderName { get; set; }
        public decimal? ParValue { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string IsPaymentGuarantee { get; set; }
        public decimal? Profit { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string PolicyPaymentContent { get; set; }
        /// <summary>
        /// Hạn mức còn lại
        /// </summary>
        public decimal? MaxMoney { get; set; }
        /// <summary>
        /// Trái tức
        /// </summary>
        public decimal? InterestRate { get; set; }
        public decimal? PriceNow { get; set; }
        public string Icon { get; set; }
        public AppIssuerDto Issuer { get; set; }
        public List<AppPolicyFileDto> PolicyFiles { get; set; }
        public AppGuaranteeAssetDto GuaranteeAsset { get; set; }
        public List<JuridicalFileDto> JuridicalFiles { get; set; }
        public List<DistributionContractFileDto> DistributionContractFiles { get; set; }
        public AppBondInfoOverviewDto BondInfoOverview { get; set; }
    }
}
