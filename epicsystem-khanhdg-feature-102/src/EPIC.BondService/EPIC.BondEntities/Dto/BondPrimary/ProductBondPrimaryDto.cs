using EPIC.BondEntities.Dto.BondInfo;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.TradingProvider;
using System;

namespace EPIC.Entities.Dto.ProductBondPrimary
{
    public class ProductBondPrimaryDto
    {
        public int? BondPrimaryId { get; set; }
        public int PartnerId { get; set; }
        public int? ProductBondId { get; set; }
        public int? TradingProviderId { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? BondTypeId { get; set; }
        public DateTime? OpenCellDate { get; set; }
        public DateTime? CloseCellDate { get; set; }
        public string ContractCode { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? MinMoney { get; set; }
        public int? PriceType { get; set; }
        public int? MaxInvestor { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public string BondName { get; set; }
        public string BondCode { get; set; }
        public string TradingProviderName { get; set; }
        public decimal? SoLuongTraiPhieuConLai { get; set; }
        public decimal? SoLuongTraiPhieuNamGiu { get; set; }
        public string IsCheck { get; set; }
        public decimal? SoLuongConLai { get; set; }
        public string AliasName { get; set; }
        public ProductBondInfoDto ProductBondInfo { get; set; }
        public TradingProviderDto TradingProvider { get; set; }
        public BusinessCustomerBankDto BusinessCustomerBank { get; set; }
    }
}
