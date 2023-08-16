using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Invest;
using System;
using System.Collections.Generic;

namespace EPIC.InvestEntities.Dto.PolicyTemp
{
    public class ViewPolicyTemp
    {
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public int? Type { get; set; }
		public decimal? IncomeTax { get; set; }
		public decimal? TransferTax { get; set; }
		public decimal? Classify { get; set; }
		public decimal? MinMoney { get; set; }

		/// <summary>
		/// Số tiền đầu tư tối đa
		/// </summary>
		public decimal? MaxMoney { get; set; }
	    
        /// <summary>
        /// % Phần trăm lợi nhuận cố định: Nếu yêu cầu rút vốn chưa rơi vào kỳ hạn nào
        /// </summary>
        public decimal? ProfitRateDefault { get; set; }

        /// <summary>
        /// Cách tính lợi tức rút vốn: 1: Kỳ hạn thấp hơn gần nhất, 2: Giá trị cố định
        /// <see cref="InvestCalculateWithdrawTypes"/>
        /// </summary>
        public int CalculateWithdrawType { get; set; }

        public string IsTransfer { get; set; }
        public string Status { get; set; }
        public decimal? MinWithdraw { get; set; }
        public decimal? CalculateType { get; set; }
        public decimal? ExitFee { get; set; }
        public decimal? ExitFeeType { get; set; }
        public string Description { get; set; }
        public int? PolicyDisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }

        //PolicyDetailTemp
        public int DeId { get; set; }
        public int? DeStt { get; set; }
        public string DeShortName { get; set; }
        public string DeName { get; set; }
        public string DePeriodType { get; set; }
        public int DePeriodQuantity { get; set; }
        public string DeStatus { get; set; }
        public decimal DeProfit { get; set; }
        public int? DeInterestDays { get; set; }
        public DateTime? DeCreatedDate { get; set; }
        public string DeCreatedBy { get; set; }
        public string DeModifiedBy { get; set; }
        public DateTime? DeModifiedDate { get; set; }
        public int? DeInterestType { get; set; }
        public int? DeInterestPeriodQuantity { get; set; }
        public string DeInterestPeriodType { get; set; }
        public int? DeFixedPaymentDate { get; set; }
        public int RenewalsType { get; set; }
        public int RemindRenewals { get; set; }
        public int ExpirationRenewals { get; set; }
        public decimal MaxWithDraw { get; set; }
        public decimal MinTakeContract { get; set; }
        public int MinInvestDay { get; set; }
        public List<PolicyDetailTempDto> PolicyDetailTemps { get; set; }
    }
}
