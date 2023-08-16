using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class CpsOrder : IFullAudited
    {
        public long Id { get; set; }
        public int TradingProviderId { get; set; }
        public string CifCode { get; set; }
        public int? DepartmentId { get; set; }
        public int? DistributionContractId { get; set; }
        public int? CpsId { get; set; }
        public int? SecondaryId { get; set; }
        public int? PolicyId { get; set; }
        public int? PolicyDetailId { get; set; }
        public decimal? TotalValue { get; set; }
        public DateTime? BuyDate { get; set; }
        public string IsInterest { get; set; }
        public string SaleReferralCode { get; set; }
        public int? Source { get; set; }
        public string ContractCode { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public int? InvestorBankAccId { get; set; }
        public DateTime? PaymentFullDate { get; set; }
        public decimal? Price { get; set; }
        public long? Quantity { get; set; }
        public int? Status { get; set; }
        public string IpAddressCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public int? InvestorIdenId { get; set; }
        public int? ContractAddressId { get; set; }
        public string DepartmentName { get; set; }
        public string DeliveryCode { get; set; }
        public int? DeliveryStatus { get; set; }
        public DateTime? InvestDate { get; set; }
        public int? SettlementMethod { get; set; }
        public int? RenewalsPolicyDetailId { get; set; }
        public int? SaleOrderId { get; set; }
        public int? DepartmentIdSub { get; set; }
        public string SaleReferralCodeSub { get; set; }
    }
}
