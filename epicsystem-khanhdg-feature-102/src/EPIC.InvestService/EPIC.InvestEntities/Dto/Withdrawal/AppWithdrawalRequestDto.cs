using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Withdrawal
{
    public class AppWithdrawalRequestDto
    {
        [Required(ErrorMessage ="Id hợp đồng không được bỏ trống")]
        [Range(1, int.MaxValue)]
        public long? OrderId { get; set; }

        [Required(ErrorMessage = "Số tiền rút không được bỏ trống")]
        [Range(1, int.MaxValue)]
        public decimal? AmountMoney { get; set; }

        private string _otp;

        [Required(ErrorMessage = "Mã OTP không được bỏ trống")]
        public string Otp
        {
            get => _otp;
            set => _otp = value?.Trim();
        }
    }
}
