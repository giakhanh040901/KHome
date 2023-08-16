using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class UpdatePolicyDto
    {
        /// <summary>
        /// id chính sách nếu là 0 thì sẽ là thêm mới nếu khác 0 thì sẽ là cập nhật
        /// </summary>
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int DistributionId { get; set; }

        private string _code;
        [Required(ErrorMessage = "Mã chính sách không được để trống")]
        [StringLength(100, ErrorMessage = "Mã chính sách không được dài hơn 100 ký tự")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        [Required(ErrorMessage = "Tên chính sách không được để trống")]
        [StringLength(256, ErrorMessage = "Tên chính sách không được dài hơn 256 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        [Required(ErrorMessage = "Kiểu chính sách sản phẩm không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { InvPolicyType.FIX, InvPolicyType.FLEXIBLE, InvPolicyType.LIMIT, InvPolicyType.FIXED_PAYMENT_DATE })]
        public int? Type { get; set; }

        [Required(ErrorMessage = "Thuế lợi nhuận không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Thuế lợi nhuận phải lớn hơn 0")]
        public decimal? IncomeTax { get; set; }

        [Required(ErrorMessage = "Thuế chuyển nhượng không được bỏ trống")]
        public decimal? TransferTax { get; set; }

        [Required(ErrorMessage = "Số tiền đầu tư tối thiểu không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Tiền đầu tư tối thiểu phải lớn hơn 0")]
        public decimal? MinMoney { get; set; }

        /// <summary>
        /// Số tiền đầu tư tối đa
        /// </summary>
        public decimal? MaxMoney { get; set; }

        [Required(ErrorMessage = "Có cho phép chuyển nhượng không được bỏ trống")]
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsTransfer { get; set; }

        [Required(ErrorMessage = "Phân loại không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { INVPolicyClassify.FLASH, INVPolicyClassify.FIX, INVPolicyClassify.FLEX })]
        public int? Classify { get; set; }

        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public decimal? MinWithDraw { get; set; }
        public int? CalculateType { get; set; }
        public decimal? ExitFee { get; set; }
        public decimal? ExitFeeType { get; set; }
        public string IsShowApp { get; set; }

        [Required(ErrorMessage = "Thứ tự hiển thị kỳ hạn không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { Utils.ConstantVariables.Shared.PolicyDisplayOrder.SHORT_TO_LONG, Utils.ConstantVariables.Shared.PolicyDisplayOrder.LONG_TO_SHORT })]
        public int? PolicyDisplayOrder { get; set; }

        [Required(ErrorMessage = "loại tái tục không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { InvestRenewalsType.TAO_HOP_DONG_MOI, InvestRenewalsType.GIU_HOP_DONG_CU })]
        public int RenewalsType { get; set; }
        [Required(ErrorMessage = "Nhắc tái tục không được bỏ trống")]
        public int RemindRenewals { get; set; }
        [Required(ErrorMessage = "Hạn gửi yêu cầu tái tục không được bỏ trống")]
        public int ExpirationRenewals { get; set; }
        [Required(ErrorMessage = "Số tiền rút tối đa không được bỏ trống")]
        public decimal MaxWithDraw { get; set; }
        [Required(ErrorMessage = "Gửi yêu cầu nhận hợp đồng từ (VND) không được bỏ trống")]
        public decimal MinTakeContract { get; set; }
        public int MinInvestDay { get; set; }

        /// <summary>
        /// % Phần trăm lợi nhuận cố định: Nếu yêu cầu rút vốn chưa rơi vào kỳ hạn nào
        /// </summary>
        public decimal? ProfitRateDefault { get; set; }

        /// <summary>
        /// Cách tính lợi tức rút vốn: 1: Kỳ hạn thấp hơn gần nhất, 2: Giá trị cố định
        /// <see cref="InvestCalculateWithdrawTypes"/>
        /// </summary>
        [Required(ErrorMessage = "Cách tính lợi tức rút vốn không được bỏ trống")]
        public int CalculateWithdrawType { get; set; }
    }
}
