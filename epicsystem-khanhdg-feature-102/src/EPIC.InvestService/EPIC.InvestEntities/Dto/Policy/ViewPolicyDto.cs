using EPIC.InvestEntities.Dto.Distribution;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Invest;
using System;
using System.Collections.Generic;

namespace EPIC.InvestEntities.Dto.Policy
{
    public class ViewPolicyDto
    {
		public int Id { get; set; }
		public int TradingProviderId { get; set; }
		public int DistributionId { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public int? Type { get; set; }
		public string IsShowApp { get; set; }

        /// <summary>
        /// % Phần trăm lợi nhuận cố định: Nếu yêu cầu rút vốn chưa rơi vào kỳ hạn nào
        /// </summary>
        public decimal? ProfitRateDefault { get; set; }

        /// <summary>
        /// Cách tính lợi tức rút vốn: 1: Kỳ hạn thấp hơn gần nhất, 2: Giá trị cố định
        /// <see cref="InvestCalculateWithdrawTypes"/>
        /// </summary>
        public int CalculateWithdrawType { get; set; }

        public decimal? IncomeTax { get; set; }
		public decimal? TransferTax { get; set; }
		public decimal? Classify { get; set; }
		public decimal? MinMoney { get; set; }
        /// <summary>
        /// Số tiền đầu tư tối đa
        /// </summary>
        public decimal? MaxMoney { get; set; }
		public string IsTransfer { get; set; }
		public string Status { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Description { get; set; }
		public decimal? MinWithdraw { get; set; }
		public decimal? CalculateType { get; set; }
		public decimal? ExitFee { get; set; }
		public decimal? ExitFeeType { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string Deleted { get; set; }
		public int? PolicyDisplayOrder { get; set; }
		public List<ViewPolicyDetailDto> PolicyDetail { get; set; }
		public DistributionDto Distribution { get; set; }
		public int FakeId { get; set; }
        public int RenewalsType { get; set; }
        public int RemindRenewals { get; set; }
        public int ExpirationRenewals { get; set; }
        public decimal MaxWithDraw { get; set; }
        public decimal MinTakeContract { get; set; }
        public int MinInvestDay { get; set; }
    }

    public class ViewPolicyDetailDto
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public int? STT { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string PeriodType { get; set; }
        public int? PeriodQuantity { get; set; }
        public string Status { get; set; }
        public decimal? Profit { get; set; }
        public string IsShowApp { get; set; }
        public int? InterestDays { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public int FakeId { get; set; }
        public int? InterestType { get; set; }
        public int? InterestPeriodQuantity { get; set; }
        public string InterestPeriodType { get; set; }
        public int? FixedPaymentDate { get; set; }
    }
}
