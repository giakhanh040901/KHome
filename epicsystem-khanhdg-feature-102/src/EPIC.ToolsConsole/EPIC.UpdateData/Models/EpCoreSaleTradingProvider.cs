using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreSaleTradingProvider
    {
        public decimal Id { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? SaleId { get; set; }
        public string EmployeeCode { get; set; }
        public decimal? SaleType { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public decimal? InvestorBankAccId { get; set; }
        public decimal? SaleParentId { get; set; }
        public string ContractCode { get; set; }
        public DateTime? DeactiveDate { get; set; }
        public decimal? BusinessCustomerBankAccId { get; set; }
    }
}
