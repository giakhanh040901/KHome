using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class VerifyOTPDto
    {
        private string _emailOrPhone;
        private string _otp;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại hoặc email không được bỏ trống")]
        public string EmailOrPhone
        {
            get => _emailOrPhone;
            set => _emailOrPhone = value?.Trim();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "OTP không được bỏ trống")]
        public string OTP
        {
            get => _otp;
            set => _otp = value?.Trim();
        }
    }
}
