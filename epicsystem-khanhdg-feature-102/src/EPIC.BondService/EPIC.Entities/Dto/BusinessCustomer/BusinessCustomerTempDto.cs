using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BusinessCustomer
{
    public class BusinessCustomerTempDto
    {
        public int BusinessCustomerTempId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string TradingAddress { get; set; }
        public string Nation { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public DateTime? LicenseDate { get; set; }
        public string LicenseIssuer { get; set; }
        public decimal? Capital { get; set; }
        public string RepName { get; set; }
        public string RepPosition { get; set; }
        public string DecisionNo { get; set; }
        public DateTime? DecisionDate { get; set; }
        public int? NumberModified { get; set; }
        public int? Status { get; set; }
        public string CifCode { get; set; }
        public DateTime? DateModified { get; set; }
        public string IsCheck { get; set; }
        public string BankAccName { get; set; }
        public string BankAccNo { get; set; }
        public string BankName { get; set; }
        public int BankId { get; set; }
        public string BankBranchName { get; set; }
        public string Website { get; set; }
        public string Fanpage { get; set; }
        public string BusinessRegistrationImg { get; set; }
        public string Server { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string AvatarImageUrl { get; set; }
        public string StampImageUrl { get; set; }
        public int? PartnerId { get; set; }
        public string AllowDuplicate { get; set; }
        public BusinessCustomerBankDto BusinessCustomerBank { get; set; }
        public List<BusinessCustomerBankDto> BusinessCustomerBanks { get; set; }
        public List<BusinessCustomerBankTempDto> BusinessCustomerBankTemps { get; set; }
    }
}
