using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class ResetPasswordDto
    {
        private string _emailOrPhone;
        private string _resetPasswordToken;
        private string _password;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại hoặc email không được bỏ trống")]
        public string EmailOrPhone 
        { 
            get => _emailOrPhone;
            set => _emailOrPhone = value?.Trim(); 
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Token không được bỏ trống")]
        public string ResetPasswordToken
        {
            get => _resetPasswordToken;
            set => _resetPasswordToken = value?.Trim();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được bỏ trống")]
        [RegularExpression(RegexPatterns.Password8Characters1Uppercase1Lowercase1Number,
            ErrorMessage = "Mật khẩu ít nhất 8 ký tự gồm 1 chữ hoa 1 chữ thường và 1 số")]
        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }
    }

    public class ResetPasswordManagerInvestorDto
    {
        private string _emailOrPhone;
        private string _password;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại hoặc email không được bỏ trống")]
        public string EmailOrPhone
        {
            get => _emailOrPhone;
            set => _emailOrPhone = value?.Trim();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được bỏ trống")]
        [RegularExpression(RegexPatterns.Password8Characters1Uppercase1Lowercase1Number,
            ErrorMessage = "Mật khẩu ít nhất 8 ký tự gồm 1 chữ hoa 1 chữ thường và 1 số")]
        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }
    }
}
