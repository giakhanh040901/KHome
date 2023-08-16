using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.PolicyTemp
{
    public class UpdatePolicyTempDto
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

        private string _isTransfer;
		[StringLength(1, ErrorMessage = "Vui lòng chọn Có cho phép chuyển nhượng")]
		[Required(ErrorMessage = "Vui lòng chọn Có cho phép chuyển nhượng")]
		public string IsTransfer
		{
			get => _isTransfer;
			set => _isTransfer = value?.Trim();
		}
		public int Id { get; set; }
		public decimal? MinWithDraw { get; set; }
		public decimal? CalculateType { get; set; }
		public decimal? ExitFee { get; set; }
		public decimal? ExitFeeType { get; set; }

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
    }
}
