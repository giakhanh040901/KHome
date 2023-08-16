using EPIC.BondEntities.Dto.BondInfo;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.ProductBondPrimary;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DistributionContract
{
    public class DistributionContractDto : CreateDistributionContractDto
    {
        public decimal DistributionContractId { get; set; }
        public string ContractCode { get; set; }
        public string BondName { get; set; }
        public string TradingProviderName { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public ProductBondInfoDto ProductBondInfo { get; set; }
        public TradingProviderDto TradingProvider { get; set; }
        public ProductBondPrimaryDto BondPrimary { get; set; }
        public BusinessCustomerBankDto BusinessCustomerBank { get; set; }
        
    }
}
