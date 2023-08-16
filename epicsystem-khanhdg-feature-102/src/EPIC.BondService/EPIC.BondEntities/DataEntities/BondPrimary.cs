using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondPrimary : IFullAudited
    {
        [Column(Name = "ID")]
        public int Id { get; set; }

        public int PartnerId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string AliasName { get; set; }

        [Column(Name = "BOND_ID")]
        public int BondId { get; set; }
        
        [Column(Name = "TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }

        [Column(Name = "BUSINESS_CUSTOMER_BANK_ACC_ID")]
        public int BusinessCustomerBankAccId { get; set; }
        
        [Column(Name = "CODE")]
        public string Code { get; set; }
        
        [Column(Name = "NAME")]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Obsolete("bỏ")]
        [Column(Name = "BOND_TYPE_ID")]
        public int? BondTypeId { get; set; }
        
        [Column(Name = "CONTRACT_CODE")]
        public string ContractCode { get; set; }
        
        [Column(Name = "OPEN_SELL_DATE")]
        public DateTime? OpenSellDate { get; set; }
        
        [Column(Name = "CLOSE_SELL_DATE")]
        public DateTime? CloseSellDate { get; set; }
        
        [Column(Name = "QUANTITY")]
        public long Quantity { get; set; }
        
        [Column(Name = "MIN_MONEY")]
        public decimal? MinMoney { get; set; }
        
        [Column(Name = "PRICE_TYPE")]
        public int PriceType { get; set; }
        
        [Column(Name = "PAYMENT_TYPE")]
        public int PaymentType { get; set; }
        
        [Column(Name = "MAX_INVESTOR")]
        public int? MaxInvestor { get; set; }
        
        [Required]        
        [Column(Name = "STATUS")]
        public string Status { get; set; }

        [Required]
        [Column(Name = "IS_CHECK")]
        public string IsCheck { get; set; }

        [Column(Name = "CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }
        
        [Column(Name = "CREATED_BY")]
        public string CreatedBy { get; set; }
        
        [Column(Name = "MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; }
        
        [Column(Name = "MODIFIED_BY")]
        public string ModifiedBy { get; set; }
        
        [Column(Name = "DELETED")]
        public string Deleted { get; set; }
    }
}
