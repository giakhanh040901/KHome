using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.User
{
    public class ChangePasswordDto
    {
        private string _oldPassword;
        private string _newPassword;

        [Required(ErrorMessage = "Mật khẩu cũ không được bỏ trống")]
        /*[RegularExpression(RegexPatterns.Password8Characters1Uppercase1Lowercase1Number,
            ErrorMessage = "Mật khẩu ít nhất 8 ký tự gồm 1 chữ hoa 1 chữ thường và 1 số")]*/
        public string OldPassword
        {
            get => _oldPassword;
            set => _oldPassword = value?.Trim();
        }

        [Required(ErrorMessage = "Mật khẩu mới không được bỏ trống")]
        [RegularExpression(RegexPatterns.Password8Characters1Uppercase1Lowercase1Number,
            ErrorMessage = "Mật khẩu ít nhất 8 ký tự gồm 1 chữ hoa 1 chữ thường và 1 số")]
        public string NewPassword
        {
            get => _newPassword;
            set => _newPassword = value?.Trim();
        }
    }

    public class ChangePasswordTempDto
    {
        private string _password;

        [Required(ErrorMessage = "Mật khẩu mới không được bỏ trống")]
        [RegularExpression(RegexPatterns.Password8Characters1Uppercase1Lowercase1Number,
            ErrorMessage = "Mật khẩu ít nhất 8 ký tự gồm 1 chữ hoa 1 chữ thường và 1 số")]
        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }
    }
}
