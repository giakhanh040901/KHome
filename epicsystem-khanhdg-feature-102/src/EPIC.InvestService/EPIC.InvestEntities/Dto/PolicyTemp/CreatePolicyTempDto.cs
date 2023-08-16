using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPIC.InvestEntities.Dto.PolicyTemp
{
    public class CreatePolicyTempDto
    {
        private string _code;
        [Required(ErrorMessage = "Mã Chính sách không được bỏ trống")]
        [StringLength(100, ErrorMessage = "Mã chính sách không được dài hơn 50 ký tự")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        [Required(ErrorMessage = "Tên chính sách không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Tên chính sách không được dài hơn 50 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }


        [Required(ErrorMessage = "Vui lòng chọn kiểu chính sách sản phẩm")]
        public int Type { get; set; }


        [Required(ErrorMessage = "Thuế lợi nhuận % không được bỏ trống")]
        public decimal IncomeTax { get; set; }

        [Required(ErrorMessage = "Thuế chuyển nhượng không được bỏ trống")]
        public decimal TransferTax { get; set; }

        [Required(ErrorMessage = "Số tiền đầu tư tối thiểu không được bỏ trống")]
        public decimal Classify { get; set; }

        [Required(ErrorMessage = "Số tiền đầu tư tối thiểu không được bỏ trống")]
        public decimal MinMoney { get; set; }

        /// <summary>
        /// Số tiền đầu tư tối đa
        /// </summary>
        public decimal? MaxMoney { get; set; }

        private string _isTransfer;
        [StringLength(1, ErrorMessage = "Vui lòng chọn Có cho phép chuyển nhượng")]
        [Required(ErrorMessage = "Vui lòng chọn Có cho phép chuyển nhượng")]
        public string IsTransfer
        {
            get => _isTransfer;
            set => _isTransfer = value?.Trim();
        }
        public decimal? MinWithdraw { get; set; }
        public decimal? CalculateType { get; set; }

        [Required(ErrorMessage = "Phí rút vốn không được bỏ trống")]
        public decimal? ExitFee { get; set; }
        [Required(ErrorMessage = "kiểu của phí rút vốn không được bỏ trống")]
        public decimal? ExitFeeType { get; set; }

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

        private string _description;
        //[Required(ErrorMessage = "Mô tả chính sách không được bỏ trống")]
        //[StringLength(512, ErrorMessage = "Mô tả chính sách không được dài hơn 512 ký tự")]
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }

        [Required(ErrorMessage = "Thứ tự hiển thị kỳ hạn không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { Utils.ConstantVariables.Shared.PolicyDisplayOrder.SHORT_TO_LONG, Utils.ConstantVariables.Shared.PolicyDisplayOrder.LONG_TO_SHORT })]
        public int? PolicyDisplayOrder { get; set; }

        public List<CreatePolicyDetailTempDto> PolicyDetailTemp { get; set; }
    }

    public class CreatePolicyDetailTempDto
    {
        [Required(ErrorMessage = "Số thứ tự không được bỏ trống")]
        public int? Stt { get; set; }

        private string _shortName;
        [StringLength(50, ErrorMessage = "Tên viết tắt kỳ hạn không được dài qua 50 ký tự")]
        [Required(ErrorMessage = "Tên viết tắt kỳ hạn không được bỏ trống")]
        public string ShortName
        {
            get => _shortName;
            set => _shortName = value?.Trim();
        }

        private string _name;
        [StringLength(256, ErrorMessage = "Tên kỳ hạn không được dài qua 256 ký tự")]
        [Required(ErrorMessage = "Tên kỳ hạn không được bỏ trống")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        [Required(ErrorMessage = "Đơn vị không được bỏ trống")]
        public string PeriodType { get; set; }

        [Required(ErrorMessage = "Số kỳ đầu tư không được bỏ trống")]
        public int? PeriodQuantity { get; set; }

        [Required(ErrorMessage = "Lợi tức không được bỏ trống")]
        public decimal? Profit { get; set; }
        public int? InterestDays { get; set; }
        /// <summary>
        /// Kiểu trả lợi tức: 1 Định kỳ, 2 Cuối kỳ, 3 Ngày cố định hàng tháng, 4 Ngày đầu tháng, 5 Ngày cuối tháng
        /// </summary>
        public int? InterestType { get; set; }
        public int? InterestPeriodQuantity { get; set; }
        public string InterestPeriodType { get; set; }

        /// <summary>
        /// Ngày trả cố định khi kiểu trả lợi tức Ngày cố định
        /// </summary>
        [Range(1, 28, ErrorMessage = "Ngày chi trả cố định phải trong khoảng từ 1 đến 28")]
        public int? FixedPaymentDate { get; set; }
    }

    public class PolicyDetailTempDto : CreatePolicyDetailTempDto
    {
        public int PolicyTempId { get; set; }
    }
}
