using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class CreatePolicyDto
    {
		public int TradingProviderId { get; set; }
		public int DistributionId { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public int? Type { get; set; }
		public decimal? IncomeTax { get; set; }
		public decimal? TransferTax { get; set; }
		public int? Classify { get; set; }
		public decimal? MinMoney { get; set; }
		public string IsTransfer { get; set; }

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
