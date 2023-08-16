using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsApp
{
    /// <summary>
    /// DTO Cổ phần
    /// </summary>
    public class CpsDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Tên cổ phần
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Lợi tức
        /// </summary>
        public double Interest { get; set; }

        /// <summary>
        /// Mệnh giá
        /// </summary>
        public long ParValue { get; set; }

        /// <summary>
        /// Ngày phát hành
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Bảo lãnh thanh toán
        /// </summary>
        public string IsPaymentGuarantee { get; set; }

        /// <summary>
        /// Hạn mức còn lại
        /// </summary>
        public double RemainLimit { get; set; }
    }

    /// <summary>
    /// DTO tổ chức phát hành
    /// </summary>
    public class IssuerDto
    {
        public BusinessInfo BusinessInfo { get; set; }
        public FinanceInfo FinanceInfo { get; set; }
        public Contact Contact { get; set; }
    }

    /// <summary>
    /// thông tin doanh nghiệp
    /// </summary>
    public class BusinessInfo
    {
        public string CompanyName { get; set; }

        /// <summary>
        /// trụ sở chính
        /// </summary>
        public string Headquarters { get; set; }
        public string RepName { get; set; }
        public long Capital { get; set; }
    }

    /// <summary>
    /// thông tin tài chính
    /// </summary>
    public class FinanceInfo
    {
        public long Sales { get; set; }
        public long ProfitAfterTax { get; set; }
        public double ROA { get; set; }
        public double ROE { get; set; }
    }

    /// <summary>
    /// liên hệ
    /// </summary>
    public class Contact
    {
        public string WebSite { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    ///
    public class PolicyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Dung lượng
        /// </summary>
        public double Capacity { get; set; }

        /// <summary>
        /// Còn hiệu lực
        /// </summary>
        public string IsEffectStill { get; set; }

        /// <summary>
        /// Loại chính sách
        /// </summary>
        public int Classify { get; set; }

        /// <summary>
        /// Id cổ phần
        /// </summary>
        public int CpsId { get; set; }
    }

    /// <summary>
    /// DTO tiền tối thiểu và tiền tối đa của chính sách
    /// </summary>
    public class policyMoneyDto
    {
        public long MinMoney { get; set; }
        public long MaxMoney { get; set; }
        public int CpsId { get; set; }
    }

    /// <summary>
    /// DTO Loại chính sách và nhà đầu tư
    /// </summary>
    public class PolicyClassify
    {
        public int Classify { get; set; }
        public int InvestorId { get; set; }

        /// <summary>
        /// Được quyền đầu tư hay không
        /// </summary>
        public string IsInvest { get; set; }
    }

    /// <summary>
    /// DTO ưu đãi của nhà đầu tư thoe chính sách
    /// </summary>
    public class PromotionInvestor
    {
        public int InvestorId { get; set; }
        public int PolicyId { get; set; }
        public List<Promotion> Promotions { get; set; }
    }


    /// <summary>
    /// DTO Ưu đãi
    /// </summary>
    public class Promotion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long MoneyRequired { get; set; }
    }

    public class PolicyDetailDto
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public string Name { get; set; }
        public double Profit { get; set; }
        public double Interest { get; set; }
        public int Quantity { get; set; }

        /// <summary>
        /// Đơn vị kỳ hạn
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Chu kỳ nhận lợi tức
        /// </summary>
        public int InterestType { get; set; }
    }

    public class OrderSuccessDto
    {
        public long InvestMoney { get; set; }
        public int InvestQuantity { get; set; }
        public string CpsName { get; set; }
        public string ContractName { get; set; }
        public double Interest { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public int Classify { get; set; }
        public DateTime IssueDate { get; set; }
        public int InterestType { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class InvestOrderDto
    {
        public long InvestMoney { get; set; }
        public int InvestQuantity { get; set; }
        public string CpsName { get; set; }
        public double Profit { get; set; }
        public double Interest { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public int Classify { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        public long ParValue { get; set; }
        public int InterestType { get; set; }
        public double IncomeTotal { get; set; }
    }

    /// <summary>
    /// Tài khoản hưởng thụ
    /// </summary>
    public class BankAccountDto
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string isDefault { get; set; }
    }

    /// <summary>
    /// Giấy tờ tùy thân
    /// </summary>
    public class IdentificationDto
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string isDefault { get; set; }
    }

    /// <summary>
    /// Mã Giới thiệu
    /// </summary>
    public class ReferralCodeDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    public class AddressDto
    {
        public string Alias { get; set; }
        public string Address { get; set; }
        public string isDefault { get; set; }
    }

    public class MoreInformationDto
    {
        public IdentificationDto Identification { get; set; }

        /// <summary>
        /// nhận bản cứng
        /// </summary>
        public string GetHardCopy { get; set; }
        public ReferralCodeDto ReferralCode { get; set; }
        public AddressDto Address { get; set; }
    }

    // lợi tức sự kiến
    public class PlanInterest
    {
        public DateTime ReceiveDate { get; set; }
        public string Name { get; set; }
        public long Income { get; set; }
        public string Status { get; set; }
    }

    public class Contract
    {
        public string ContractName { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string LinkPDF { get; set; }
    }

    /// <summary>
    /// Tỉnh thành
    /// </summary>
    public class Province
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Province> Provinces { get; set; }
    }

    public class CreateAddressDto
    {
        public string provinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string Address { get; set; }
        public bool IsDefault { get; set; }
    }

    public class SupportBank
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string BankName { get; set; }
    }

    public class PayInformation
    {
        public string BankAccount { get; set; }
        public string Alias { get; set; }
        public string Owner { get; set; }

        /// <summary>
        /// số tiền giao dịch
        /// </summary>
        public long TransactionDeposit { get; set; }

        /// <summary>
        /// nội dung giao dịch
        /// </summary>
        public string TransactionContent { get; set; }
    }

    /// <summary>
    /// DTO màn hình thanh toán
    /// </summary>
    public class PaymentDto
    {
        public PayInformation payInformation { get; set; }
        public OrderSuccessDto OrderSuccessInfo { get; set; }
    }
}
