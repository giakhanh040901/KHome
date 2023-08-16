using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpProductBondPrimary
    {
        public decimal BondPrimaryId { get; set; }
        public decimal? ProductBondId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? BusinessCustomerBankAccId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? BondTypeId { get; set; }
        public DateTime? OpenCellDate { get; set; }
        public DateTime? CloseCellDate { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? MinMoney { get; set; }
        public bool? PriceType { get; set; }
        public bool? PaymentType { get; set; }
        public decimal? MaxInvestor { get; set; }
        public string ContractCode { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string IsCheck { get; set; }
        public decimal? PartnerId { get; set; }
    }
}
