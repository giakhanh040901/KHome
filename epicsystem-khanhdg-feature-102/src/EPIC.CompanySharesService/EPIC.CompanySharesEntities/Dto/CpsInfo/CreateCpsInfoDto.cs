using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsInfo
{
    public class CreateCpsInfoDto
    {
        [Required(ErrorMessage = "Mã TCPH không được bỏ trống")]
        public int IssuerId { get; set; }

        private string _cpsCode;
        [Required(ErrorMessage = "Mã cổ phần không được bỏ trống")]
        [StringLength(50, ErrorMessage = "Mã cổ phần không được dài hơn 50 ký tự")]
        public string CpsCode
        {
            get => _cpsCode;
            set => _cpsCode = value?.Trim();
        }

        private string _cpsName;
        [Required(ErrorMessage = "Tên cổ phần không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Tên cổ phần không được dài hơn 256 ký tự")]
        public string CpsName
        {
            get => _cpsName;
            set => _cpsName = value?.Trim();
        }

        private string _description;
        [StringLength(5000, ErrorMessage = "Mô tả loại cổ phần không được dài hơn 5000 ký tự")]
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }

        private string _content;
        [StringLength(5000, ErrorMessage = "Giới thiệu không được dài hơn 5000 ký tự")]
        public string Content
        {
            get => _content;
            set => _content = value?.Trim();
        }
        public DateTime? IssueDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int? ParValue { get; set; }

        public int? Period { get; set; }

        private string _periodUnit;
        [StringLength(1, ErrorMessage = "Đơn vị kỳ hạn không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { Utils.PeriodUnit.YEAR, Utils.PeriodUnit.MONTH, Utils.PeriodUnit.DAY }, ErrorMessage = "Vui lòng chọn Đơn vị kỳ hạn")]
        public string PeriodUnit
        {
            get => _periodUnit;
            set => _periodUnit = value?.Trim();
        }

        public decimal? InterestRate { get; set; }

        public int? InterestPeriod { get; set; }

        private string _interestPeriodUnit;
        [StringLength(1, ErrorMessage = "Đơn vị kỳ hạn trả lãi không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { Utils.PeriodUnit.YEAR, Utils.PeriodUnit.MONTH, Utils.PeriodUnit.DAY }, ErrorMessage = "Vui lòng chọn Đơn vị kỳ hạn trả lãi")]
        public string InterestPeriodUnit
        {
            get => _interestPeriodUnit;
            set => _interestPeriodUnit = value?.Trim();
        }

        public int? InterestRateType { get; set; }

        private string _isPaymentGuarantee;
        [StringLength(1, ErrorMessage = "Có bảo lãnh thanh toán không? không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO }, ErrorMessage = "Vui lòng chọn Có bảo lãnh thanh toán không?")]
        public string IsPaymentGuarantee
        {
            get => _isPaymentGuarantee;
            set => _isPaymentGuarantee = value?.Trim();
        }

        private string _isAllowSbd;
        [StringLength(1, ErrorMessage = "Có cho phép bán lại trước hạn không? không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO }, ErrorMessage = "Vui lòng chọn Có cho phép bán lại trước hạn không?")]
        public string IsAllowSbd
        {
            get => _isAllowSbd;
            set => _isAllowSbd = value?.Trim();
        }

        private string _policyPaymentContent;
        [StringLength(5000, ErrorMessage = "Mô tả chính sách gốc và lợi tức không được dài hơn 5000 ký tự")]
        public string PolicyPaymentContent
        {
            get => _policyPaymentContent;
            set => _policyPaymentContent = value?.Trim();
        }
        public long? Quantity { get; set; }
        public int? AllowSbdDay { get; set; }

        [Required(ErrorMessage = "Số ngày chốt quyền không được bỏ trống")]
        public int? NumberClosePer { get; set; }
        public string IsCollateral { get; set; }
        public int? MaxInvestor { get; set; }

        public int CountType { get; set; }
        private string _isListing;
        [StringLength(5000, ErrorMessage = "Trường này không được dài hơn 1 ký tự")]
        public string IsListing
        {
            get => _isListing;
            set => _isListing = value?.Trim();
        }
        public string IsCheck { get; set; }
        public string Icon { get; set; }
        public int? TotalInvestment { get; set; }
        public string HasTotalInvestmentSub { get; set; }
    }
}
