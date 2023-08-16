using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpBondOrder
    {
        public decimal OrderId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public string CifCode { get; set; }
        public decimal? DepartmentId { get; set; }
        public decimal? ProductBondId { get; set; }
        public decimal? BondSecondaryId { get; set; }
        public decimal? BondPolicyId { get; set; }
        public decimal? BondPolicyDetailId { get; set; }
        public decimal? TotalValue { get; set; }
        public DateTime? BuyDate { get; set; }
        public string IsInterest { get; set; }
        public decimal? Status { get; set; }
        public decimal? Source { get; set; }
        public string ContractCode { get; set; }
        public decimal? BusinessCustomerBankAccId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public DateTime? PaymentFullDate { get; set; }
        public decimal? InvestorBankAccId { get; set; }
        public string SaleReferralCode { get; set; }
        public decimal? DeliveryStatus { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public string IpAddressCreated { get; set; }
        public decimal? InvestorIdenId { get; set; }
        public decimal? ContractAddressId { get; set; }
        public DateTime? RequestContractDate { get; set; }
        public string DeliveryCode { get; set; }
        public DateTime? ActiveDate { get; set; }
        public DateTime? InvestDate { get; set; }
        public DateTime? SettlementDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryDateModifiedBy { get; set; }
        public DateTime? FinishedDate { get; set; }
        public string FinishedDateModifiedBy { get; set; }
        public DateTime? PendingDate { get; set; }
        public string PendingDateModifiedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string ReceivedDateModifiedBy { get; set; }
        public decimal? SaleOrderId { get; set; }
        public decimal? DepartmentIdSub { get; set; }
        public decimal? RenewalsPolicyDetailId { get; set; }
        public string SaleReferralCodeSub { get; set; }
        public decimal? SettlementMethod { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? ApproveDate { get; set; }
    }
}
