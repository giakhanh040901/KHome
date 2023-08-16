using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerSharedEntities.Dto
{
    public class GarnerOrderDto
    {
        public long Id { get; set; }
        public int TradingProviderId { get; set; }
        public string CifCode { get; set; }
        public int? DepartmentId { get; set; }
        public string CustomerName { get; set; }
        public int ProductId { get; set; }
        public int DistributionId { get; set; }
        public int PolicyId { get; set; }
        public int? PolicyDetailId { get; set; }
        public decimal TotalValue { get; set; }
        public decimal InitTotalValue { get; set; }
        public DateTime BuyDate { get; set; }
        public int Status { get; set; }
        public int Source { get; set; }
        public string ContractCode { get; set; }
        public int TradingBankAccId { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public DateTime? PaymentFullDate { get; set; }
        public int? InvestorBankAccId { get; set; }
        public string SaleReferralCode { get; set; }
        public int? DeliveryStatus { get; set; }
        public string IpAddressCreated { get; set; }
        public int InvestorIdenId { get; set; }
        public int? ContractAddressId { get; set; }
        public string DeliveryCode { get; set; }
        public DateTime? ActiveDate { get; set; }
        public DateTime? InvestDate { get; set; }
        public DateTime? SettlementDate { get; set; }
        public int? SaleOrderId { get; set; }
        public DateTime? PendingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public string PendingDateModifiedBy { get; set; }
        public string DeliveryDateModifiedBy { get; set; }
        public string ReceivedDateModifiedBy { get; set; }
        public string FinishedDateModifiedBy { get; set; }
        public int? DepartmentIdSub { get; set; }
        public string SaleReferralCodeSub { get; set; }
        public int? RenewalsPolicyId { get; set; }
        public int? RenewalsPolicyDetailId { get; set; }
        public int? SettlementMethod { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? ApproveDate { get; set; }


        /// <summary>
        /// lợi tức tích lũy đến thời điểm hiện tại
        /// </summary>
        public decimal? ProfitNow { get; set; }

        /// <summary>
        /// Số ngày tích lũy đến thời điểm hiện tại
        /// </summary>
        public int? InvestmentDayNow { get; set; }
        public string GenContractCode { get; set; }
        public string ContractCodeSort { get; set; }

    }
}
