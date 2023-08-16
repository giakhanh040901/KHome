using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPIC.Entities.Dto.BusinessCustomer
{
    public class BusinessCustomerDto
    {
        public int Id { get; set; }
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
        public int? BankId { get; set; }
        public string BankBranchName { get; set; }
        public string Website { get; set; }
        public string Fanpage { get; set; }
        public string BusinessRegistrationImg { get; set; }
        public int? TradingProviderId { get; set; }
        public string Server { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string AvatarImageUrl { get; set; }
        public string StampImageUrl { get; set; }
        public string ReferralCodeSelf { get; set; }
        public string AllowDuplicate { get; set; }
        public string RepIdNo { get; set; }
        public DateTime? RepIdDate { get; set; }
        public string RepIdIssuer { get; set; }
        public string RepSex { get; set; }
        public string RepAddress { get; set; }
        public DateTime? RepBirthDate { get; set; }

        /// <summary>
        /// Có phải là thông tin doanh nghiệp của đại lý đang login hay không
        /// </summary>
        public bool IsAccountLogin { get; set; }
        public BusinessCustomerBankDto BusinessCustomerBank { get; set; }
        public List<BusinessCustomerBankDto> BusinessCustomerBanks { get; set; }
    }

    public class RequestBusinessCustomerDto
    {
        public int Id { get; set; }
        public int ActionType { get; set; }
        public string RequestNote { get; set; }
        public int UserApproveId { get; set; }
        public string Summary { get; set; }
    }

    public class ApproveBusinessCustomerDto
    {
        public int Id { get; set; }
        public int ApproveID { get; set; }
        public string ApproveNote { get; set; }
    }

    public class CheckBusinessCustomerDto
    {
        public int Id { get; set; }
        public int ApproveID { get; set; }
        public int? UserCheckId { get; set; }
    }

    public class CancelBusinessCustomerDto
    {
        public int Id { get; set; }
        public string CancelNote { get; set; }
    }
}
