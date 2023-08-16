using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreSaleTemp
    {
        public decimal Id { get; set; }
        public decimal? InvestorId { get; set; }
        public decimal? DepartmentId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public string EmployeeCode { get; set; }
        public decimal? SaleType { get; set; }
        public decimal? SaleParentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public decimal? Status { get; set; }
        public decimal? InvestorBankAccId { get; set; }
        public decimal? Source { get; set; }
        public decimal? SaleRegisterId { get; set; }
        public decimal? BusinessCustomerId { get; set; }
        public decimal? BusinessCustomerBankAccId { get; set; }
    }
}
