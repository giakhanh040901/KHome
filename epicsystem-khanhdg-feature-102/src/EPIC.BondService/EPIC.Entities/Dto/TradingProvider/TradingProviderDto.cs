using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.TradingProvider
{
    public class TradingProviderDto
    {
        public int? TradingProviderId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public int? Status { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ShortName { get; set; }
        public string RepName { get; set; }
        public string TaxCode { get; set; }
        public int PartnerId { get; set; }
        public string AliasName { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
    }

    public class ViewTradingProviderDto
    {
        public int TradingProviderId { get; set; }
        public int BusinessCustomerId { get; set; }
        public int PartnerId { get; set; }
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
        public string Status { get; set; }
        public string CifCode { get; set; }
        public DateTime? DateModified { get; set; }
        public string AliasName { get; set; }
    }
}
