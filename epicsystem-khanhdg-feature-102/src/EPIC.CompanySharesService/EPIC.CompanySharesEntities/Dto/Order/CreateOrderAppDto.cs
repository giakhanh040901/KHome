using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.Order
{
    public class CreateOrderAppDto : CheckOrderAppDto
    {
        private string _otp;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã OTP không được bỏ trống")]
        public string OTP
        {
            get => _otp;
            set => _otp = value?.Trim();
        }
    }
}
