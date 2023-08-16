using EPIC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class Order : IFullAudited
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string CifCode { get; set; }
        public int? DepartmentId { get; set; }
        public int? CpsId { get; set; }
        public int? CpsSecondaryId { get; set; }
        public int? CpsPolicyId { get; set; }
        public int? CpsPolicyDetailId { get; set; }
        public decimal? TotalValue { get; set; }
        public DateTime? BuyDate { get; set; }
        public string IsInterest { get; set; }
        public int Status { get; set; }
        public int? Source { get; set; }
        public string ContractCode { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public DateTime? PaymentFullDate { get; set; }
        public int? InvestorBankAccId { get; set; }
        public string SaleReferralCode { get; set; }
        public int? DeliveryStatus { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public string IpAddressCreated { get; set; }
        public int? InvestorIdenId { get; set; }
        public int? ContractAddressId { get; set; }
        public DateTime? RequestContractDate { get; set; }
        public string DeliveryCode { get; set; }
        public DateTime? ActivateDate { get; set; }
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
        public int? RenewalsPolicyDetailId { get; set; }
        public int? SettlementMethod { get; set; }
    }
}
