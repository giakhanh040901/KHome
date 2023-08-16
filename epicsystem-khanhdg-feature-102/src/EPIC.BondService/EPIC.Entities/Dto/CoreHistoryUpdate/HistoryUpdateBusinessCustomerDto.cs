using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreHistoryUpdate
{
    public class HistoryUpdateBusinessCustomerDto
    {
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
        public DateTime? DateModified { get; set; }
        public string Deleted { get; set; }
        public string RepIdNo { get; set; }
        public DateTime? RepIdDate { get; set; }
        public string RepIdIssuer { get; set; }
        public string RepAddress { get; set; }
        public string RepSex { get; set; }
        public DateTime? RepBirthDate { get; set; }
        public string Website { get; set; }
        public string BusinessRegistrationImg { get; set; }
        public string Fanpage { get; set; }
        public string Server { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string AvatarImageUrl { get; set; }
        public string StampImageUrl { get; set; }
    }
}
