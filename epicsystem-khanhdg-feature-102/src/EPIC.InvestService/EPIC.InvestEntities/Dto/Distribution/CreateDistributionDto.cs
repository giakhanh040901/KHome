using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class CreateDistributionDto
    {
        public int ProjectId { get; set; }
        public int TradingProviderId { get; set; }
        [MinLength(1, ErrorMessage = "Tài khoản thụ hưởng không được bỏ trống")]
        [Required(ErrorMessage = "Vui lòng chọn tài khoản thụ hưởng")]
        public List<int> TradingBankAcc { get; set; }
        public List<int> TradingBankAccPays { get; set; }
        public DateTime? OpenCellDate { get; set; }
        public DateTime? CloseCellDate { get; set; }
        /// <summary>
        /// Hình thức chi trả lợi tức, đáo hạn (1: có chi tiền, 2: không chi tiền)
        /// </summary>
        [IntegerRange(AllowableValues = new int[] { InvestMethodInterests.DoPayment, InvestMethodInterests.NoPayment },  ErrorMessage = "Vui lòng chọn hình thức chi trả lợi tức, đáo hạn")]
        public int MethodInterest { get; set; } = InvestMethodInterests.DoPayment;
    }
}
