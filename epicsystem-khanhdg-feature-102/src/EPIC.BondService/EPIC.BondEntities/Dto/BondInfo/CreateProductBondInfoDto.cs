using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondInfo
{
    public class CreateProductBondInfoDto
    {

        [Required(ErrorMessage = "Mã TCPH không được bỏ trống")]
        public int IssuerId { get; set; }

        [Required(ErrorMessage = "Mã DLLK không được bỏ trống")]
        public int DepositProviderId { get; set; }

        public int BondTypeId { get; set; }

        private string _bondCode;
        [Required(ErrorMessage = "Mã trái phiếu không được bỏ trống")]
        [StringLength(50, ErrorMessage = "Mã trái phiếu không được dài hơn 50 ký tự")]
        public string BondCode
        {
            get => _bondCode;
            set => _bondCode = value?.Trim();
        }

        private string _bondName;
        [Required(ErrorMessage = "Tên trái phiếu không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Tên trái phiếu không được dài hơn 256 ký tự")]
        public string BondName
        {
            get => _bondName;
            set => _bondName = value?.Trim();
        }

        private string _description;
        [StringLength(5000, ErrorMessage = "Mô tả loại trái phiếu không được dài hơn 5000 ký tự")]
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

        public decimal ParValue { get; set; }

        public int BondPeriod { get; set; }

        private string _bondPeriodUnit;
        [StringLength(1, ErrorMessage = "Đơn vị kỳ hạn không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { PeriodUnit.YEAR, PeriodUnit.MONTH, PeriodUnit.DAY }, ErrorMessage = "Vui lòng chọn Đơn vị kỳ hạn")]
        public string BondPeriodUnit
        {
            get => _bondPeriodUnit;
            set => _bondPeriodUnit = value?.Trim();
        }

        public decimal InterestRate { get; set; }

        public int? InterestPeriod { get; set; }

        private string _interestPeriodUnit;
        [StringLength(1, ErrorMessage = "Đơn vị kỳ hạn trả lãi không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { PeriodUnit.YEAR, PeriodUnit.MONTH, PeriodUnit.DAY }, ErrorMessage = "Vui lòng chọn Đơn vị kỳ hạn trả lãi")]
        public string InterestPeriodUnit
        {
            get => _interestPeriodUnit;
            set => _interestPeriodUnit = value?.Trim();
        }

        private string _interestType;
        [StringLength(1, ErrorMessage = "Loại lợi tức không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { INTEREST_TYPE.Coupon, INTEREST_TYPE.ZeroCoupon }, ErrorMessage = "Vui lòng chọn Loại lợi tức")]
        public string InterestType
        {
            get => _interestType;
            set => _interestType = value?.Trim();
        }

        public int InterestRateType { get; set; }

        private string _interestCouponType;
        [StringLength(1, ErrorMessage = "Kiểu lãi theo Coupon không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { INTEREST_Coupon_TYPE.ThaNoi, INTEREST_Coupon_TYPE.DinhKy }, ErrorMessage = "Vui lòng chọn Kiểu lãi theo Coupon")]
        public string InterestCouponType
        {
            get => _interestCouponType;
            set => _interestCouponType = value?.Trim();
        }

        private string _couponBondType;
        [StringLength(1, ErrorMessage = "Loại Coupon không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { COUPON_TYPE.Coupon, COUPON_TYPE.ZeroCoupon }, ErrorMessage = "Vui lòng chọn Loại Coupon")]
        public string CouponBondType
        {
            get => _couponBondType;
            set => _couponBondType = value?.Trim();
        }

        private string _isPaymentGuarantee;
        [StringLength(1, ErrorMessage = "Có bảo lãnh thanh toán không? không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO }, ErrorMessage = "Vui lòng chọn Có bảo lãnh thanh toán không?")]
        public string IsPaymentGuarantee
        {
            get => _isPaymentGuarantee;
            set => _isPaymentGuarantee = value?.Trim();
        }

        private string _allowSbd;
        [StringLength(1, ErrorMessage = "Có cho phép bán lại trước hạn không? không được dài hơn 1 ký tự")]
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO }, ErrorMessage = "Vui lòng chọn Có cho phép bán lại trước hạn không?")]
        public string AllowSbd
        {
            get => _allowSbd;
            set => _allowSbd = value?.Trim();
        }

        private string _policyPaymentContent;
        [StringLength(5000, ErrorMessage = "Mô tả chính sách gốc và lợi tức không được dài hơn 5000 ký tự")]
        public string PolicyPaymentContent
        {
            get => _policyPaymentContent;
            set => _policyPaymentContent = value?.Trim();
        }
        public long Quantity { get; set; }
        public int? AllowSbdMonth { get; set; }

        [Required(ErrorMessage = "Số ngày chốt quyền không được bỏ trống")]
        public int NumberClosePer { get; set; }

        public string IsCollateral { get; set; }

        public int? MaxInvestor { get; set; }

        public int CountType { get; set; }

        public string NiemYet { get; set; }

        public string IsCheck { get; set; }
        public string Icon { get; set; }
    }
}
