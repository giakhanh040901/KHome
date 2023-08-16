using EPIC.CompanySharesEntities.Dto.CpsInfo;
using EPIC.CompanySharesEntities.Dto.Issuer;
using EPIC.CompanySharesEntities.Dto.JuridicalFile;
using EPIC.CompanySharesEntities.Dto.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsSecondary
{
    /// <summary>
    /// Lấy thông tin của lô
    /// </summary>
    public class AppCpsInfoDto
    {
        public int SecondaryId { get; set; }
        public int CpsId { get; set; }
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
        /// Cổ tức
        /// </summary>
        public decimal? InterestRate { get; set; }
        public decimal? PriceNow { get; set; }
        public string Icon { get; set; }
        public AppIssuerDto Issuer { get; set; }
        public List<AppPolicyFileDto> PolicyFiles { get; set; }
        public AppGuaranteeAssetDto GuaranteeAsset { get; set; }
        public List<JuridicalFileDto> JuridicalFiles { get; set; }
        public List<SecondaryContractFileDto> SecondaryContractFiles { get; set; }
        public AppCpsInfoOverviewDto CpsInfoOverview { get; set; }
    }
}
