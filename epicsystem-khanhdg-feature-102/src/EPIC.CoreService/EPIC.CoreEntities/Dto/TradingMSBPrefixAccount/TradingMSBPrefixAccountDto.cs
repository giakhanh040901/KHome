using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.TradingMSBPrefixAccount
{
    public class TradingMsbPrefixAccountDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int TradingBankAccountId { get; set; }
        public string PrefixMsb { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public BusinessCustomerBankDto businessCustomerBank { get; set; }
    }
}
