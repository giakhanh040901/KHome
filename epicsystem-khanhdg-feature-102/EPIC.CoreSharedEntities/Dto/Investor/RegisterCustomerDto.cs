using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class RegisterCustomerDto
    {
        private string _email;
        private string _phone;
        private string _address;
        private string _name;
        private string _password;

        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [MaxLength(10, ErrorMessage = "Số điện thoại không dài quá 10 ký tự")]
        [RegularExpression(RegexPatterns.PhoneNumber, ErrorMessage = " Số điện thoại bắt đầu bằng số 0 và chỉ được phép nhập số")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        public string Address
        {
            get => _address;
            set => _address = value?.Trim();
        }

        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }
    }
}
